using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movment;
using System;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{

    public class PlayerController : MonoBehaviour
    {

        void Update()
        {

            if (GetComponent<Health>().IsDead()) return;

            if (InteractWithComabt()) return;
            if(InteractWithMovment()) return;
            

        }

        private bool InteractWithComabt()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (var hit in hits)
            {

                hit.transform.TryGetComponent(out CombatTarget target);
                if(target==null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue;
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
            }
            
            return false;
        }

        public bool InteractWithMovment()
        {
            bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {

                    GetComponent<Mover>().StartMoveAction(hit.point ,1f);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

}