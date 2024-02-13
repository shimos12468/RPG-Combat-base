using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics{
    public class CinematicTrigger : MonoBehaviour ,ISaveable
    {

        private bool triggerd =false;

        public object CaptureState()
        {
           return triggerd;
        }

        public void RestoreState(object state)
        {
           triggerd =(bool)state;
        }

        private void OnTriggerEnter(Collider other) {
                if(other.gameObject.tag=="Player"&&!triggerd){

                        triggerd =true;
                    GetComponent<PlayableDirector>().Play();
                }
            }
    }

}