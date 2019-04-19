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
        public List<Transform> Visibles = new List<Transform>();

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
        public Vector3 offset;

        private float distanceTraveled = 0;
        private Vector3 lastPosition;

        private Vector3 velocity = Vector3.one;

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
            if (hasPlayed)
            {
                distanceTraveled = 0;
            }
        }

        public bool moved = false;
        public IEnumerator Move(Transform target)
        {
            yield return new WaitForSeconds(1);
            if (distanceTraveled < GetComponent<PlayerStats>().AP.Value * 5)
            {
                while (transform.position != target.position)
                {
                    //transform.position = Vector3.MoveTowards(transform.position, target.position + offset, GetComponent<PlayerStats>().AP.Value / Vector3.Distance(transform.position, target.position));
                    //transform.Translate(target.position , Space.World);
                    transform.position = Vector3.MoveTowards(transform.position, target.position-offset ,1/GetComponent<PlayerStats>().Speed.Value*0.1f);
                    yield return null;
                }
            }
            
            
            
        }
        public IEnumerator HasPlayed()
        {
            while (!hasPlayed)
            {
                yield return new WaitForSeconds(2.0f);
                hasPlayed = true;
            }
            
        }
        public void Action()
        {
            switch (aiModes)
            {
                case AiModes.Attack:
                    StartCoroutine(Move(target));
                    StartCoroutine(skills.Attack());
                    StartCoroutine(HasPlayed());
                    //hasPlayed = true;
                    score = 0;
                    break;
                case AiModes.Cure:
                    StartCoroutine(skills.Cure());
                    StartCoroutine(HasPlayed());

                    //hasPlayed = true;
                    score = 0;
                    break;
                case AiModes.RangedAttack:
                    StartCoroutine(Move(target));
                    StartCoroutine(skills.RangedAttack());
                    StartCoroutine(HasPlayed());
                    //hasPlayed = true;
                    score = 0;
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
            Gizmos.DrawSphere(transform.position, AP + range);
        }


    }
}