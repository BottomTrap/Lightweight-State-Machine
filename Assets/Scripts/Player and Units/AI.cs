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

        void Awake()
        {
            aiModes = AiModes.Attack; //Default AI Mode;
            skills = GetComponent<Skills>();
        }

        public void Move(Transform target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 5.0f * Time.deltaTime);
        }

        public void Action()
        {
            switch (aiModes)
            {
                case AiModes.Attack:
                    Move(target);
                    skills.Attack();
                    break;
                case AiModes.Cure:
                    skills.Cure();
                    break;
                case AiModes.RangedAttack:
                    Move(target);
                    skills.RangedAttack();
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