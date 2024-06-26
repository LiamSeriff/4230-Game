using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class REnemyScript : MonoBehaviour
{
    private GameObject player;
    private GameObject projectile;
    public float speed;
    public float attackRange;
    public float attackSpeed;
    public float health;
    public Image healthBar;
    private float _maxHealth;
    private float _distance;
    private SpriteRenderer _spriteRenderer;
    private bool _running;
    private bool _canRun;
    private bool _canAttack;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        projectile = GameObject.FindGameObjectWithTag("EnemyProjectile");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _maxHealth = health;
        _running = false;
        _canRun = true;
        _canAttack = true;
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
        
        if (_distance <= attackRange && _canAttack)
        {
            StartCoroutine(nameof(FireBurst));
        }
        if (_distance >= attackRange && !_running)
        {
            
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else if (_distance <= attackRange - 1 && _canRun && !_running)
        {
            StartCoroutine(MoveAway(-direction));
        }
    }

    private IEnumerator FireBurst()
    {
        StartCoroutine(nameof(AttackCooldown));
        for (int i = 0; i < 3; i++)
        {
            GameObject projectileClone = Instantiate(projectile, transform.position, Quaternion.identity);
            projectileClone.GetComponent<EnemyProjectile>().StartMoveInDirection(new Vector2(player.transform.position.x, player.transform.position.y) - new Vector2(transform.position.x, transform.position.y));
            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator MoveAway(Vector2 direction)
    {
        _running = true;
        float xVar = Random.Range(-3f, 3f) * Mathf.Sign(direction.x);
        float yVar = Random.Range(-3f, 3f) * Mathf.Sign(direction.y);
        Vector2 runTo = new Vector2(transform.position.x + xVar, transform.position.y + yVar) + direction;
        float runDistance = Vector2.Distance(transform.position, runTo);
        while (runDistance > 0.01)
        {
            transform.position = Vector2.MoveTowards(transform.position, runTo, speed * Time.deltaTime);
            runDistance = Vector2.Distance(transform.position, runTo);
            yield return null;
        }
        transform.position = runTo;
        _running = false;
        StartCoroutine(nameof(RunCooldown));
        yield return null;
    }
    
    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(attackSpeed);
        _canAttack = true;
    }

    private IEnumerator RunCooldown()
    {
        _canRun = false;
        yield return new WaitForSeconds(7f);
        _canRun = true;
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.gameObject.CompareTag("Terrain"))
        {
            StartCoroutine(nameof(StopAllActions));
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

    private IEnumerator StopAllActions()
    {
        StopCoroutine(nameof(MoveAway));
        yield return null;
    }
    
    private void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / _maxHealth;
    }
}
