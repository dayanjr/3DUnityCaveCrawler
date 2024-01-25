using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementAnim))]
[RequireComponent(typeof(Animator))]
public class InputProcessor : MonoBehaviour
{
    MovementAnim movementScript;

    [System.Serializable]
    public class InputSettings
    {
        public string forwardInput = "Vertical";
        public string strafeInput = "Horizontal";
        public string sprintInput = "Sprint";
        public string aim = "Fire2";
        public string fire = "Fire1";
    }
    [SerializeField]

    [Header("Camera And Character Syncing")]
    public float viewDistance = 5;
    public float viewSpeed = 5;

    [Header("Aiming Settings")]
    public RaycastHit hit;
    public LayerMask aimLayers;
    public Ray ray;

    [Header("Spine Settings")]
    public Transform spine;
    public Vector3 spineOffset;

    [Header("Head Rotation Settings")]
    public float lookAtPoint = 2.8f;

    [Header("Head Rotation Settings")]
    public float gravityValue = 1.2f;

    public InputSettings inputSettings;
    public Bow bowScript;
    public Transform cameraCenter;
    public Transform mainCamera;
    public Animator animator;
    public CharacterController characterController;
    public bool isAiming;
    public bool testAim;
    public bool hitDetected;


    // Start is called before the first frame update
    void Start()
    {
        movementScript = GetComponent<MovementAnim>();
        cameraCenter = Camera.main.transform.parent;
        mainCamera = Camera.main.transform;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis(inputSettings.forwardInput) != 0 || Input.GetAxis(inputSettings.strafeInput) != 0)
        {
            RotateToCameraView();
        }
        
        if (!characterController.isGrounded)
        {
            characterController.Move(new Vector3(transform.position.x, transform.position.y - gravityValue, transform.position.z));
        }
        isAiming = Input.GetButton(inputSettings.aim);

        if (testAim)
        {
            isAiming = true;
        }

        movementScript.AnimateCharacter(Input.GetAxis(inputSettings.forwardInput), Input.GetAxis(inputSettings.strafeInput));
        movementScript.SprintCharacter(Input.GetButton(inputSettings.sprintInput));
        movementScript.CharacterAim(isAiming);

        if (isAiming)
        {
            Aim();
            bowScript.EquipBow();
            movementScript.CharacterPullString(Input.GetButton(inputSettings.fire));
            if (Input.GetButtonUp(inputSettings.fire))
            {
                movementScript.CharacterFireArrow();
                if (hitDetected)
                {
                    bowScript.FireArrow(hit.point);
                }
                else
                {
                    bowScript.FireArrow(ray.GetPoint(300f));
                }
            }
        }
        else
        {
            bowScript.UnequipBow();
            bowScript.HideCrosshair();
            DisableArrow();
            ReleaseString();
        }
    }

    private void LateUpdate()
    {
        if (isAiming)
        {
            RotateCharacterSpine();
        }
    }

    void RotateToCameraView()
    {
        Vector3 cameraCenterPosition = cameraCenter.position;

        Vector3 viewPoint = cameraCenterPosition + (cameraCenter.forward * viewDistance);
        Vector3 direction = viewPoint - transform.position;

        Quaternion viewRotation = Quaternion.LookRotation(direction);
        viewRotation.x = 0;
        viewRotation.z = 0;

        Quaternion finalRotation = Quaternion.Lerp(transform.rotation, viewRotation, Time.deltaTime * viewSpeed);
        transform.rotation = finalRotation;
    }

    void Aim()
    {
        Vector3 cameraPosition = mainCamera.position;
        Vector3 direction = mainCamera.forward;

        ray = new Ray(cameraPosition, direction);

        if (Physics.Raycast(ray, out hit, 500f, aimLayers))
        {
            hitDetected = true;
            Debug.DrawLine(ray.origin, hit.point, Color.green);
            bowScript.ShowCrosshair(hit.point);
        }
        else
        {
            hitDetected = false;
            bowScript.HideCrosshair();
        }
    }

    void RotateCharacterSpine()
    {
        spine.LookAt(ray.GetPoint(50));
        spine.Rotate(spineOffset);
    }

    public void PullString()
    {
        bowScript.PullString();
    }

    public void EnableArrow()
    {
        bowScript.EnableArrow();
    }
    public void DisableArrow()
    {
        bowScript.DisableArrow();
    }

    public void ReleaseString()
    {
        bowScript.ReleaseString();
    }

}
