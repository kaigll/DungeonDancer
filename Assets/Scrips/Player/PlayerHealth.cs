using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private HealthUI healthUI;
    private int currentHealth;
    static PlayerHealth instance;
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoadManager.DontDestroyOnLoad(this.gameObject);
            instance = this;
            healthUI.SetHealthBar(1);
        }
        else
        {
            Destroy(this.gameObject);
            healthUI.SetHealthBar((float)currentHealth / maxHealth);
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthUI.SetHealthBar((float)currentHealth / maxHealth);
    }

    public int GetHealth() { return currentHealth; }
    public void Damage(int damage)
    {
        if (currentHealth - damage < 0) { currentHealth = 0; Death(); }
        else if (currentHealth - damage == 0) { currentHealth = 0; Death(); }
        else currentHealth = currentHealth - damage;
        Debug.Log(currentHealth);
        healthUI.SetHealthBar((float)currentHealth / maxHealth);
    }
    private void Heal(int healAmount)
    {
        if (currentHealth + healAmount >= maxHealth) currentHealth = maxHealth;
        else currentHealth = currentHealth + healAmount;
        healthUI.SetHealthBar((float)currentHealth / maxHealth);
    }
    private void Death()
    {
        //Destroy(this.gameObject);
        /*
         * 
         * MAKE THIS
         * 
         */
    }

    private void SetHealth(int health)
    {
        currentHealth = health;
        healthUI.SetHealthBar((float)currentHealth / maxHealth);
    }
}

/*
 * FIX THE DEATH
*/