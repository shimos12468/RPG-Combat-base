using RPG.Combat;
using RPG.Attributes;
using RPG.Movment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using GameDevTV.Utils;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float aggroCooldownTime = 3f;
        [SerializeField] float dwellingTime = 10f;
        [SerializeField] float patrolSpeedFraction =0.2f;
        [SerializeField] float shoutDistance = 5f;
        GameObject player;
        Fighter fighter;
        Mover mover;
        Health health;

        LazyValue<Vector3> guardPosition;
        float timeSinceLastSawPlayer =Mathf.Infinity;
        float timeSinceAggrivated = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex =0;


        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            guardPosition = new LazyValue<Vector3>(GetInitialPosition);
        }

        private Vector3 GetInitialPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }

        void Update()
        {
            if (health.IsDead()) return;
            if (player == null) return;

            if (IsAggrevated() && fighter.CanAttack(player))
            {

                AttackBehaviour();
            }

            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                if(patrolPath!=null)
                PatrolBehaviour();
            }
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceAggrivated += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }


       

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

            if (AtWaypoint())
            {
                timeSinceArrivedAtWaypoint = 0;
                CycleWaypoint();
            }
            nextPosition = GetCurrentWaypoint();


            if(timeSinceArrivedAtWaypoint>=dwellingTime)
            mover.StartMoveAction(nextPosition ,patrolSpeedFraction);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetwayPoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextPoint(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) <= chaseDistance;
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
            AggrivateNearByEnemies();
        }

        private void AggrivateNearByEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);

            foreach (RaycastHit hit in hits) {

                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai != null)
                {
                    hit.collider.GetComponent<AIController>().Aggrevate();
                }
            }
        }

        public void Aggrevate()
        {
            timeSinceAggrivated = 0;
        }

        private bool IsAggrevated()
        {
            if (timeSinceAggrivated < aggroCooldownTime)
            {
                return true;
            }
            return Vector3.Distance(player.transform.position, transform.position)<=chaseDistance;
        }

        // called by unity 
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }

}