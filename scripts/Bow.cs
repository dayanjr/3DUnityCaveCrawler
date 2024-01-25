using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

    [System.Serializable]
    public class BowSettings
    {
        [Header("Arrow Settings")]
        public float arrowCount;
        public Rigidbody arrowPrefab;
        public Transform arrowPosition;
        public Transform arrowEquipParent;
        public float arrowForce = 5;

        [Header("Bow Equip Settings")]
        public Transform equipPosition;
        public Transform equipParent;
        public Transform unequipPosition;
        public Transform unequipParent;

        [Header("Bow String Settings")]
        public Transform bowString;
        public Transform stringInitialPosition;
        public Transform stringPulledPosition;
        public Transform stringInitialParent;

        
    }
    [SerializeField]

    public BowSettings bowSettings;

    [Header("Crosshair Settings")]
    public GameObject crosshairPrefab;
    public GameObject currentCrosshair;

    public Rigidbody currentArrow;

    bool canPullString = false;
    bool canFireArrow = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableArrow()
    {
        bowSettings.arrowPosition.gameObject.SetActive(true);
    }

    public void DisableArrow()
    {
        bowSettings.arrowPosition.gameObject.SetActive(false);
    }

    public void PullString()
    {
        bowSettings.bowString.transform.position = bowSettings.stringPulledPosition.position;
        bowSettings.bowString.transform.parent = bowSettings.stringPulledPosition;
    }

    public void ReleaseString()
    {
        bowSettings.bowString.transform.position = bowSettings.stringInitialPosition.position;
        bowSettings.bowString.transform.parent = bowSettings.stringInitialParent;

    }

    public void EquipBow()
    {
        transform.position = bowSettings.equipPosition.position;
        transform.rotation = bowSettings.equipPosition.rotation;
        transform.parent = bowSettings.equipParent;
    }
    public void UnequipBow()
    {
        transform.position = bowSettings.unequipPosition.position;
        transform.rotation = bowSettings.unequipPosition.rotation;
        transform.parent = bowSettings.unequipParent;
    }

    public void ShowCrosshair(Vector3 crosshairPosition)
    {
        if (!currentCrosshair)
        {
            currentCrosshair = Instantiate(crosshairPrefab) as GameObject;
        }

        currentCrosshair.transform.position = crosshairPosition;
        currentCrosshair.transform.LookAt(Camera.main.transform);
    }

    public void HideCrosshair()
    {
        if (currentCrosshair)
        {
            Destroy(currentCrosshair);
        }
    }

    public void FireArrow(Vector3 hitPoint)
    {
        Vector3 direction = hitPoint - bowSettings.arrowPosition.position;
        currentArrow = Instantiate(bowSettings.arrowPrefab, bowSettings.arrowPosition.position, bowSettings.arrowPosition.rotation);

        currentArrow.AddForce(direction * bowSettings.arrowForce, ForceMode.VelocityChange);
    }

}
