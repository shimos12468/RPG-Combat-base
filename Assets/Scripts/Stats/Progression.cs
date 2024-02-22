using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.stats
{

    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/ New Progression", order = 0)]
    public class Progression : ScriptableObject
    {

        [SerializeField] ProgressionCharacterClass[] characterClasses;

        [System.Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] CharacterClass characterClass;
            [SerializeField] List<float> health;
        }

    }
}
