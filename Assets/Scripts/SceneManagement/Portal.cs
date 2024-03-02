using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Rpg.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        private enum PortalIdentifier
        {
            A, B, C
        }

        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private PortalIdentifier portalIdentifier;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayerPostition(otherPortal);
            Destroy(gameObject);
        }

        public Portal GetOtherPortal()
        {
            Portal[] portals = GameObject.FindObjectsOfType<Portal>();
            foreach (Portal p in portals)
            {
                if (p.portalIdentifier == this.portalIdentifier && p != this) return p;
            }
            return null;
        }

        public void UpdatePlayerPostition(Portal portal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            Transform spawnPoint = portal.spawnPoint;
            player.GetComponent<NavMeshAgent>().Warp(spawnPoint.position);
            player.transform.rotation = spawnPoint.rotation;
        }
    }
}