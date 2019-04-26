using System;
using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.EventSystems;
using RG;


//[RequireComponent(typeof(Animator))]

namespace RG
{
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        private CharacterController characterController;
        private Animator animator;
        public GameModeManager statesManager;

        [SerializeField] private float moveSpeed = 30;
        [SerializeField] private float turnSpeed = 3.5f;


        private Vector3 lastPosition;
        public float distanceTraveled;
        public bool didHit=false;
        public bool isAlive;
        //private bool aiming = false;

        private PlayerStats playerstats;
        public bool isClicked=false;

        public Texture crosshair;

        public bool drawcrosshair;
        public GameObject bullet;

        public Transform rotator;

        private void Awake()
        {
            playerstats = (PlayerStats) GetComponent(typeof(PlayerStats));
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            lastPosition = transform.position;
            didHit = false;
            rotator = transform.GetChild(1).transform;
        }

        private void FixedUpdate()
        {
             distanceTraveled += Vector3.Distance(transform.position, lastPosition);
             lastPosition = transform.position;

            //if (didHit  )
            //{
            //    var anim = GetComponentInChildren<Animator>();
            //    if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            //        anim.SetBool("Attack",false);
            //}
            
        }

        public void Attack()
        {
            var anim = GetComponentInChildren<Animator>();
            anim.SetTrigger("Attack");
            Debug.Log("Attack");
            if (!anim.IsInTransition(0))
            didHit = true;
        }

        void OnGUI()
        {
            if (drawcrosshair)
            {
                if (Time.time != 0 && Time.timeScale != 0)
                    GUI.DrawTexture(
                        new Rect(Screen.width / 2 - (crosshair.width * 0.5f),
                            Screen.height / 2 - (crosshair.height * 0.5f), crosshair.width, crosshair.height),
                        crosshair);
            }
        }

        public void RangedAttack()
        {
           
                RaycastHit hit;
            //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height, 0));
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                //int layerMask = ~( (1 << 8) | ( 1 << 9 ) ) ;
            Debug.Log("checking for fire");
                if (Physics.Raycast(ray,out hit))
                {
                Debug.Log("checking for ray");
                    if (hit.transform.tag == "EnemyUnit")
                    {
                        var heading = hit.transform.position - transform.GetChild(2).position;
                        var projectile = Instantiate(bullet, transform.GetChild(2).position, transform.GetChild(2).rotation);
                        projectile.GetComponent<Bullet>().shooter = this.gameObject;
                        projectile.transform.LookAt(hit.transform);
                        projectile.GetComponent<Rigidbody>().AddForce(heading * 5.0f, ForceMode.VelocityChange);
                        Destroy(projectile, 3);
                    Debug.Log("fired");
                    }
                    
                
            }
            else
            {
                var heading = transform.GetChild(2).forward;
                var projectile = Instantiate(bullet, transform.GetChild(2).position, transform.GetChild(2).rotation);
                projectile.GetComponent<Bullet>().shooter = this.gameObject;
                projectile.GetComponent<Rigidbody>().AddForce(heading * 5.0f, ForceMode.Impulse);
                Destroy(projectile, 3);
            }
            //Get the raycast hit point from the crosshair in the cameraScript
            //Instantiate bullet from the weapon point towards the raycast hit
            //if there is no raycast hit, what do? DO NOTHING , that'd be too dumb, let the enemies be hit or hitable targets only
        }

        public void Movement()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var movement = new Vector3(horizontal, 0, vertical);
            movement = transform.TransformDirection(movement);

            characterController.SimpleMove(movement * Time.deltaTime * moveSpeed);
        }

        public void Rotate()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var movement = new Vector3(horizontal, 0, vertical);
            movement = transform.TransformDirection(movement);
            if (movement.magnitude > 0)
            {
                Quaternion newDirection = Quaternion.LookRotation(movement);

                transform.rotation = Quaternion.Slerp(transform.rotation, newDirection, Time.deltaTime * turnSpeed);
            }
        }



        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "WeaponBullet" && other.gameObject != this.GetComponentInChildren<Transform>().gameObject && other.GetComponent<Bullet>().shooter.tag != "PlayerUnit")
            {
                var successRate = other.GetComponent<Bullet>().shooter.GetComponent<PlayerStats>().HitRate.Value / 10.0f;
                var result = UnityEngine.Random.Range(0.0f, 1.0f) < successRate;
                Debug.Log(result);
                if (result)
                {
                    playerstats.startHealth -= 1 / other.GetComponent<Bullet>().shooter.GetComponent<PlayerStats>().GunStrength.Value; //GET THE UNIT PLAYER STATS NOT THE BULLET , DUH
                    Debug.Log(playerstats.startHealth);
                }
            }
            if (other.gameObject.tag == "Weapon" && other.gameObject != this.GetComponentInChildren<Transform>().gameObject)
            {
                playerstats.startHealth -= 1 / other.GetComponent<Bullet>().shooter.GetComponent<PlayerStats>().Strength.Value; //GET THE UNIT PLAYER STATS NOT THE BULLET , DUH
                Debug.Log(playerstats.startHealth);
            }
        }

    }
}