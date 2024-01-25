using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enabledDisable : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable() {

        StartCoroutine(Disable());

    }
    IEnumerator Disable()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
