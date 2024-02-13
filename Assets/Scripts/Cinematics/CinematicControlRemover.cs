using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics{



    public class CinematicControlRemover : MonoBehaviour
    {

        GameObject player;
        private void Start() {

            GetComponent<PlayableDirector>().played += EnableControl;
            GetComponent<PlayableDirector>().stopped+= DisableControl;
            player = GameObject.FindWithTag("Player");
        }
       void EnableControl(PlayableDirector pd){
             player.GetComponent<ActionScheduler>().CancelCurrentAction();
             player.GetComponent<PlayerController>().enabled =false;
       }
       void DisableControl(PlayableDirector pd)
        {  
            player.GetComponent<PlayerController>().enabled = true;
        }
    }

}