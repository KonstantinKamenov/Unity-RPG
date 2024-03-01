using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rpg.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private int sceneToLoad = -1;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}