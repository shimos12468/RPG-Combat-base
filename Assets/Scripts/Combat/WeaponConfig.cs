using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] Weapon equipedPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] float weaponDamage = 20f;
        [SerializeField] float percentageBouns = 10f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded =false;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";
        public Weapon Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator anim)
        {
            Weapon weapon=null;
            DestroyOldWeapon(rightHandTransform, leftHandTransform);

            if (equipedPrefab != null)
            {
                 weapon = Instantiate(equipedPrefab, GetTransform(rightHandTransform, leftHandTransform));
                 weapon.gameObject.name= weaponName;
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
            return weapon;
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
        public void LaunchProjectile(Transform rightHandTransform, Transform leftHandTransform,Health target,GameObject instigator ,float calculatedDamage)
        {
           
            Projectile projectileInstance = Instantiate(projectile,GetTransform(rightHandTransform,leftHandTransform).position,Quaternion.identity);
            projectileInstance.SetTarget(target,instigator , calculatedDamage);
        }

        public float GetDamage() { return weaponDamage; }
        public float GetPercentageBouns() { return percentageBouns; }
        public float GetRange() { return weaponRange; }
    }

}