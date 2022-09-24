using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;
using System;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, IModifierProvider
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfiguration defaultWeaponConfig = null;
        [SerializeField] int readyCooldownTime = 2;
        FieldOfView fov;
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        WeaponConfiguration currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;
        Animator animator = null;

        public Animator GetFighterAnimator()
        {
            return animator;
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            fov = GetComponent<FieldOfView>();
            currentWeaponConfig = defaultWeaponConfig;
            currentWeapon = new LazyValue<Weapon>(SetUpDefaultWeapon);
        }
        private Weapon SetUpDefaultWeapon()
        {
            return SetUpWeapon(defaultWeaponConfig);
        }
        private void Start() 
        {
            currentWeapon.ForceInit();
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
            
            if (!GetIsInAttackRange(target.transform))
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
                StopAttack();
            }
            else if(GetIsInAttackRange(target.transform))
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }
        //=====Weapon Config=====
        public void EquipWeapon(WeaponConfiguration weaponConfiguration)
        {
            currentWeaponConfig = weaponConfiguration;
            currentWeapon.value = SetUpWeapon(weaponConfiguration);
        }
        public Weapon SetUpWeapon(WeaponConfiguration weaponConfiguration)
        {
            // print("set up "+ weaponConfiguration +" for "+gameObject.name + weaponConfiguration.IsRightHanded());
            if(weaponConfiguration.IsRightHanded()) 
                animator.SetBool("hasComboAttack", true);
            else  animator.SetBool("hasComboAttack", false);
            return weaponConfiguration.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        //=====Behaviours=====
        private bool CombatBehaviourStatus()
        {
          
            if(!currentWeaponConfig.IsRightHanded() && currentWeaponConfig.GetWeaponRange() > fov.radius)
                fov.ReconfigFOVRadius(this.currentWeaponConfig.GetWeaponRange());

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
        
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            if(!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) &&
               !GetIsInAttackRange(target.transform))
                 return false;

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
        

        //=====Animation=====
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
        private bool GetIsInAttackRange(Transform target)
        {
            return Vector3.Distance(transform.position, target.position) < currentWeaponConfig.GetWeaponRange() && fov.canSeeTarget;
        }

        //=====Animation Event=====
        void Hit()
        {
            if(target == null) return;
            float damage = GetComponent<BaseStats>().GetStats(Stats.Stats.damage);
            
            //melee weapon
            if(currentWeapon.value != null)
                currentWeapon.value.Hit();

            //Range weapon
            if(currentWeaponConfig.HasProjectile())
                currentWeaponConfig.LaunchProjectile(rightHandTransform ,leftHandTransform ,target , this.gameObject, damage);
            else
                target.TakeDamage(this.gameObject, GetComponent<BaseStats>().GetStats(Stats.Stats.damage));

        }

        public void Shoot()
        {
            Hit();
        }
        
       
        //Get the target
        public Health GetTarget()
        {
            return target;
        }

       
        //=====State modifier=====
        public IEnumerable<float> GetAdditiveModifier(Stats.Stats stat)
        {
            if(stat == Stats.Stats.damage)
            {
                yield return currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stats.Stats stat)
        {
            if(stat == Stats.Stats.damage)
            {
                yield return currentWeaponConfig.GetPercentageDamageBonus();
            }
        }
    }
}