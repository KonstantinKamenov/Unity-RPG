using UnityEngine;

namespace RPG.Core
{
    public class ForwardFacing : MonoBehaviour
    {
        private void Update()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}