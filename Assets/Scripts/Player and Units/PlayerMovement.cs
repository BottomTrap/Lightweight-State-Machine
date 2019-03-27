using System;
using System.Collections;
using System.Collections.Generic;
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
        private float distanceTraveled;
        public bool didHit = true;
        private bool aiming = false;

        private PlayerStats playerstats;
        public bool isClicked=false;


        public delegate void ViewChangeDelegate();

        public static event ViewChangeDelegate viewChangeDelegate;

        private void Awake()
        {
            playerstats = (PlayerStats) GetComponent(typeof(PlayerStats));
            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            lastPosition = transform.position;
        }

        private void FixedUpdate()
        {
            if (isClicked)
            {
                //didHit = false;   // IT GETS CALLED EVERYTIME, WONT WORK
                distanceTraveled += Vector3.Distance(transform.position, lastPosition);
                lastPosition = transform.position;
                Rotate();
                if (distanceTraveled < playerstats.AP.BaseValue * 10f)
                {
                    Movement();
                }

                if (!didHit)
                {
                    if (Input.GetKeyDown(KeyCode.R) && !aiming)
                    {
                        Attack();
                    }

                    if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        aiming = !aiming;

                        RangedAttack();
                    }

                }
            }
        }

        void Attack()
        {
            animator.SetTrigger("Attack");
            didHit = true;
        }

        void RangedAttack()
        {
            //Get to the aim view
            //Get to the aim control
            //then push R button or Attack button elli houa
            viewChangeDelegate();
            //aiming = !aiming;
        }

        void Movement()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var movement = new Vector3(horizontal, 0, vertical);
            movement = transform.TransformDirection(movement);

            characterController.SimpleMove(movement * Time.deltaTime * moveSpeed);




        }

        void Rotate()
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

        //void OnMouseDown()
        //{
        //    isClicked = true;
        //}
    }
}