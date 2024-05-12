using System.Collections;
using RPG.Attributes;
using RPG.Control;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private WeaponConfig weapon;
        [SerializeField] private float healthToRestore = 0.0f;
        [SerializeField] private float respawnTime = 5.0f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            if (weapon != null) other.GetComponent<Fighter>().EquipWeapon(weapon);
            if (healthToRestore > 0) other.GetComponent<Health>().Heal(healthToRestore);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<SphereCollider>().enabled = shouldShow;
            foreach (Transform child in transform) child.gameObject.SetActive(shouldShow);
        }

        public bool HandleRaycast(PlayerController playerController)
        {
            if (!playerController.GetComponent<Mover>().CanMoveTo(transform.position)) return false;

            if (Input.GetMouseButtonDown(0))
            {
                playerController.GetComponent<Mover>().StartMoveAction(transform.position, 1.0f);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}