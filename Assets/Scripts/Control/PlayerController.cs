using System;
using System.Collections;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static UnityEngine.ParticleSystem;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavmeshProjectingDistance = 1f;
        public float speedFraction = 1f;
        private SkillManager skillManager = null;
        private bool CanHoverOver = false;
        private Health health;

        public SkillManager SkillManager
        {
            get { return skillManager; }
        }
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType cursor;
            public Texture2D texture;
            public Vector2 spot;
        }
        public void SetHoverPosibility(bool status)
        {
            CanHoverOver = status;
        }
        
        private void Awake() {
            health = GetComponent<Health>();
            skillManager = GetComponent<SkillManager>();
        }

        private void Update()
        {
            if(InteractWithUI()) return;
            if (health.IsDead()) 
            {
                SetCursor(CursorType.none);
                return;
            }
            if(InteractWithStuff()) return;
            if(InteractWithSkill()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.none);
        }
        
        //==========Stuff_Interaction=========//
        private bool InteractWithStuff()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach(RaycastHit hit in hits)
            {
                IRaycastable[] raycastable = hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable raycast in raycastable)
                {
                    if(raycast.HandleRaycast(this))
                    {
                        SetCursor(raycast.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }
        //==========Skill_Interaction=========//
        private bool IsInSkillAnimation(int keycode)
        {
            if(keycode < 0) return false;

            return skillManager.CheckSkillIsInAnimationState(keycode);
        }
        
        private bool InteractWithSkill()
        {
            int keycode = -1;
            for(int i = 0; i < 9; i++)
            {
                if(Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    keycode = i;
                    skillManager.PlaySkillAnimation(i);
                }
            }
            return false;
        }
        
        //==========UI_Interaction=========//
        private bool InteractWithUI()
        {
            if(EventSystem.current.IsPointerOverGameObject() && CanHoverOver == false)
            {
                SetCursor(CursorType.UI);
                return true;
            }

            return false;
        }
        
        //==========Movement_Interaction=========//
        private bool InteractWithMovement()
        {
            Vector3 target = new Vector3();;
            
            bool hasHit = RaycastNavmesh(ref target);
            if (hasHit)
            {
                if(!GetComponent<Mover>().CanMoveTo(target))
                    return false;
#if UNITY_IOS || UNITY_ANDROID
                foreach (Touch touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                    {
                        GetComponent<Mover>().StartMoveAction(target, speedFraction);
                    }
                }
#endif
                if (Input.GetMouseButton(1))
                {
                    GetComponent<Mover>().StartMoveAction(target, speedFraction);
                }

                SetCursor(CursorType.movement);
                return true;
            }
            return false;
        }
        //==========Raycast=========//
        private RaycastHit[] RaycastAllSorted()
        {
            //Get all hits
            RaycastHit[] hits =  Physics.RaycastAll(GetMouseRay());
            //Build distances array
            float[] distances = new float[hits.Length];
            for(int i=0; i<distances.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            //sort by raycasthit
            Array.Sort(distances, hits);
            return hits;
        }
        private bool RaycastNavmesh(ref Vector3 target)
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if(!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasNavmeshCasted = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavmeshProjectingDistance, NavMesh.AllAreas);
            
            if(!hasNavmeshCasted) return false;
            target = navMeshHit.position;
            
            return true;
        }

        

        //==========Cursor_setup=========//
        private void SetCursor(CursorType cursor)
        {
            CursorMapping cursorMapping = GetCursorMapping(cursor);
            Cursor.SetCursor(cursorMapping.texture, cursorMapping.spot, CursorMode.Auto);
        }
        private CursorMapping GetCursorMapping(CursorType cursor)
        {
            foreach(CursorMapping mapping in cursorMappings)
            {
                if(mapping.cursor ==  cursor)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
       
    }
}