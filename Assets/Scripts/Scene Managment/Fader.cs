using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagment
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup = null;
        Coroutine currentCoroutine = null;
        void Awake()
        {
            canvasGroup= GetComponent<CanvasGroup>();

        }

       
        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1.0f;
        }


        public Coroutine FadeOut(float time)
        {
            return Fade(1, time);
        }
        public Coroutine FadeIn(float time)
        {
            return Fade(0,time);
        }

        private Coroutine Fade(float target,float time)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(FadeRoutine(target, time));
            return currentCoroutine;
        }

        private IEnumerator FadeRoutine(float target,float time )
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha,target,Time.deltaTime / time);
                yield return null;
            }
        }
    }

}