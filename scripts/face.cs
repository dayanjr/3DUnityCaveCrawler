using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class face : MonoBehaviour
{
    public Transform fire;

    void Update()
    {
        transform.LookAt(fire);
    }
}
