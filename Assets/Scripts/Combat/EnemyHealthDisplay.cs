using RPG.Attributes;
using RPG.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{

    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;


        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }


        private void Update()
        {

            if (fighter.GetTarget() == null)
            {
                GetComponent<TMP_Text>().text = "N/A";
                return;
            }
            else
            {
                GetComponent<TMP_Text>().text = String.Format("{0:0}/{1:0}", fighter.GetTarget().GetHealthPoints().ToString(),fighter.GetTarget().GetMaxHealth());

            }
        }

    }

}