using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;


namespace RG
{
   
    public class CameraScript : MonoBehaviour
    {
        
        public GameModeManager statesManager;




         public Transform playerTransform; //container to current player transform to follow during its actions phase

       



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
        public Vector2 yawMinMax = new Vector2(-50, 50);
        public float rotationSmoothTime = 8f;
        public Vector3 aimViewOffset;

        [SerializeField]
        public Vector3 offset; //Private variable to store the offset distance between the player and camera

        private Transform oldTransform; // old transform to retransition back into

        public AnimationCurve curve;
       
        //private Animator animator;

        private void Awake()
        {
            //animator = GetComponent<Animator>();
            
        }


        //Get the reference of the player clicked so that we can control him later on in the Action Mode.
        public bool PlayerClicked(int commandPoints)
        {
            
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.tag == "PlayerUnit" && commandPoints > 0)
                        {
                            playerTransform = hit.transform;
                            rotator = playerTransform.GetComponent<PlayerMovement>().rotator;
                            return true;

                        }

                        return false;
                    }

                    return false;
                }
                else

                    return false;
            
            
        }

        void Start()
        {
            oldTransform = transform;
            //Third Person Camera Stuff
            offset = new Vector3(-1, 1.6f, -3);
            //Calculate and store the offset value by getting the distance between the player's position and camera's position.
           
           
           

        }


        public void CameraTransition(Transform target) // Cool looking lerp
        {
            //    float currentTime = 0;
            //    float totalTime = 5.0f;
            //    while (currentTime < totalTime)
            //    {
            //        
            //        oldTransform = transform;
            //        float angle = target.eulerAngles.y;
            //        Quaternion rotation = Quaternion.Euler(0, angle, 0);
            //        Vector3 firstLerp =
            //            new Vector3(target.position.x, target.position.y, GetComponentInParent<Transform>().position.z);
            //        GetComponentInParent<Transform>().position = Vector3.Lerp(GetComponentInParent<Transform>().position, firstLerp, curve.Evaluate(currentTime / totalTime));
            //        GetComponentInParent<Transform>().position = Vector3.Lerp(GetComponentInParent<Transform>().position, target.position + offset, curve.Evaluate(
            //            currentTime / totalTime));
            //        //transform.LookAt(player.transform);
            //        Vector3 direction = target.position - GetComponentInParent<Transform>().position;
            //        Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
            //        GetComponentInParent<Transform>().rotation = Quaternion.Lerp(transform.rotation, toRotation, curve.Evaluate(currentTime / totalTime));
            //        currentTime += Time.deltaTime;
            //    }

            //StartCoroutine(MoveObject(transform.position, playerTransform.position - offset, 1f));
            float angle = target.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);

            GetComponentInParent<Transform>().position = Vector3.MoveTowards(GetComponentInParent<Transform>().position, target.position + rotation * offset, 10.0f);
        }

        IEnumerator MoveObject(Vector3 source, Vector3 target, float overTime)
        {
            float startTime = Time.time;
            while (Time.time < startTime + overTime)
            {
                transform.position = Vector3.MoveTowards(source, target, Time.deltaTime);
                transform.LookAt(target);
                yield return null;
            }
            transform.position = target;
        }

        public void  IsoCameraTransition()
        {
            float currentTime=0;
            float totalTime = 1f;
            while (currentTime < totalTime)
            {
                
                GetComponentInParent<Transform>().position = Vector3.Lerp(transform.position, oldTransform.position, curve.Evaluate(currentTime / totalTime));
                GetComponentInParent<Transform>().LookAt(playerTransform);
                currentTime += Time.deltaTime;
                
            }
            
        }

        public void CameraMovement(Transform target) //player follow !! to make after we made camera transition
        {
            // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
            GetComponentInParent<Transform>().position = target.position + offset;
            float angle = target.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            GetComponentInParent<Transform>().position = target.position + rotation * offset;
        }

        public void IsoMovement()
        {
            //Keyboard Scroll

            float translationX = Input.GetAxis("Horizontal");
            float translationY = Input.GetAxis("Vertical") ;
            float fastTranslationX = 2 * Input.GetAxis("Horizontal");
            float fastTranslationY = 2 * Input.GetAxis("Vertical");
            Vector3 dir = new Vector3(translationX, translationY, translationY);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                GetComponentInParent<Transform>().Translate(fastTranslationX, 0, fastTranslationY, Space.Self);
            }
            else
            {
               transform.Translate(dir, Space.Self);
            }


            if (Input.GetMouseButton(1))
            {
                float mouseTranslationX = Input.GetAxis("Mouse X");
                Vector3 rot = GetComponentInParent<Transform>().forward * -mouseTranslationX * rotateSpeed;
                //transform.Rotate(axis: new Vector3(0, 1, 0), angle: translationX * scrollSpeed * Time.deltaTime,Space.Self);
                //GetComponentInParent<Transform>().Rotate(new Vector3(0, translationX, 0), Space.Self);
                transform.RotateAround(transform.position,Vector3.up, mouseTranslationX);
            }
        }



        public void AimView()
        {
            rotator = playerTransform.GetChild(2).transform;
            //reticle or corshair or whatever control and appearance
            // aim and orientation animation when models ready 
            //that's about it
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
            //yaw = Mathf.Clamp(yaw, yawMinMax.x, yawMinMax.y);
            currentRotation = Vector3.Lerp(currentRotation, new Vector3(pitch, yaw,0), rotateSpeed* Time.deltaTime);

            GetComponentInParent<Transform>().eulerAngles = currentRotation;
            Vector3 e = GetComponentInParent<Transform>().eulerAngles;
            e.x = 0;
            Vector3 g = GetComponentInParent<Transform>().eulerAngles;
            g.y = 0;
            rotator.eulerAngles = new Vector3(g.x, rotator.eulerAngles.y, rotator.eulerAngles.z);

            rotator.eulerAngles = e;
            GetComponentInParent<Transform>().position = Vector3.Lerp(transform.position, playerTransform.position - aimViewOffset, rotateSpeed);
        }
       
    }
}