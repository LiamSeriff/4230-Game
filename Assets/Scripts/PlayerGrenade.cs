using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrenade : MonoBehaviour
{
    private Camera _mainCamera;
    private Animator _explosionAnimator;
    private CircleCollider2D _circleCollider;
    private string explode = "Explode";
    
    
    // Start is called before the first frame update
    void Start()
    {
        _explosionAnimator = GetComponent<Animator>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _mainCamera = Camera.main;
        StartCoroutine(MoveTo());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator MoveTo()
    {
        Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(transform.position, mousePos);
        while (distance > .01)
        {
            transform.position = Vector2.MoveTowards(transform.position, mousePos, Time.deltaTime * 20f);
            distance = Vector2.Distance(transform.position, mousePos);
            yield return null;
        }
        transform.position = mousePos;
        Explode();
        yield return null;
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.CompareTag("Enemy"))
        {
            StopCoroutine(nameof(MoveTo));
            Explode();   
        }
    }

    private void Explode()
    {
        //Play animation
        _explosionAnimator.SetTrigger(explode);
        //Increase collider radius
        _circleCollider.radius = .35f;
        //Destroy gameObject
        StartCoroutine(nameof(DelayDestroy));
    }
    
    
}
