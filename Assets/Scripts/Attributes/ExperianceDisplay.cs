using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class ExperianceDisplay : MonoBehaviour
    {
        Experiance experiance;

        private void Awake()
        {
            experiance = GameObject.FindWithTag("Player").GetComponent<Experiance>();
        }


        private void Update()
        {
            GetComponent<TMP_Text>().text = String.Format("{0:0}", experiance.ExperiancePoints);
        }
    }

}