using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDistance : MonoBehaviour
{
    public bool looted = false;
    public Collider triggerArea;
    private GameObject chestUI;

    public Animator chestAnim;
    public Collider playerCol;

    private void Start()
    {
        playerCol = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();
        chestUI = GameObject.FindGameObjectWithTag("UI").transform.GetChild(0).gameObject;
        chestUI.SetActive(false);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            chestUI.SetActive(true);
            //CHECK FOR INPUT
            if (Input.GetKey(KeyCode.E))
            {
                looted = true;
                //ADD FUNCTIONALITY FOR UPGRADES
                playerCol.gameObject.GetComponent<health>().takeDamage(-20);

                GameObject.FindGameObjectWithTag("UI").transform.GetChild(2).transform.gameObject.SetActive(true);


                chestAnim.enabled = true;


                chestUI.SetActive(false);
                triggerArea.enabled = false;
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other = playerCol)
        {
            chestUI.SetActive(false);
        }
    }
}
