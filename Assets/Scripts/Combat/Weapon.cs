using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject equipedPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] float weaponDamage = 20f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded =false;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";
        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator anim)
        {

            DestroyOldWeapon(rightHandTransform, leftHandTransform);

            if (equipedPrefab != null)
            {
                 GameObject weapon= Instantiate(equipedPrefab, GetTransform(rightHandTransform, leftHandTransform));
                 weapon.name= weaponName;
            }
            var OC = anim.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                anim.runtimeAnimatorController= animatorOverride;
            }
            else if (OC != null)
            {
                 anim.runtimeAnimatorController= OC.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform oldWeapon = rightHandTransform.Find(weaponName);
            if (oldWeapon == null)
            {
               oldWeapon = leftHandTransform.Find(weaponName);
            }

            if (oldWeapon == null) { return; }
            oldWeapon.name = "Destroyed Weapon";
            Destroy(oldWeapon.gameObject);

        }

        private Transform GetTransform(Transform rightHandTransform, Transform leftHandTransform)
        {
            return isRightHanded == true ? rightHandTransform : leftHandTransform;
        }

        public bool HasProjectile() { return projectile != null; }

        public void LaunchProjectile(Transform rightHandTransform, Transform leftHandTransform,Health target)
        {
            Projectile projectileInstance = Instantiate(projectile,GetTransform(rightHandTransform,leftHandTransform).position,Quaternion.identity);
            projectileInstance.SetTarget(target ,weaponDamage);
        }

        public float GetDamage() { return weaponDamage; }
        public float GetRange() { return weaponRange; }
    }

}