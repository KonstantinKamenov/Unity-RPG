using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText damageTextPrefab = null;

        public void SpawnText(float damage)
        {
            DamageText damageText = Instantiate(damageTextPrefab, transform);
            damageText.SetDamage(damage);
        }
    }
}