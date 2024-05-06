using UnityEngine;

namespace RPG.Core
{
    public class ForwardFacing : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}