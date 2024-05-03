using System;
using TMPro;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        public void SetDamage(float damage)
        {
            GetComponentInChildren<TextMeshPro>().text = String.Format("{0:0}", damage);
        }

        public void DestroyText()
        {
            Destroy(gameObject);
        }
    }
}