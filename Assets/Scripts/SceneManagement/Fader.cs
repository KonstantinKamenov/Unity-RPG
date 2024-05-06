using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private float fadeOutTime = 1.5f;
        [SerializeField] private float fadeWaitTime = 0.5f;
        [SerializeField] private float fadeInTime = 1.5f;


        private CanvasGroup canvasGroup;
        private Coroutine currentlyActiveFade = null;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutCompete()
        {
            canvasGroup.alpha = 1;
        }

        public Coroutine FadeOut()
        {
            return Fade(1.0f, fadeOutTime);
        }

        public IEnumerator FadeWait()
        {
            yield return new WaitForSeconds(fadeWaitTime);
        }

        public Coroutine FadeIn()
        {
            return Fade(0.0f, fadeInTime);
        }

        private Coroutine Fade(float target, float time)
        {
            if (currentlyActiveFade != null) StopCoroutine(currentlyActiveFade);
            currentlyActiveFade = StartCoroutine(FadeRoutine(target, time));
            return currentlyActiveFade;
        }

        public IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}