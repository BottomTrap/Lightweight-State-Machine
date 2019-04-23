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
       

        private void Awake()
        {
            playerstats = (PlayerStats) GetComponent(typeof(PlayerStats));
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            lastPosition = transform.position;
            didHit = false;
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
           //Instantiate bullet in corsair position
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
            if (other.gameObject.tag == "Weapon" && other.gameObject != this.GetComponentInChildren<Transform>().gameObject)
            {
                playerstats.startHealth -= 1 / other.GetComponentInParent<PlayerStats>().Strength.Value; //GET THE UNIT PLAYER STATS NOT THE BULLET , DUH
                Debug.Log(playerstats.startHealth);
            }
        }

    }
}