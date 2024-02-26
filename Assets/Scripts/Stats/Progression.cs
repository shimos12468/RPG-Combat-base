using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{

    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/ New Progression", order = 0)]
    public class Progression : ScriptableObject
    {

        [SerializeField] ProgressionCharacterClass[] characterClasses;
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> LookupTable = null;

        
        public float GetStat(CharacterClass characterClass,Stat stat ,int level)
        {
           BuildLookup();

            float[] levels = LookupTable[characterClass][stat];
            if (levels.Length < level)
            {
                return 0;
            }
                
           return levels[level-1];
        }

        public void BuildLookup()
        {
            if (LookupTable != null) return;


            LookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (var characterClass in characterClasses)
            {

                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach(var progressionStat in characterClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }
                LookupTable[characterClass.characterClass] = statLookupTable;

            }

        }


        public int GetLevels(CharacterClass characterClass, Stat stat)
        {
            BuildLookup();
            return LookupTable[characterClass][stat].Length;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
             public CharacterClass characterClass;
             public ProgressionStat [] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;

        }
    }
}
