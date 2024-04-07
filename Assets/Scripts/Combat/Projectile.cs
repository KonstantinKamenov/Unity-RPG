namespace RPG.Combat
{
    using RPG.Core;
    using UnityEngine;

    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Health target = null;
        [SerializeField] private float speed = 5.0f;

        private float damage = 0.0f;

        private void Update()
        {
            if (target == null) return;

            transform.LookAt(GetAimLocation(target.transform));
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
        }

        private Vector3 GetAimLocation(Transform target)
        {
            CapsuleCollider collider = target.GetComponent<CapsuleCollider>();
            if (collider == null)
            {
                return target.position;
            }
            return target.position + Vector3.up * collider.height * 0.5f;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("aaa");
            Health hitHealth = other.GetComponent<Health>();
            if (hitHealth == null || hitHealth != target) return;

            hitHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}