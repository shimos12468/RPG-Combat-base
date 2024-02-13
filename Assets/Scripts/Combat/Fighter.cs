using RPG.Core;
using RPG.Movment;
using RPG.Saving;
using System;
using UnityEngine;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour,IAction,ISaveable
    {
         Health target;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] Transform rightHandPosition= null;
        [SerializeField] Transform leftHandPosition = null;
        string defaultWeaponName = "Unarmed";
        Weapon currentWeapon= null;
        Animator anim;
        private float timeSinceLastAttack = 1f;


        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            
        }
        private void Start()
        {
            if (currentWeapon == null)
            {

                EquipeWeapon(defaultWeapon);
            }
        }

        public void EquipeWeapon(Weapon weapon)
        {
            currentWeapon=weapon;
            weapon.Spawn(rightHandPosition,leftHandPosition, anim);
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

        //Animation event
        void Hit()
        {
            if (target == null)return;


            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandPosition, leftHandPosition, target);
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
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
        }

        public object CaptureState()
        {
            return currentWeapon.name;
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