using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentCoroutine;

        private void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }
        public IEnumerator Fade(float targetPoint, float time)
        {
            if(currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(FadeSchedule(targetPoint, time));
            yield return currentCoroutine;

        }
        public IEnumerator FadeSchedule(float targetPoint,float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, targetPoint))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetPoint, Time.unscaledDeltaTime / time);
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            return Fade(0, time);
        }
        public IEnumerator FadeOut(float time)
        {
            return Fade(1, time);
        }
    }
}