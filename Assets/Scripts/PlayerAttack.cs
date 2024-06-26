using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;
    private RaycastHit2D _hit;
    private Vector2 _mousePos;
    public GameObject projectile;
    private GameObject _projectileClone;
    private bool _canAttack;
    
    // Start is called before the first frame update
    void Start()
    {
        _canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireProjectile(RaycastHit2D target)
    {
        _projectileClone = Instantiate(projectile, transform.position, Quaternion.identity);
        _projectileClone.GetComponent<Projectile>().StartMoveToTarget(target);
        StartCoroutine(nameof(AttackCooldown));
    }

    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(1 / attackSpeed);
        _canAttack = true;
    }

    public bool GetCanAttack()
    {
        return _canAttack;
    }

    public void SetAttackSpeed(float _attackSpeed)
    {
        attackSpeed = _attackSpeed;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }
    
    public float GetAttackRange()
    {
        return attackRange;
    }

    public void SetAttackRange(float newRange)
    {
        attackRange = newRange;
    }
}
