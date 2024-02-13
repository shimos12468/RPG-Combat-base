using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {

        [SerializeField] Weapon weapon;
        [SerializeField] float hideDuration = 15f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipeWeapon(weapon);
                StartCoroutine(HideForSeconds());
            }
        }

        private IEnumerator HideForSeconds()
        {
            foreach(Transform obj in transform)
            {
                obj.gameObject.SetActive(false);
            }
            GetComponent<SphereCollider>().enabled =false;
            yield return new WaitForSeconds(hideDuration);
            foreach (Transform obj in transform)
            {
                obj.gameObject.SetActive(true);
            }
            GetComponent<SphereCollider>().enabled = true;
        }
    }
}
