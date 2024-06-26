using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    public Image healthBar;
    public float health;
    private float _maxHealth;
    private bool invuln;
    
    // Start is called before the first frame update
    void Start()
    {
        invuln = false;
        _maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0)
        {
            SceneManager.LoadScene("Death Scene");
        }

        if (Input.GetKeyDown("i"))
        {
            invuln = !invuln;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (!invuln)
        {
            if (_collider.gameObject.CompareTag("Enemy")) 
            {
                TakeDamage(15);
            }
            else if (_collider.gameObject.CompareTag("EnemyProjectile"))
            {
                TakeDamage(10);
            }
        }
    }
    
    private void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / _maxHealth;
    }
    
}
