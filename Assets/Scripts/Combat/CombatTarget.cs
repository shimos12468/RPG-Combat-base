
using RPG.Attributes;
using RPG.Control;
using UnityEngine;
using static RPG.Control.PlayerController;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
       

        public bool HandleRaycast(PlayerController controller)
        {
            if (!controller.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return false;
            }
            if (Input.GetMouseButton(0))
            {
                controller.GetComponent<Fighter>().Attack(gameObject);
            }
            return true;
        }
        public CursorType GetCursorType()
        {
            return CursorType.Comabt;
        }
    }
}