using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MEnemyScript : MonoBehaviour
{
    private GameObject player;
    private SpriteRenderer _spriteRenderer;
    public float speed;
    public float health;
    public Image healthBar;
    private float _maxHealth;
    private float _distance;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        
        _distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        
        if (player.transform.position.x - transform.position.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
        
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
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
