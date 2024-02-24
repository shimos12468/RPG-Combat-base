using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.stats
{

    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/ New Progression", order = 0)]
    public class Progression : ScriptableObject
    {

        [SerializeField] ProgressionCharacterClass[] characterClasses;


        public float GetHealth(CharacterClass cc ,int level)
        {

           foreach (var c in characterClasses)
           {
                if (c.characterClass == cc)
                {
                    return c.health[level-1];
                }
           }
           return 0;
        }


        [System.Serializable]
        class ProgressionCharacterClass
        {
             public CharacterClass characterClass;
             public List<float> health;
        }

    }
}
