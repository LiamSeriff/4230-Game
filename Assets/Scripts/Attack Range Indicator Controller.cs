using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AttackRangeIndicatorController : MonoBehaviour
{
    private bool _visible;
    private SpriteRenderer _spriteRenderer;
    private PlayerAttack _playerAttack;
    private float _attackRange;
    
    // Start is called before the first frame update
    void Start()
    {
        //_playerAttack = GetComponent<PlayerAttack>();
        //_attackRange = _playerAttack.GetAttackRange();
        
        _visible = false;
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            _spriteRenderer.enabled = true;
            _visible = true;
        }
        if (Input.GetMouseButtonDown(1) && _visible)
        {
            
            _spriteRenderer.enabled = false;
            _visible = false;
        }
    }

    //Scale the attack range indicator to the range determined by _attackRange
    private void ScaleAttackRangeIndicator()
    {
        
    }

    public bool Get_visible()
    {
        return _visible;
    }

    public void Set_visible(bool visible)
    {
        if (visible)
        {
            _spriteRenderer.enabled = true;
        }
        else
        {
            _spriteRenderer.enabled = false;
        }
        _visible = visible;
    }
}
