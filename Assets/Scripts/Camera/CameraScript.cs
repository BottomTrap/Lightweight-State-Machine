using UnityEngine;
using System.Collections;



namespace RG
{
    [RequireComponent(typeof(NewMatrixBlender))]
    public class CameraScript : MonoBehaviour
    {
        [Header("Perspective Switcher Variables")]
        public float fov = 60f;

        public float near = .3f;
        public float far = 1000f;
        public float orthographicSize = 50f;

        //PerspectiveSwticher Private Variables
        Camera m_camera;

        private Matrix4x4 ortho,
            perspective;

        private float aspect;
        private NewMatrixBlender blender;




        private Transform
            playerTransform; //Public variable to store a reference to the player transform from ObjectClick.cs when delegate is activated


        [Header("State Bools")] public bool orthoOn = true;
        public bool transitionning = false;
        public bool aimView = false;



        [Header("Camera Control Variables")] public float rotateSpeed = 5f;
        public float mouseSensitivity = 10f;

        [Header("AimView Stuff")] private float yaw;
        private float pitch;
        private Vector3 currentRotation;

        public Vector3 playerOffset;
        public float distanceFromOffset;
        public Transform rotator;
        public Transform target;

        public Vector2 pitchMinMax = new Vector2(-40, 85);
        public float rotationSmoothTime = 8f;

        [SerializeField]
        private Vector3 offset; //Private variable to store the offset distance between the player and camera


        private Transform oldTransform; // old transform to retransition back into
        //float horizontal = 0.0f;




        //FiniteStateMachine
        //private IsometricState isometricManager;
        //private Animator animator;

        private void Awake()
        {
            //animator = GetComponent<Animator>();
        }

        public Transform PlayerClicked(int commandPoints)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray,out hit))
                {
                    if (hit.transform.tag == "PlayerUnit" && commandPoints > 0)
                    {
                        return hit.transform;
                    }
                }

                return null;
            }else

            return null;
        }
        void Start()
        {
            playerTransform = target.transform;
            //***Delegate subscription 
            //ObjectClick.characterSelectDelegate += CamTrans;
            //PlayerMovement.viewChangeDelegate += AimViewOn;

            //Third Person Camera Stuff
            offset = new Vector3(-2, 1, -4);
            //Calculate and store the offset value by getting the distance between the player's position and camera's position.
            //offset = transform.position - player.transform.position;

            //FSM Stuff
            //isometricManager = animator.GetBehaviour<IsometricState>  ();
            //isometricManager.camBehaviour = this;


            //Perspective switcher stuff

            aspect = (float) Screen.width / (float) Screen.height;
            ortho = Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize,
                orthographicSize, near, far);
            perspective = Matrix4x4.Perspective(fov, aspect, near, far);
            m_camera = GetComponent<Camera>();
            m_camera.projectionMatrix = ortho;
            orthoOn = true;
            blender = (NewMatrixBlender) GetComponent(typeof(NewMatrixBlender));
            blender.BlendToMatrix(ortho, 1f, 8, true);

            // 
            orthoOn = true;
        }

        void Update()
        {

        }

        //void LateUpdate() //NOTE TO SELF : LATE UPDATE DOES WEIRD THINGS WHEN ITS WITH UPDATE SIMULTANEOUSLY 
        //{
        //    if (!orthoOn)
        //    {
        //        CameraMovement();
        //    }
        //}
        

        void CamTrans() //activated by delegate
        {
           
            transitionning = true;
            orthoOn = false;
            Debug.Log(orthoOn);
            Debug.Log(transitionning);




        }

        public void CameraTransition() // Cool looking lerp
        {
            blender.BlendToMatrix(perspective, 1f, 8, false);
            oldTransform = transform;
            float angle = playerTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 firstLerp =
                new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, firstLerp, rotateSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, playerTransform.position + offset,
                rotateSpeed * Time.deltaTime);
            //transform.LookAt(player.transform);
            Vector3 direction = playerTransform.position - transform.position;
            Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
            transitionning = false;
            orthoOn = false;
        }

        public void IsoCameraTransition()
        {
            blender.BlendToMatrix(ortho, 1f, 8, true);
            transform.position = Vector3.Lerp(transform.position, oldTransform.position, rotateSpeed * Time.deltaTime);
            transform.rotation =
                Quaternion.Lerp(transform.rotation, oldTransform.rotation, rotateSpeed * Time.deltaTime);
        }

        public void CameraMovement() //player follow !! to make after we made camera transition
        {
            // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
            transform.position = playerTransform.position + offset;
            // if (Input.GetMouseButton(1))
            // {
            //     horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
            // }
            // player.transform.Rotate(0, horizontal, 0);
            float angle = playerTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.position = playerTransform.position + rotation * offset;
            transform.LookAt(playerTransform);


        }

        public void IsoMovement()
        {
            //Keyboard Scroll

            float translationX = Input.GetAxis("Horizontal");
            float translationY = Input.GetAxis("Vertical");
            float fastTranslationX = 2 * Input.GetAxis("Horizontal");
            float fastTranslationY = 2 * Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(fastTranslationX, 0, fastTranslationY);
            }
            else
            {
                transform.Translate(translationX, 0, translationY, Space.World);
            }

            //Mouse Scroll

            var mousePosX = Input.mousePosition.x;
            var mousePosY = Input.mousePosition.y;
            int scrollDistance = 3;
            float scrollSpeed = 15f;

            //Horizontal Camera Movement
            if (Input.GetKey(KeyCode.Space))
            {
                if (mousePosX < scrollDistance)
                {
                    //horizontal left
                    transform.Translate(-1, 0, 1);
                }

                if (mousePosY >= Screen.width - scrollDistance)
                {
                    //horizontal right
                    transform.Translate(1, 0, -1);
                }

                //Vertical Camera Movement
                if (mousePosY < scrollDistance)
                {
                    //scrolling down
                    transform.Translate(-1, 0, -1);
                }

                if (mousePosY >= Screen.height - scrollDistance)
                {
                    //scrolling up
                    transform.Translate(1, 0, 1);
                }
            }

            if (Input.GetMouseButton(1))
            {
                translationX = Input.GetAxis("Mouse X");
                transform.Rotate(axis: new Vector3(0, 1, 0), angle: translationX * scrollSpeed * Time.deltaTime,
                    Space.World);
            }
        }



        public void AimView()
        {
            //reticle or corshair or whatever control and appearance
            // aim and orientation animation when models ready 
            //that's about it
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
            currentRotation =
                Vector3.Lerp(currentRotation, new Vector3(pitch, yaw), rotationSmoothTime * Time.deltaTime);

            transform.eulerAngles = currentRotation;
            Vector3 e = transform.eulerAngles;
            e.x = 0;
            Vector3 g = transform.eulerAngles;
            g.y = 0;

            rotator.eulerAngles = new Vector3(g.x, rotator.eulerAngles.y, rotator.eulerAngles.z);

            target.eulerAngles = e;
        }

        public void AimViewOn()
        {
            aimView = !aimView;
        }
    }
}