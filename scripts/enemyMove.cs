using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMove : MonoBehaviour
{
    private Transform playerPos;
    private UnityEngine.AI.NavMeshAgent agent;

    public int maxHealth;
    public int currentHealth;

    float timer = 0f;
    float distance;

    public int damage;

    public float viewDistance;
    public float stoppingDistance;
    public bool lookat;
    bool playerSeen = false;
    public Animator anim;
    public AudioSource sound;

    void Start()
    {
        currentHealth = maxHealth;
        distance += GetComponent<CharacterController>().radius;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        stoppingDistance += GetComponent<CharacterController>().radius;
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        GetComponent<CharacterController>().enabled = false;
        agent.enabled = false;
        GetComponent<Collider>().enabled = false;
        //PLAY ANIMATION THEN DESTORY
        anim.speed = 0.1f;
        yield return new WaitForSeconds(5f);

        Destroy(gameObject);
    }

    void attackPlayer()
    {
        playerPos.gameObject.GetComponent<health>().takeDamage(damage);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            transform.position += new Vector3(0, -1, 0) * Time.deltaTime;
        }
        else if (playerPos != null)
        {
            distance = Vector3.Distance(transform.position, playerPos.position);

            
            if (playerSeen)
            {
                if (distance > stoppingDistance)
                {
                    agent.SetDestination(playerPos.position);
                    if (!lookat)
                    {
                        anim.SetFloat("Blend", 1);
                        anim.SetBool("attack", false);
                    }
                }
                else
                {
                    
                    agent.SetDestination(agent.transform.position);


                    if (lookat)
                        transform.LookAt(playerPos);
                    else
                    {
                        anim.SetFloat("Blend", 0);
                        anim.SetBool("attack", true);
                    }
                }
            }
            else
            {
                if (distance < viewDistance)
                {
                    playerSeen = true;

                    sound.pitch = sound.pitch * 1 + (Random.Range(-1f, 1f));
                    sound.enabled = true;
                }
            }


            if (timer > 0)
                timer -= Time.deltaTime;
            else
            {
                if (distance <= stoppingDistance)
                {
                   

                    attackPlayer();

                    float x = transform.rotation.eulerAngles.x;
                    float z = transform.rotation.eulerAngles.z;
                    Quaternion rot = Quaternion.LookRotation(playerPos.position - transform.position);
                    transform.rotation = Quaternion.Euler(x, rot.eulerAngles.y, z);
                }

                timer = 1f;
            }
        }
    }
}
