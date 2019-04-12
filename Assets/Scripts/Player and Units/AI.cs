using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RG
{
    public class AI : MonoBehaviour
    {
        public List<Collider> colliders = new List<Collider>();
        public List<Transform> Visibles= new List<Transform>();

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
        private void FixedUpdate()
        {
            distanceTraveled += Vector3.Distance(transform.position, lastPosition);
            lastPosition = transform.position;
        }
        public IEnumerator Move(Transform target)
        {
            if (distanceTraveled <
            GetComponent<PlayerStats>().AP.Value )
            {
                float currentTime = 0f;
                float totalTime = 9f;
                while (currentTime < totalTime)
                {
                    transform.position = Vector3.Lerp(transform.position, target.position, GetComponent<PlayerStats>().AP.Value/Vector3.Distance(transform.position, target.position)  );
                    currentTime += Time.deltaTime;
                    yield return null;
                }

            }
            
            
        }

        public void Action()
        {
            switch (aiModes)
            {
                case AiModes.Attack:
                    StartCoroutine(Move(target));
                    skills.Attack();
                    hasPlayed = true;
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