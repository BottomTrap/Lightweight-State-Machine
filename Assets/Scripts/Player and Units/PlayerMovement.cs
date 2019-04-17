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
        public bool didHit;
        private bool aiming = false;

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
        }

        private void FixedUpdate()
        {
             distanceTraveled += Vector3.Distance(transform.position, lastPosition);
             lastPosition = transform.position;
                
             
        }

        public void Attack()
        {
            Debug.Log("Attack");
            //animator.SetTrigger("Attack");
            
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


        


    }
}