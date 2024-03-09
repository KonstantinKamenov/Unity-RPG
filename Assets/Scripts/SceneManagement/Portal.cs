using System.Collections;
using RPG.Saving;
using RPG.SceneManagement;
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

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

            yield return fader.FadeOut();
            savingWrapper.Save();
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            savingWrapper.Load();
            Portal otherPortal = GetOtherPortal();
            UpdatePlayerPostition(otherPortal);
            savingWrapper.Save();

            yield return fader.FadeWait();
            yield return fader.FadeIn();

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