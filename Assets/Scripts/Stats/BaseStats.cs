using GameDevTV.Utils;
using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int level = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression;
        [SerializeField] GameObject levelUpParticleEffect;
        [SerializeField] bool shouldUseModifiers = false;
        public event Action onLevelUp; 
        LazyValue<int> currentLevel;
        Experiance experiance;
        private void Awake()
        {
            experiance = GetComponent<Experiance>();
            currentLevel = new LazyValue<int>(GetInitialCurrentLevel);

        }

        private int GetInitialCurrentLevel()
        {
            return CalculateLevel();
        }

        private void Start()
        {
            currentLevel.ForceInit(); 
        }

        private void OnEnable()
        {
            if (experiance != null)
            {
                experiance.OnExperianceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experiance != null)
            {
                experiance.OnExperianceGained -= UpdateLevel;
            }
        }
        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {


                currentLevel.value = newLevel;
                onLevelUp?.Invoke();
                LevelUpEffect();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + AdditiveModifiers(stat))*(1+(PercentageModifiers(stat)/100));
        }

       

        private float GetBaseStat(Stat stat)
        {
            
            return progression.GetStat(characterClass, stat, GetLevel());
        }

        private float AdditiveModifiers(Stat stat)
        {
           if(!shouldUseModifiers)return 0;

           float totalAdditaveBouns =0;
           foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
           {
                foreach(var bouns in provider.GetAdditiveModifiers(stat))
                {
                    totalAdditaveBouns += bouns;
                }
           }
           return totalAdditaveBouns;
        }


        private float PercentageModifiers(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float totalePercentageBouns = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (var bouns in provider.GetPercentageModifiers(stat))
                {
                    totalePercentageBouns += bouns;
                }
            }
            return totalePercentageBouns;
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }


        private int CalculateLevel()
        {
           Experiance experiance = GetComponent<Experiance>();
            if (experiance == null)
            {
               
                return level;
            }
           float currentXP= experiance.ExperiancePoints;
           int penultimateLevel = progression.GetLevels(characterClass,Stat.XPToLevelUp);
           for (int i = 1; i <= penultimateLevel; i++)
            {
                
                float XPToLevelUp = progression.GetStat(characterClass, Stat.XPToLevelUp, i);
                if (XPToLevelUp > currentXP)
                {
                    
                    return i;
                }
            }

            return penultimateLevel + 1;
        }
    }

}