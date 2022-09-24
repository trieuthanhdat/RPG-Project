using UnityEngine;
using RPG.Resources;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.combat;
        }

        public bool HandleRaycast(PlayerController player)
        {
            if (!player.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                 player.GetComponent<Fighter>().Attack(gameObject);
            }
            return true;
        }
    }
}