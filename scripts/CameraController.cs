using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class CameraController : MonoBehaviour
{
    [System.Serializable]
    public class CameraSettings
    {
        [Header("Camera Move Settings")]
        public float zoomSpeed = 5;
        public float moveSpeed = 5;
        public float rotationSpeed = 5;
        public float originalFOV = 70;
        public float zoomedInFOV = 20;
        public float mouseXSensitivity = 5;
        public float mouseYSensitivity = 5;
        public float maxAngleClamp = 90;
        public float minAngleClamp = -30;

        [Header("Camera Collision")]
        public Transform cameraPosition;
        public LayerMask cameraCollisionLayers;
    }
    [SerializeField]

    [System.Serializable]
    public class CameraInputSettings
    {
        [Header("Camera Input Settings")]
        public string mouseXAxis = "Mouse X";
        public string mouseYAxis = "Mouse Y";
        public string aimingInput = "Fire2";

    }
    [SerializeField]

    public CameraSettings cameraSettings;
    public CameraInputSettings cameraInputSettings;
    public Transform center;
    public Transform target;

    Camera mainCamera;
    Camera UICamera;
    float cameraXRotation = 0;
    float cameraYRotation = 0;
    public Vector3 initialCameraPosition;
    public RaycastHit hit;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        UICamera = mainCamera.GetComponentInChildren<Camera>();
        center = transform.GetChild(0);
        FindPlayer();
        initialCameraPosition = mainCamera.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!target)
        {
            return;
        }

        if (!Application.isPlaying)
        {
            return;
        }

        RotateCamera();
        ZoomCamera();
        HandleCameraCollision();
    }

    void LateUpdate()
    {
        if (target)
        {
            FollowPlayer();
        }
        else
        {
            FindPlayer();
        }

    }

    void FindPlayer()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FollowPlayer()
    {
        Vector3 movementVector = Vector3.Lerp(transform.position, target.transform.position, cameraSettings.moveSpeed * Time.deltaTime);

        transform.position = movementVector;
    }

    void RotateCamera()
    {
        cameraXRotation += Input.GetAxis(cameraInputSettings.mouseYAxis) * cameraSettings.mouseYSensitivity;
        cameraYRotation += Input.GetAxis(cameraInputSettings.mouseXAxis) * cameraSettings.mouseXSensitivity;

        cameraXRotation = Mathf.Clamp(cameraXRotation, cameraSettings.minAngleClamp, cameraSettings.maxAngleClamp);
        cameraYRotation = Mathf.Repeat(cameraYRotation, 360);

        Vector3 rotatingAngle = new Vector3(-cameraXRotation, cameraYRotation, 0);

        Quaternion rotation = Quaternion.Slerp(center.transform.localRotation, Quaternion.Euler(rotatingAngle), cameraSettings.rotationSpeed * Time.deltaTime);
        center.transform.localRotation = rotation;
    }
    
    void ZoomCamera()
    {
        if (Input.GetButton(cameraInputSettings.aimingInput))
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, cameraSettings.zoomedInFOV, cameraSettings.zoomSpeed * Time.deltaTime);
            UICamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, cameraSettings.zoomedInFOV, cameraSettings.zoomSpeed * Time.deltaTime);
        }
        else
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, cameraSettings.originalFOV, cameraSettings.zoomSpeed * Time.deltaTime);
            UICamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, cameraSettings.originalFOV, cameraSettings.zoomSpeed * Time.deltaTime);
        }
    }

    void HandleCameraCollision()
    {
        /*if (Physics.Linecast(target.transform.position + target.transform.up, cameraSettings.cameraPosition.position, out hit, cameraSettings.cameraCollisionLayers))
        {
            Vector3 newCameraPosition = new Vector3(hit.point.x + hit.normal.x * .2f, hit.point.y + hit.normal.y * .8f, hit.point.z + hit.normal.z * .2f);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, newCameraPosition, Time.deltaTime * cameraSettings.moveSpeed);
        }
        else
        {
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, initialCameraPosition, Time.deltaTime * cameraSettings.moveSpeed);
        }

        Debug.DrawLine(target.transform.position + target.transform.up, cameraSettings.cameraPosition.position, Color.blue);*/
    }

}
