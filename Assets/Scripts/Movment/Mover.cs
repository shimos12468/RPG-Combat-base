
using RPG.Attributes;
using RPG.Core;
using RPG.Saving;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movment
{
    public class Mover : MonoBehaviour,IAction ,ISaveable
    {
        private Animator _anim;
        private NavMeshAgent agent;
        private Health health;
        [SerializeField]float maxSpeed =6f;
        [SerializeField]float maxPathLength = 40f;
        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            _anim = GetComponentInChildren<Animator>();
            health = GetComponent<Health>();
            
        }


        void LateUpdate()
        {
            GetComponent<NavMeshAgent>().enabled=!health.IsDead();
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            _anim.SetFloat("forwardSpeed", localVelocity.z);
        }

        public void StartMoveAction(Vector3 destination ,float speedFraction){

            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination ,speedFraction);
        }

        public void MoveTo(Vector3 destination ,float speedFraction)
        {   
            agent.SetDestination(destination);
            agent.speed =maxSpeed*Mathf.Clamp01(speedFraction);
            agent.isStopped =false;
        }


        [System.Serializable]
        struct MoverSavedData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }
        public void Cancel(){
            agent.isStopped =true;
        }

        public object CaptureState()
        {

            MoverSavedData data = new MoverSavedData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            MoverSavedData data = (MoverSavedData)state;
            agent.Warp(data.position.ToVector());
            transform.eulerAngles = data.rotation.ToVector();
        }

        internal bool CanMoveTo(Vector3 distination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, distination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxPathLength) return false;
            return true;
        }
        private float GetPathLength(NavMeshPath path)
        {
            float totalDistance = 0;
            if (path.corners.Length < 2) return totalDistance;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                totalDistance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return totalDistance;
        }
    }
}
