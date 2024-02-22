using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {

        [SerializeField] Weapon weapon;
        [SerializeField] float respawnTime = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipeWeapon(weapon);
                StartCoroutine(HideForSeconds(respawnTime));
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shoulShow)
        {
            foreach (Transform obj in transform)
            {
                obj.gameObject.SetActive(shoulShow);
            }
            GetComponent<Collider>().enabled = shoulShow;
        }
    }
}


