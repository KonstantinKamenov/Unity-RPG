namespace RPG.Combat
{
    using RPG.Attributes;
    using UnityEngine;

    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Health target = null;
        [SerializeField] private float speed = 5.0f;
        [SerializeField] private bool isHoming = false;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float maxLifetime = 10.0f;

        private float damage = 0.0f;

        private void Update()
        {
            if (target == null) return;

            if (isHoming && !target.IsDead()) transform.LookAt(GetAimLocation(target.transform));
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            transform.LookAt(GetAimLocation(target.transform));
            this.damage = damage;

            Destroy(gameObject, maxLifetime);
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
            Health hitHealth = other.GetComponent<Health>();
            if (hitHealth == null || hitHealth != target || hitHealth.IsDead()) return;

            if (hitEffect != null) Instantiate(hitEffect, transform.position, transform.rotation);
            hitHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}