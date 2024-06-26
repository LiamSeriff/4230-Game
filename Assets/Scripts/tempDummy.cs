using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class tempDummy : MonoBehaviour
{
    public Image healthBar;
    [SerializeField] private float health;
    private float _maxHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        _maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.CompareTag("PlayerProjectile"))
        {
            TakeDamage(20);
        }
        else if (_collider.gameObject.CompareTag("PlayerGrenade"))
        {
            TakeDamage(40);
        }
    }
        
    private void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / _maxHealth;
    }
}
