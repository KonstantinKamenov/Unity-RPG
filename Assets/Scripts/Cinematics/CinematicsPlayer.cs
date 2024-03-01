using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class NewBehaviourScript : MonoBehaviour
    {
        private bool hasTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (hasTriggered) return;
            if (other.tag != "Player") return;

            hasTriggered = true;
            GetComponent<PlayableDirector>().Play();
        }
    }

}