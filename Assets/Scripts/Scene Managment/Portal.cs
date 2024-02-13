using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagment{

    public class Portal : MonoBehaviour
    {
        [SerializeField]int sceneToLoad =-1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] float fadeOutDuration =3f, fadeInDuration=1f, fadeWaitDuration =2f;

        enum Destinations
        {
            A,B,C,D,E,F
        }
        [SerializeField] Destinations destination;       
        
        private void OnTriggerEnter(Collider other) {

            if(other.gameObject.tag=="Player"){

                StartCoroutine(Transition());

            }

        }

        public IEnumerator Transition()
        {

            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load is below 0");
                yield break;
            }
            DontDestroyOnLoad(gameObject);
            Fader fader = GameObject.FindObjectOfType<Fader>();

            yield return fader.FadeOut(fadeOutDuration);
            SavingWrapper warpper = FindObjectOfType<SavingWrapper>();

            warpper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            
            warpper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            warpper.Save();

            yield return new WaitForSeconds(fadeWaitDuration);
            yield return fader.FadeIn(fadeInDuration);
           
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
          foreach(Portal obj in FindObjectsOfType<Portal>())
            {
                if (obj == this) continue;
                if(obj.destination==destination)
                    return obj;
               
            }
          return null;
        }
    }

}