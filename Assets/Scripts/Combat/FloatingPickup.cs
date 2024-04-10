namespace RPG.Combat
{
    using System.Collections;
    using UnityEngine;

    public class FloatingPickup : MonoBehaviour
    {
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private float duration = 1.0f;

        private float directionMultiplier = 1.0f;

        private void Start()
        {
            StartCoroutine(ChangeDirection());
        }

        private void Update()
        {
            transform.Translate(Vector3.up * speed * directionMultiplier * Time.deltaTime);
        }

        private IEnumerator ChangeDirection()
        {
            while (true)
            {
                yield return new WaitForSeconds(duration);
                directionMultiplier *= -1.0f;
            }
        }
    }
}