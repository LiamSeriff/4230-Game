using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed;
    private PlayerMovement _playerMovement;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartMoveToTarget(RaycastHit2D target)
    {
        StartCoroutine(MoveToTarget(target));
    }

    private IEnumerator MoveToTarget(RaycastHit2D target)
    {
        while (true)
        {
            if (target.collider is not null)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                    target.collider.gameObject.transform.position, Time.deltaTime * projectileSpeed);
                yield return null;
            }
            else
            {
                Destroy(gameObject);
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
