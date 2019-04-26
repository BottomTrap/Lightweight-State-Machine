using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SA;

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
        private PlayerStats stats;
        public Transform target;
        public bool hasPlayed = false; //For enemy units AI
        public bool isAlive = true;
        public int score = 0;
        public float fov = 90.0f;
        public AiModes aiModes;
        public Vector3 offset;
        public Vector3 finalTarget;
        private float distanceTraveled = 0;
        private Vector3 lastPosition;

        public GameModeManager gameModeManager;

        private Vector3 velocity = Vector3.one;

        void Awake()
        {
            aiModes = AiModes.Attack; //Default AI Mode;
            skills = GetComponent<Skills>();
            stats = GetComponent<PlayerStats>();
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
            //offset = Vector3.zero;


            if (stats.startHealth <= 0.0f)
            {
                Death();
            }
        }

        public bool moved = false;
        public IEnumerator Move(Transform target, IEnumerator nextMove)
        {

            yield return new WaitForSeconds(1);
            if (distanceTraveled < GetComponent<PlayerStats>().AP.Value * 5 && gameModeManager.currentState == gameModeManager.GetState("actionAiState"))
            {
                
                while (transform.position != target.position-offset)
                {
                    finalTarget = target.position - offset;
                    //transform.position = Vector3.MoveTowards(transform.position, target.position + offset, GetComponent<PlayerStats>().AP.Value / Vector3.Distance(transform.position, target.position));
                    //transform.Translate(target.position , Space.World);
                    transform.position = Vector3.MoveTowards(transform.position, finalTarget ,1/GetComponent<PlayerStats>().Speed.Value*0.02f);
                    Vector3 direction = target.position - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1 / GetComponent<PlayerStats>().Speed.Value * 0.02f);
                    yield return null;
                }
            }else 
            yield return StartCoroutine(nextMove);

            yield return StartCoroutine(nextMove);

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
                    StartCoroutine(Move(target,skills.Attack()));
                    //StartCoroutine(skills.Attack());
                    if (hasPlayed){
                        StopCoroutine (skills.Attack());
                    }
                    //StartCoroutine(HasPlayed());
                    //hasPlayed = true;
                    score = 0;
                    break;
                case AiModes.Cure:
                    StartCoroutine(skills.Cure());
                    //StartCoroutine(HasPlayed());
                    if (hasPlayed){
                        StopCoroutine (skills.Cure());
                    }
                    //hasPlayed = true;
                    score = 0;
                    break;
                case AiModes.RangedAttack:
                    StartCoroutine(Move(target,skills.RangedAttack()));
                    //StartCoroutine(skills.RangedAttack());
                    if (hasPlayed){
                        StopCoroutine (skills.RangedAttack());
                    }
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "PlayerWeapon")
            {
                if (other.GetComponentInParent<PlayerMovement>().didHit == true && other.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {

                    stats.startHealth -= 1 / other.GetComponentInParent<PlayerStats>().GunStrength.Value;
                    Debug.Log(stats.startHealth);

                }
            }
            if (other.gameObject.tag == "WeaponBullet" && other.gameObject.GetComponent<Bullet>().shooter.tag != "EnemyUnit")
            {
                    var successRate = other.GetComponent<Bullet>().shooter.GetComponent<PlayerStats>().HitRate.Value / 10.0f;
                    var result = UnityEngine.Random.Range(0.0f, 1.0f) < successRate;
                    if (result)
                    {
                        stats.startHealth -= 1 / other.GetComponent<Bullet>().shooter.GetComponent<PlayerStats>().Strength.Value;
                        Debug.Log(stats.startHealth);
                    }
            }
            
        }
        bool isPlaying(Animator anim, string stateName)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) ||
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                return true;
            else
                return false;
        }

        void Death()
        {

            Destroy(this.gameObject);

        }

    }
}