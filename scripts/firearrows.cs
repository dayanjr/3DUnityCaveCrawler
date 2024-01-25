using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firearrows : MonoBehaviour
{
    public GameObject projectilePrefab;  // The projectile prefab to be fired
    public GameObject projectilePrefabFast;  // The projectile prefab to be fired
    [SerializeField] private float maxChargeTime = 2f;     // The maximum charge time for the shot
    [SerializeField] private float maxLaunchForce = 10f;   // The maximum launch force for the shot
    private float currentChargeTime = 0f;                  // The current charge time for the shot
    private bool isCharging = false;                       // Whether the player is currently charging up the shot
    public Transform bow;
    float timer = 0f;
    public float arrowReloadSpeed;

    public bool RapiDFIRE;
    public AudioSource arrow;
    // Update is called once per frame
    void Update()
    {

        if (timer > 0)
            timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.V))
        {
            RapiDFIRE = !RapiDFIRE;
        }

        if (!RapiDFIRE)
        {// Check for right mouse click input
            if (Input.GetMouseButtonDown(1))
            {
                isCharging = true;
            }
            else if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0))
            {
                if (isCharging && timer <= 0)
                {
                    // Calculate the launch force based on the charge time
                    float launchForce = Mathf.Clamp(currentChargeTime, 0f, maxChargeTime) * maxLaunchForce / maxChargeTime;

                    // Fire the projectile in the direction of the camera raycast
                    FireProjectile(launchForce);
                    timer = 1f;
                    // Reset the charge time and charging flag
                    currentChargeTime = 0f;
                    isCharging = false;
                }
            }

            // Update the charge time while the player is charging up the shot
            if (isCharging)
            {
                currentChargeTime += Time.deltaTime;
            }
        }
        else
        {

            if (Input.GetMouseButton(1) && Input.GetMouseButton(0) && timer <= 0)
            {
                // Calculate the launch force based on the charge time
                float launchForce = maxLaunchForce / maxChargeTime;

                // Fire the projectile in the direction of the camera raycast
                FireProjectile(launchForce);
                timer = arrowReloadSpeed;

                // Reset the charge time and charging flag 
            }
        }
    }
    private void FireProjectile(float launchForce)
    {
        arrow.Play();
        // Get the camera transform and center of the screen
        Transform cameraTransform = Camera.main.transform;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

        // Calculate the direction of the shot based on a raycast from the camera to the center of the screen
        Ray cameraRay = Camera.main.ScreenPointToRay(screenCenter);
        Vector3 shotDirection = cameraRay.direction;
        GameObject projectile;
        if (!RapiDFIRE)
            projectile = Instantiate(projectilePrefab, bow.transform.position, Quaternion.identity);
        else
            projectile = Instantiate(projectilePrefabFast, bow.transform.position, Quaternion.identity);

        // Add force to the projectile in the calculated direction and with the calculated launch force
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.AddForce(shotDirection * launchForce, ForceMode.Impulse);
    }
}
