using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
        using UnityEngine.SceneManagement;

public class health : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    public Slider healhbar;
    float timer = 0f;
    public GameObject generator;
    public GameObject died;
    public AudioSource dieSound;
    private bool isDead = false;


    void Start()
    {
        currentHealth = maxHealth;

        healhbar.maxValue = maxHealth;
        healhbar.value = currentHealth;

    }
    public void takeDamage(int damage)
    {
        if (timer <= 0)
        {
            
            currentHealth -= damage;
            healhbar.value = currentHealth;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            Debug.Log("Damage Taken, Health: " + currentHealth);

            timer = 0.25f;
            if (currentHealth <= 0 && !isDead)
            {
                isDead = true;
                StartCoroutine(die());
            }
        }
    }

    IEnumerator die()
    {
        dieSound.Play();
        //PLAY ANIMATION THEN DESTORY
        yield return new WaitForSeconds(2.0f);
        died.SetActive(true);
        Time.timeScale = .2f;
        yield return new WaitForSeconds(1.0f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        timer -= Time.deltaTime;
    }
}
