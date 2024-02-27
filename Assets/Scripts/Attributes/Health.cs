using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour ,ISaveable
    {
        [SerializeField] float regenPercentage = 70;
        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent onDie;
        LazyValue<float>health;
        private Animator anim;
        private bool dead = false;

        BaseStats baseStats = null;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            baseStats = GetComponent<BaseStats>();
            health = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return baseStats.GetStat(Stat.Health);
        }

        private void Start()
        {
          health.ForceInit();
        }

        private void OnEnable()
        {
            baseStats.onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            baseStats.onLevelUp -= RegenerateHealth;
        }
        public bool IsDead()
        {
            return dead;
        }

   

        public void TakeDamage(GameObject instigator, float damage)
        {
           
            health.value = Mathf.Max(health.value - damage, 0);
            takeDamage?.Invoke(damage);
            if (health.value == 0)
            {
                onDie.Invoke();
                Die();

                AwardExperiancec(instigator);
            }
        }


        public float GetHealthPoints()
        {
            return health.value;
        }
        public float GetMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            
            return 100*GetFraction();
        }
        public float GetFraction()
        {

            return (health.value / GetComponent<BaseStats>().GetStat(Stat.Health));
        }


        public object CaptureState()
        {
            return health.value;
        }
        public void RestoreState(object state)
        {
            health.value = (float)state;
            if(health.value == 0){
                Die();
            }

        }
        private void AwardExperiancec(GameObject instigator)
        {
            Experiance experiance = instigator.GetComponent<Experiance>();
            if (experiance != null)
            {
                experiance.GainExperiance(GetComponent<BaseStats>().GetStat(Stat.XP));
            }

        }
        private void Die()
        {
            if (dead) return;
            anim.SetTrigger("Die");
            dead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();

        }

        private void RegenerateHealth()
        {
            float healthRegen = GetComponent<BaseStats>().GetStat(Stat.Health)* regenPercentage / 100;
            health.value = Mathf.Max(health.value, healthRegen);
        }

        public void Heal(float healthToRestore)
        {
            health.value =Mathf.Min(health.value+healthToRestore,GetMaxHealth());
        }
    }

}