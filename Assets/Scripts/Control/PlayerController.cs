using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movment;
using System;
using RPG.Combat;
using RPG.Attributes;
using System.Data;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        public enum CursorType
        {
            None,
            Comabt,
            Movment,
            UI
        }
        [System.Serializable]
        public struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[]cursorMappings =null;

        private void Awake()
        {
            health = GetComponent<Health>();
        }


        void Update()
        {

            if (InteractWithUI()) return;
            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithComabt()) return;
            if(InteractWithMovment()) return;
            

        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
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
                SetCursor(CursorType.Comabt);
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
                SetCursor(CursorType.Movment);
                return true;
            }
            return false;
        }

        private void SetCursor(CursorType type)
        {

            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }
        private CursorMapping GetCursorMapping(CursorType type)
        {
            CursorMapping mappin = cursorMappings[0];
            foreach (var mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    mappin=mapping;
                    break;
                }
            }
            return mappin;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

}