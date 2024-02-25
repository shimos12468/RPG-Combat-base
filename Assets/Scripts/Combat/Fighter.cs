using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Core;
using RPG.Movment;
using RPG.Saving;
using RPG.stats;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour,IAction,ISaveable,IModifierProvider
    {
        Health target;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] Transform rightHandPosition= null;
        [SerializeField] Transform leftHandPosition = null;
        string defaultWeaponName = "Unarmed";
        LazyValue<Weapon> currentWeapon;
        Animator anim;
        private float timeSinceLastAttack = 1f;


        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
            
        }

        private Weapon SetupDefaultWeapon()
        {
           AttachWeapon(defaultWeapon);
           return defaultWeapon;
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        public void EquipeWeapon(Weapon weapon)
        {
            currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(Weapon weapon)
        {
            weapon.Spawn(rightHandPosition, leftHandPosition, anim);
        }

        public Health GetTarget()
        {
            return target;
        }


        public bool CanAttack(GameObject target)
        {
            if(target == null) return false;
            return target.GetComponent<Health>()!=null&&!target.GetComponent<Health>().IsDead();
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null)return;
            if (target.IsDead()) return;

            if (!GetIsInRange())
            {
                
                GetComponent<Mover>().MoveTo(target.transform.position,1f);
            }
            else
            {

                GetComponent<Mover>().Cancel();
                AttackingBehaviour();
            }
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
                yield return currentWeapon.value.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetPercentageBouns();
            }
        }


        //Animation event
        void Hit()
        {
            if (target == null)return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            print(damage);
            if (currentWeapon.value.HasProjectile())
            {
                currentWeapon.value.LaunchProjectile(rightHandPosition, leftHandPosition, target ,gameObject , damage);
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

        public bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.value.GetRange();
        }

        public object CaptureState()
        {
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            defaultWeaponName= state.ToString();

            print(defaultWeaponName);
            print(gameObject.name);
            Weapon equipedWeapon = Resources.Load<Weapon>(defaultWeaponName);
            EquipeWeapon(equipedWeapon);
        }

        
    }
}