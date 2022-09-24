using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] int readyCooldownTime = 2;
        FieldOfView fov;
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        Weapon currentWeapon = null;
        Animator animator = null;

        public Animator GetFighterAnimator()
        {
            return animator;
        }
        private void Start() 
        {
            
            if(currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);    
            }
            animator = GetComponent<Animator>();
            fov = GetComponent<FieldOfView>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if(ReadyToBattle())
                EnableBattlingStand();
            else
            {
                StopAttack();
                DisableBattlingStand();
            }

            if(CombatBehaviourStatus() == false) 
            {
                StopAttack();
                return;
            }
            
            if (!GetIsInAttackRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
                StopAttack();
            }
            else if(GetIsInAttackRange())
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            if(weapon.IsRightHanded()) animator.SetBool("isRightHanded", true);
            else animator.SetBool("isRightHanded", false);
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }
        private bool CombatBehaviourStatus()
        {
            if(target == null) return false;
            if(fov.GetRangeTargets != null) return true;
            if(target.IsDead()) return false;

            return true;
        }
       
        private void AttackBehaviour()
        {

            transform.LookAt(target.transform);
            
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // This will trigger the Hit() event.
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }
        private void Timing()
        {
            float timer = 0;
            while(timer <= readyCooldownTime)
            {
                timer += Time.deltaTime;
            }
        }
        private void EnableBattlingStand()
        {
           Timing();
            animator.SetBool("isReady", true);
        }
        private void DisableBattlingStand()
        {
            Timing();
            animator.SetBool("isReady", false);
        }
        private void TriggerAttack()
        {
            animator.SetBool("isCombo", true);
        }
        private void StopAttack()
        {
            animator.SetBool("isCombo", false);
        }

        // Animation Event
        void Hit()
        {
            if(target == null) { return; }
            
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            }
            else
            {
                target.TakeDamage(currentWeapon.GetDamage());
            }
        }

        void Shoot()
        {
            Hit();
        }
        private bool ReadyToBattle()
        {
            if(fov.GetRangeTargets != null && fov.canSeeTarget)
            {
                if(target != null)
                {
                    if(target.IsDead())
                        return false;
                }
                return true;

            }
            return false;
        }
       
        private bool GetIsInAttackRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string) state;
            print(weaponName);
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}