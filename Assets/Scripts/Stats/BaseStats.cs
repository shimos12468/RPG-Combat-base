using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.stats
{
    public partial class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int level = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression;  
    }

}