using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour ,ISaveable
    {
        [SerializeField]float health =100f;
         private Animator anim;
        private bool dead = false; 


        public bool IsDead()
        {
            return dead;
        }
        private void Awake()
        {
            
            anim = GetComponent<Animator>();
        }
        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (health == 0 )
            {
                Die();
            }
        }

        private void Die()
        {
                if(dead)return;
                anim.SetTrigger("Die");
                dead = true;
                GetComponent<ActionScheduler>().CancelCurrentAction();
            
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
             health= (float)state;
            if(health==0){
                Die();
            }

        }
    }

}