using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Saving;
using RPG.stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour ,ISaveable
    {
        [SerializeField]float health =100f;
         private Animator anim;
        private bool dead = false;


        private void Start()
        {
            health = GetComponent<BaseStats>().GetHealth();
        }

        public bool IsDead()
        {
            return dead;
        }
        private void Awake()
        {
            
            anim = GetComponent<Animator>();
        }
        public void TakeDamage( GameObject instigator,float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (health == 0 )
            {
                Die();

                AwardExperiancec(instigator);
            }
        }

        private void AwardExperiancec(GameObject instigator)
        {
             Experiance experiance= instigator.GetComponent<Experiance>();
             if (experiance != null )
            {
                experiance.GainExperiance(GetComponent<BaseStats>().GetXP());
            }
        
        }

        public float GetPercentage()
        {
            return 100*(health/ GetComponent<BaseStats>().GetHealth());
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