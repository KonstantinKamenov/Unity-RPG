using System;
using RPG.Attributes;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            Health health = fighter.GetTarget();
            string text = "N/A";
            if (health != null) text = String.Format("{0:0}%", health.GetPercentage());
            GetComponent<TextMeshProUGUI>().text = text;
        }
    }
}