using UnityEngine;

namespace RPG.Core
{
    [RequireComponent(typeof(ParticleSystem))]
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] private GameObject objectToDestroy = null;
        private void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                if (objectToDestroy != null)
                {
                    Destroy(objectToDestroy);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}