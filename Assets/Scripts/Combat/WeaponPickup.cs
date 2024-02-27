using RPG.Attributes;
using RPG.Control;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {

        [SerializeField] WeaponConfig weaponConfig;
        [SerializeField] float healthToRestore = 0;
        [SerializeField] float respawnTime = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {

            if (weaponConfig != null)
            {
                subject.GetComponent<Fighter>().EquipeWeapon(weaponConfig);
            }
            if (healthToRestore != 0)
            {
                subject.GetComponent<Health>().Heal(healthToRestore);
            }
            StartCoroutine(HideForSeconds(respawnTime));
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

        public bool HandleRaycast(PlayerController controller)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(controller.gameObject);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}


