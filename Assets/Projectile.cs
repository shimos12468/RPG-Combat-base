using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {

        Health target;
        [SerializeField] GameObject hitEffect;
        [SerializeField] float speed = 1.0f;
        [SerializeField] bool isHooming = false;
        float damage = 0;
        [SerializeField] float durationBeforeDestroy = 10f;
        [SerializeField] GameObject[] projectileParts = null;
        [SerializeField] float lifeAfterImpact = 2f;
        // Update is called once per frame

        private void Start()
        {
            Destroy(gameObject, durationBeforeDestroy);
        }

        void Update()
        {
            if (target == null) return;

            if (isHooming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation(target));
                ThrowProjectile();
            }
            else
            {
                ThrowProjectile();
            }

        }

        private void ThrowProjectile()
        {

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;

            transform.LookAt(GetAimLocation(target));
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() == null || other.GetComponent<Health>() != target) return;

            if (target.IsDead()) return;

            target.TakeDamage(damage);
            if (hitEffect != null) Instantiate(hitEffect, GetAimLocation(target), transform.rotation);
            speed = 0;
            foreach (var obj in projectileParts)
            {
                Destroy(obj);
            }

            Destroy(gameObject, lifeAfterImpact);

        }
        private Vector3 GetAimLocation(Health target)
        {
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
            if (targetCollider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCollider.height / 2;
        }
    }

}