namespace RPG.Core
{
    using UnityEngine;

    [RequireComponent(typeof(ParticleSystem))]
    public class DestroyAfterEffect : MonoBehaviour
    {
        private void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive()) Destroy(gameObject);
        }
    }
}