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

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutCompete()
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut()
        {
            float timeElapsed = 0.0f;
            while (timeElapsed < fadeOutTime)
            {
                timeElapsed += Time.deltaTime;
                canvasGroup.alpha = timeElapsed / fadeOutTime;
                yield return null;
            }
        }

        public IEnumerator FadeWait()
        {
            yield return new WaitForSeconds(fadeWaitTime);
        }

        public IEnumerator FadeIn()
        {
            float timeElapsed = 0.0f;
            while (timeElapsed < fadeInTime)
            {
                timeElapsed += Time.deltaTime;
                canvasGroup.alpha = 1 - timeElapsed / fadeInTime;
                yield return null;
            }
        }
    }
}