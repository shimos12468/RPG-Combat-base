using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Core;
using RPG.Movment;
using RPG.Saving;
using RPG.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour,IAction,ISaveable,IModifierProvider
    {
        Health target;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] WeaponConfig defaultWeapon = null;
        [SerializeField] Transform rightHandPosition= null;
        [SerializeField] Transform leftHandPosition = null;
        string defaultWeaponName = "Unarmed";
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;
        Animator anim;
        private float timeSinceLastAttack = 1f;


        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);

        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null)return;
            if (target.IsDead()) return;

            if (!GetIsInRange(target.transform))
            {
                
                GetComponent<Mover>().MoveTo(target.transform.position,1f);
            }
            else
            {

                GetComponent<Mover>().Cancel();
                AttackingBehaviour();
            }
        }


        public void EquipeWeapon(WeaponConfig weaponConfig)
        {
            currentWeaponConfig = weaponConfig;
            currentWeapon.value = AttachWeapon(weaponConfig);
        }

        private Weapon AttachWeapon(WeaponConfig weaponConfig)
        {
            return weaponConfig.Spawn(rightHandPosition, leftHandPosition, anim);
        }

        public Health GetTarget()
        {
            return target;
        }

        public bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.transform.position) < currentWeaponConfig.GetRange();
        }
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) && !GetIsInRange(combatTarget.transform)) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        private void AttackingBehaviour()
        {
            transform.LookAt(target.transform.position);
            if (timeSinceLastAttack>timeBetweenAttacks)
            {
                //this will trigger the Hit() event
                TriggerAttack();
                timeSinceLastAttack = 0f;

            }
        }

        private void TriggerAttack()
        {
            anim.ResetTrigger("StopAttack");
            anim.SetTrigger("Attack");
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBouns();
            }
        }


        //Animation event
        void Hit()
        {
            if (target == null)return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }
            
            
            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandPosition, leftHandPosition, target ,gameObject , damage);
            }
            else
            {
                target.TakeDamage(gameObject , damage);
            }
            
        }

        void Shoot()
        {
            Hit();
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

        private void StopAttack()
        {
            anim.ResetTrigger("Attack");
            anim.SetTrigger("StopAttack");
        }

        

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            defaultWeaponName = state.ToString();
            WeaponConfig equipedWeapon = Resources.Load<WeaponConfig>(defaultWeaponName);
            EquipeWeapon(equipedWeapon);
        }

        
    }
}