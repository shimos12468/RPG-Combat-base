using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace RPG.Attributes{
    public class HealthBar : MonoBehaviour
    {

        [SerializeField] RectTransform rect;

        [SerializeField]Health health;
        [SerializeField]Canvas rootCanvas;

        void Update()
        {
            
            if(Mathf.Approximately(health.GetFraction(),0)|| Mathf.Approximately(health.GetFraction(), 1))
            {
                rootCanvas.enabled = false;
                return;
            }
            rootCanvas.enabled = true;
            rect.localScale = new Vector3 (health.GetFraction(), 1f, 1f);
        }
    }

}