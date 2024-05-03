using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private RectTransform foreground;
        [SerializeField] private Canvas rootCanvas;

        private void Update()
        {
            float healthFraction = health.GetHealth() / health.GetMaxHealth();

            if (Mathf.Approximately(healthFraction, 0.0f) || Mathf.Approximately(healthFraction, 1.0f))
            {
                rootCanvas.enabled = false;
                return;
            }

            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(healthFraction, 1.0f, 1.0f);
        }
    }
}