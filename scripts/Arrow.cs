using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody rigidBody;
    BoxCollider boxCollider;
    bool disableRotation;
    public float destroyTime = 10f;

    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();

        Destroy(this.gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!disableRotation)
        {
            transform.rotation = Quaternion.LookRotation(rigidBody.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 6 && collision.gameObject.layer != 7)
        {
            disableRotation = true;
            rigidBody.isKinematic = true;
            boxCollider.isTrigger = true;
            transform.parent = collision.transform;
            if(collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
            {
                collision.gameObject.GetComponent<enemyMove>().takeDamage(damage);
                Debug.Log(damage);

            }
        }
    }
}
