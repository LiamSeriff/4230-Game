using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyProjectile : MonoBehaviour
{
    public float projectileSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator MoveInDirection(Vector2 direction)
    {
        Vector2 scaledDirection = Vector2.Scale(direction, new Vector2(10, 10));
        float distance = Vector2.Distance(scaledDirection, transform.position);
        float xVar = Random.Range(-14f, 14f) * Mathf.Sign(direction.x);
        float yVar = Random.Range(-14f, 14f) * Mathf.Sign(direction.y);
        scaledDirection.x += xVar;
        scaledDirection.y += yVar;
        while (distance > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, scaledDirection, Time.deltaTime * projectileSpeed);
            distance = Vector2.Distance(scaledDirection, transform.position);
            yield return null;
        }
        Destroy(gameObject);
        yield return null;
    }
    
    public void StartMoveInDirection(Vector2 direction)
    {
        StartCoroutine(MoveInDirection(direction));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
