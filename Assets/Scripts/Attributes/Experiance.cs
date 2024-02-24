using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class Experiance : MonoBehaviour
    {
        public float ExperiancePoints { get { return experiancePoints; } }
        [SerializeField] float experiancePoints = 0;
        public void GainExperiance(float experiance)
        {
            experiancePoints += experiance;
        }

    }

}