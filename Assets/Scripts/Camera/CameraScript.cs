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
       
        [Header("Camera Control Variables")]
        public float rotateSpeed = 5f;
        public float mouseSensitivity = 10f;
        public bool didCameraArrive = true;

        [Header("AimView Stuff")]
        public Vector2 pitchMinMax = new Vector2(-40, 85);
        public Vector3 aimViewOffset;
        public Transform rotator;
        private float yaw;
        private float pitch;
        private Vector3 currentRotation;


        [SerializeField]
        public Vector3 offset; //Private variable to store the offset distance between the player and camera
        private Transform oldTransform; // old transform to retransition back into


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
            //Third Person Camera offset
            offset = new Vector3(-1, 1.6f, -3);
            //Calculate and store the offset value by getting the distance between the player's position and camera's position.

            didCameraArrive = true; //For the smooth camera transition , wait if it arrived to go to next state
           

        }


        public void CameraTransition(Transform target) // Starts Coroutine to begin a smooth transition using a Coroutine and MoveTowards
        {
             float angle = target.eulerAngles.y;
             Quaternion rotation = Quaternion.Euler(0, angle, 0);
            StartCoroutine(MoveTo(this.transform, target.position +rotation* offset,15.0f)); 
        }

        
        //This is a moveTo method for moving things OUTSIDE an Update
        IEnumerator MoveTo(Transform mover, Vector3 destination, float speed)
        {
            while (mover.position != destination)
            {
                mover.position = Vector3.MoveTowards(
                    mover.position,
                    destination,
                    speed * Time.deltaTime);
                // Wait a frame and move again.
                yield return null;
            }
            yield return didCameraArrive = true;
        }

        public void  IsoCameraTransition() //Same as CamreaTransition but for reverting back to the old view 
        {
            didCameraArrive = false;
            StartCoroutine(MoveTo(this.transform, oldTransform.position, 15.0f));
        }

        public void CameraMovement(Transform target) //player follow !!
        {
            if (didCameraArrive) //if camera arrived to transition destination, then start moving
            {
                StopCoroutine("MoveTo");
                // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
                transform.position = target.position + offset;
                float angle = target.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                transform.position = target.position + rotation * offset;
            }
        }


        //IsoMetric Camera Controls at the beginning
        public void IsoMovement()
        {
            if (didCameraArrive)
            {
                //Basic movement then basic rotation
                StopCoroutine("MoveTo");
                float translationX = Input.GetAxis("Horizontal");
                float translationY = Input.GetAxis("Vertical");
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
                    float mouseTranslationY = Input.GetAxis("Mouse Y");
                    transform.RotateAround(transform.position, Vector3.up, mouseTranslationX);
                    transform.Rotate(Vector3.right, mouseTranslationY);
                }
            }
        }



        public void AimView()
        {
            rotator = playerTransform.GetChild(2).transform; //Rotator is a sphere collider gameObject put inside the model's head
            //Rotator to control the view from a head side  , control the rotation
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
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