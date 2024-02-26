using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats;
        void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        // Update is called once per frame
        void Update()
        {

            GetComponent<TMP_Text>().text = string.Format("{0:0}", baseStats.GetLevel());

        }
    }

}