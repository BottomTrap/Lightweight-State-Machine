using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RG
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AI : MonoBehaviour
    {
        public List<Collider> colliders = new List<Collider>();
        public List<Transform> Visibles= new List<Transform>();

        private NavMeshAgent navAgent;
        public enum AiModes
        {
            Attack = 100,
            RangedAttack = 200,
            Cure = 300
        }

        private Skills skills;
        public Transform target;
        public bool hasPlayed = false; //For enemy units AI
        public bool isAlive = true;
        public int score = 0;
        public float fov = 90.0f;
        public AiModes aiModes;

        private float distanceTraveled;
        private Vector3 lastPosition;

        void Awake()
        {
            aiModes = AiModes.Attack; //Default AI Mode;
            skills = GetComponent<Skills>();
        }

        private void Start()
        {
            navAgent = GetComponent<NavMeshAgent>();
            navAgent.speed = GetComponent<PlayerStats>().Speed.Value; //Setting the Movement speed of each UNIT
        }
        private void FixedUpdate()
        {
            distanceTraveled += Vector3.Distance(transform.position, lastPosition);
            lastPosition = transform.position;
        }
        public IEnumerator Move(Transform target)
        {
            yield return null;
            if (distanceTraveled < GetComponent<PlayerStats>().AP.Value)
            {
                transform.position = Vector3.Lerp(transform.position, target.position, GetComponent<PlayerStats>().AP.Value/Vector3.Distance(transform.position, target.position)  );
            }
            hasPlayed = true;
        }

        public void Action()
        {
            switch (aiModes)
            {
                case AiModes.Attack:
                    StartCoroutine(Move(target));
                    skills.Attack();
                   
                    break;
                case AiModes.Cure:
                    skills.Cure();
                    hasPlayed = true;
                    break;
                case AiModes.RangedAttack:
                    StartCoroutine(Move(target));
                    skills.RangedAttack();
                    hasPlayed = true;
                    break;
            }
            
        }
     
        void OnDrawGizmosSelected()
        {
            var AP = GetComponent<PlayerStats>().AP.Value;
            var range = GetComponent<PlayerStats>().Range.Value;
            //Debug.Log(AP);
            // Draw a yellow sphere at the transform's position
            Gizmos.color = new Color(255, 204, 102, 0.3f);
            Gizmos.DrawSphere(transform.position, AP+range);
        }
    }
}