using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class clearTheRoom : MonoBehaviour
{
    public int destroyArea;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, destroyArea);

    }

    void Start()
    {
        var destroy = Physics.OverlapSphere(transform.position, destroyArea);

        foreach (Collider collider in destroy)
        {
            if (collider.tag != "Room" && collider.tag != "Arrow" && collider.tag != "Player" && collider.gameObject != this)
            {
                Destroy(collider.gameObject);
            }
        }       
    }

}
