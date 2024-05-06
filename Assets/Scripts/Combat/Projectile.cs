namespace RPG.Combat
{
    using RPG.Attributes;
    using UnityEngine;
    using UnityEngine.Events;

    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Health target = null;
        [SerializeField] private float speed = 5.0f;
        [SerializeField] private bool isHoming = false;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float maxLifetime = 10.0f;
        [SerializeField] private GameObject[] intantDestroy;
        [SerializeField] private float destroyAfter = 5.0f;
        [SerializeField] private UnityEvent onHit;

        private float damage = 0.0f;
        private GameObject attacker;

        private void Update()
        {
            if (target == null) return;

            if (isHoming && !target.IsDead()) transform.LookAt(GetAimLocation(target.transform));
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(GameObject attacker, Health target, float damage)
        {
            this.attacker = attacker;
            this.target = target;
            this.damage = damage;
            transform.LookAt(GetAimLocation(target.transform));

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

            onHit.Invoke();
            if (hitEffect != null) Instantiate(hitEffect, transform.position, transform.rotation);
            hitHealth.TakeDamage(attacker, damage);
            foreach (GameObject go in intantDestroy)
            {
                Destroy(go);
            }
            Destroy(gameObject, destroyAfter);
        }
    }
}