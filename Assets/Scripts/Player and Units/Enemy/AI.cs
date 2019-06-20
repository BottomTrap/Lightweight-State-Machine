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

            //target.position = Vector3.zero;
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
        }
    
        public IEnumerator Move(Transform target, IEnumerator nextMove) //Using a coroutine to wait the enemy to finish moving then attack
        {

            yield return new WaitForSeconds(1);

            
                
                
                finalTarget = target.position - offset;
                navAgent.speed = GetComponent<PlayerStats>().Speed.Value;
                //navAgent.destination = finalTarget;
                //navAgent.isStopped = false;
                //transform.position = Vector3.MoveTowards(transform.position, finalTarget, 0.02f);
                navAgent.SetDestination(finalTarget);
            navAgent.stoppingDistance = 3.0f;
                navAgent.isStopped = false;
                GetComponent<Animator>().SetBool("Running", true);
                Debug.Log("Couroutine?");
                //Quaternion rotation = Quaternion.LookRotation(direction);
                //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1 / GetComponent<PlayerStats>().Speed.Value * 0.02f);
                transform.LookAt(target);
            
            
            if (!navAgent.pathPending)
            {
                Debug.Log("is this" + navAgent.pathPending);
                if (navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    Debug.Log("and is this? " + (navAgent.remainingDistance <= navAgent.stoppingDistance));
                    if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
                    {
                        Debug.Log("path?" + navAgent.hasPath);
                        Debug.Log("vel? " + (navAgent.velocity.sqrMagnitude == 0f));
                        Debug.Log("wseltchi");
                        GetComponent<Animator>().SetBool("Running", false);
                        navAgent.isStopped = true;
                        //yield return new WaitForSeconds(0.2f);
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
            
        }

        public void Action()
        {
            switch (aiModes)
            {
                case AiModes.Attack:
                    StartCoroutine(Move(target,skills.Attack()));
                    //StartCoroutine(skills.Attack());
                    if (hasPlayed){
                        StopAllCoroutines();
                    }
                    //StartCoroutine(HasPlayed());
                    //hasPlayed = true;
                    score = 0;
                    break;
                case AiModes.Cure:
                    StartCoroutine(skills.Cure());
                    //StartCoroutine(HasPlayed());
                    if (hasPlayed){
                        StopAllCoroutines();
                    }
                    //hasPlayed = true;
                    score = 0;
                    break;
                case AiModes.RangedAttack:
                    StartCoroutine(Move(target,skills.RangedAttack()));
                    //StartCoroutine(skills.RangedAttack());
                    if (hasPlayed){
                        StopAllCoroutines();
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
            isAlive = false;
            GetComponent<Animator>().SetBool("Death", true);
            this.GetComponent<CapsuleCollider>().enabled = false;

        }

      //public void PassiveActions(Transform enemy)
      //{
      //    if( playersInView.Contains(enemy)){
      //            target = enemy;
      //            StartCoroutine(skills.PassiveRangedAttack());
      //        }
      //    
      //}


    
      // public void PassiveActions()
      // {
      //     var cam = GetComponentInChildren<Camera>();
      //     List<Vector3> pointsPlayerInView= new List<Vector3>();
      //     foreach (Transform player in playersInView)
      //     {
      //         pointsPlayerInView.Add(cam.WorldToScreenPoint(player.GetComponentInChildren<Renderer>().bounds.center));
      //
      //     }
      //
      //     foreach (Vector3 pointOnScreen in pointsPlayerInView)
      //     {
      //         if (pointOnScreen.z < 0)
      //         {
      //             //Debug.Log("Behind: " + toCheck.name);
      //             continue;
      //         }
      //
      //         //Is in FOV
      //         if ((pointOnScreen.x < 0) || (pointOnScreen.x > (float)cam.pixelWidth) ||
      //                 (pointOnScreen.y < 0) || (pointOnScreen.y > (float)cam.pixelHeight))
      //         {
      //            // Debug.Log("OutOfBounds: " + toCheck.name);
      //             continue;
      //         }
      //
      //         RaycastHit hit;
      //         Vector3 heading = pointOnScreen - transform.position;
      //         Vector3 direction = heading.normalized;// / heading.magnitude;
      //
      //         if (Physics.Linecast(cam.transform.position,pointOnScreen, out hit))
      //         {
      //             if (hit.transform.tag == "EnemyUnit" || hit.transform.tag == "PlayerUnit" || hit.transform.tag == "Weapon" || hit.transform.tag == "PlayerWeapon")
      //             {
      //                 target = hit.transform;
      //                 StartCoroutine(skills.PassiveRangedAttack());
      //             }
      //             else
      //                 continue;
      //         }
      //     }
      //
      // }
      //
    }
}