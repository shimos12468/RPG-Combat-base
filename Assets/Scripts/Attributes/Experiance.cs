using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class Experiance : MonoBehaviour ,ISaveable
    {
        public float ExperiancePoints { get { return experiancePoints; } }
        [SerializeField] float experiancePoints = 0;

        public event Action OnExperianceGained;

        public void GainExperiance(float experiance)
        {
            experiancePoints += experiance;
            OnExperianceGained?.Invoke();
        }

        public object CaptureState()
        {
            return experiancePoints;
        }

        public void RestoreState(object state)
        {
            experiancePoints = (float)state;
        }
    }

}