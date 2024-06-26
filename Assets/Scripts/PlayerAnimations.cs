using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;
    private PlayerMovement _playerMovement;
    private Vector2 _mousePos;
    private RaycastHit2D _hit;
    private string Up = "Up";
    private string Right = "Right";
    private string Down = "Down";
    private string Left = "Left";

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        _mousePos = _playerMovement.GetMousePos();
        _hit = _playerMovement.Get_hit();
        //Check direction
        FindAngle();
    }
        

    private void FindAngle()
    {
        Vector2 targetPos;
        if (_hit.collider is not null)
        {
            targetPos = _hit.collider.gameObject.transform.position;
        }
        else
        {
            targetPos = _mousePos;
        }

        float angle = Mathf.Atan((targetPos.y - transform.position.y) / (targetPos.x - transform.position.x)) * Mathf.Rad2Deg;

        if (targetPos.x - transform.position.x < 0) //Target is left of current pos
        {
            if (angle <= -45f)
            {
                //Up
                _animator.SetTrigger(Up);
            }
            else if (angle is > -45f and <= 45f)
            {
                //Left
                _animator.SetTrigger(Left);
            }
            else
            {
                //Down
                _animator.SetTrigger(Down);
            }
        }
        else //Target is right of current pos
        {
            if (angle <= -45f)
            {
                //Down
                _animator.SetTrigger(Down);
            }
            else if (angle is > -45f and <= 45f)
            {
                //Right
                _animator.SetTrigger(Right);
            }
            else
            {
                //Up
                _animator.SetTrigger(Up);
            }
        } 
    }
    
}
