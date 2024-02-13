using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{

    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] GameObject target;

        private void Start() {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = target.transform.position;
        }
    }

}