using UnityEngine;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        public void OnHit()
        {
            print(gameObject.name);
        }
    }
}