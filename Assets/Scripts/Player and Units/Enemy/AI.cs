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

        public Transform damagePopUpPrefab;

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
        public List<Transform> playersInView = new List<Transform>();

        public GameModeManager gameModeManager;

        private Vector3 velocity = Vector3.one;



        public Vector3 RandomPointOnCircleEdge(float radius)
        {
            var vector2 = Random.insideUnitCircle.normalized * radius;
            return new Vector3(vector2.x, 0, vector2.y);
        }


        void Awake()
        {
            aiModes = AiModes.Attack; //Default AI Mode;
            skills = GetComponent<Skills>();
            stats = GetComponent<PlayerStats>();
            gameModeManager = FindObjectOfType<GameModeManager>();

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

            if (stats.startHealth <= 0.0f)
            {
                Death();
            }

            
        }

        public bool PathComplete(Vector3 target)
        {
            if (target == null)
            {
                return false;
            }
            NavMeshPath path = new NavMeshPath();
            navAgent.CalculatePath(target, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                return true;
            }
            else
                return false;
        } //Checks if the path chosen is a valid one
    
        public IEnumerator Move(Transform target, IEnumerator nextMove) //Using a coroutine to wait the enemy to finish moving then attack
        {
            yield return new WaitForSeconds(1);
                finalTarget = target.position - offset;
                navAgent.speed = GetComponent<PlayerStats>().Speed.Value;
                navAgent.SetDestination(finalTarget);
            navAgent.stoppingDistance = 3.0f;
                navAgent.isStopped = false;
                GetComponent<Animator>().SetBool("Running", true);
                transform.LookAt(target);
            if (!navAgent.pathPending)
            {
                if (navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
                    {
                        GetComponent<Animator>().SetBool("Running", false);
                        navAgent.isStopped = true;
                        yield return StartCoroutine(nextMove);
                    }
                }
            }
            yield return null;
        }

        public IEnumerator HasPlayed(IEnumerator cor)
        {
            while (!hasPlayed)
            {
                StopCoroutine(cor);
                yield return new WaitForSeconds(1f);
                hasPlayed = true;
                yield return null;
            }
        }  // ** Checks if the unit has finished playing to set the hasPlayed bool 
        // ** , when all has played, it goes to the next state 

        public void Action()
        {
            switch (aiModes)
            {
                case AiModes.Attack:
                    StartCoroutine(Move(target,skills.Attack()));
                    if (hasPlayed){
                        StopAllCoroutines();
                    }
                    score = 0;
                    break;
                case AiModes.Cure:
                    StartCoroutine(skills.Cure());
                    if (hasPlayed){
                        StopAllCoroutines();
                    }
                    score = 0;
                    break;
                case AiModes.RangedAttack:
                    StartCoroutine(Move(target,skills.RangedAttack()));
                    if (hasPlayed){
                        StopAllCoroutines();
                    }
                    score = 0;
                    break;
            }

        } //switch method to choose between different actions (attack, cure, ranged attack etc)

        void OnDrawGizmosSelected() //This will help show me the range of the enemy, in editor
        {
            var AP = GetComponent<PlayerStats>().AP.Value;
            var range = GetComponent<PlayerStats>().Range.Value;
            // Draw a yellow sphere at the transform's position
            Gizmos.color = new Color(255, 204, 102, 0.3f);
            Gizmos.DrawSphere(transform.position, AP + range);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "PlayerWeapon")
            {
                if (other.GetComponentInParent<PlayerMovement>().didHit == true && other.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {

                    stats.startHealth -= 1 / other.GetComponentInParent<PlayerStats>().GunStrength.Value;
                    Debug.Log(stats.startHealth);
                    Transform damagePopUpTransform = Instantiate(damagePopUpPrefab, transform.position, Quaternion.identity);
                    DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
                    damagePopUp.Setup(1 / other.GetComponentInParent<PlayerStats>().GunStrength.Value);

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
                        Transform damagePopUpTransform = Instantiate(damagePopUpPrefab, transform.position, Quaternion.identity);
                        DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
                        damagePopUp.Setup(1 / other.GetComponent<Bullet>().shooter.GetComponent<PlayerStats>().Strength.Value);
                }
            }
            
        } //Collision detection for damage dealt to the unit

        void Death()
        {
            isAlive = false;
            GetComponent<Animator>().SetBool("Death", true);
            this.GetComponent<CapsuleCollider>().enabled = false;

        } //Self Explanatory

    }
}