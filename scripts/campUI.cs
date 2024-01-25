using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class campUI : MonoBehaviour
{
    public Collider triggerArea;
    private GameObject NextLevelUI;


    //private Collider playerCol;

    private void Awake()
    {
        //playerCol = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();
        NextLevelUI = GameObject.FindGameObjectWithTag("UI").transform.GetChild(1).gameObject;
        NextLevelUI.SetActive(false);

        GameObject.Find("point").GetComponent<face>().fire = this.transform;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            NextLevelUI.SetActive(true);
            //CHECK FOR INPUT
            if (Input.GetKey(KeyCode.E))
            {
                //LOAD NEXT LEVEL
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        NextLevelUI.SetActive(false);
    }
}
