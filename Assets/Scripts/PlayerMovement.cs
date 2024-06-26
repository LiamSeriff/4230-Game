using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed;
    private Vector2 _mousePos;
    private RaycastHit2D _hit;
    private RaycastHit2D[] _hitArray;
    public Camera mainCamera;
    public Collider2D playerCollider;
    private float _playerAttackRange;
    private Animator _animator;
    public GameObject clickAnimation;
    private GameObject _clickAnimationClone;
    private PlayerAttack _playerAttack;
    private AttackRangeIndicatorController _aric;
    private bool _canAttack;
    private bool _visible;
    //Animation Names
    private string Up = "Up";
    private string Right = "Right";
    private string Down = "Down";
    private string Left = "Left";

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 170;
        _animator = GetComponent<Animator>();
        _playerAttackRange = gameObject.GetComponent<PlayerAttack>().GetAttackRange();
        _playerAttack = gameObject.GetComponent<PlayerAttack>();
        _aric = GetComponentInChildren<AttackRangeIndicatorController>();
    }

    // Update is called once per frame
    void Update()
    {
        _canAttack = _playerAttack.GetCanAttack();
        _visible = _aric.Get_visible();
        
        //Attack Range Indicator is visible and player left clicks
        if (_visible && Input.GetMouseButtonDown(0))
        {
            _aric.Set_visible(false);
            StopAllActions();
            _mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            
            //Find nearest enemy collider
            _hitArray = Physics2D.CircleCastAll(_mousePos, 200f, Vector2.up, 0f);
            _hit = _hitArray[0];
            
            //Set nearestColliderToMouse to the closest enemy collider
            foreach (RaycastHit2D hit in _hitArray)
            {
                if (hit.collider.tag is "Enemy")
                {
                    _hit = hit;
                    break;
                }
            }
            
            float nearestColliderDistance = Vector2.Distance(_mousePos, new Vector2(_hit.transform.position.x, _hit.transform.position.y));
            
            //Find actual nearest enemy to _mousePos
            foreach (RaycastHit2D hit in _hitArray)
            {
                if (hit.collider.tag is "Enemy")
                {
                    float hitDistance = Vector2.Distance(_mousePos, new Vector2(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y));
                    if (hitDistance < nearestColliderDistance)
                    {
                        _hit = hit;
                        nearestColliderDistance = hitDistance;
                    }
                }
            }
            
            if (_hit.collider is null)
            {
                //Didn't click enemy so move to location
                StartCoroutine(nameof(MoveTo));

                //Play click animation
                _clickAnimationClone = Instantiate(clickAnimation, new Vector3(_mousePos.x, _mousePos.y, 0),
                    Quaternion.identity);
                Destroy(_clickAnimationClone, 2f);
            }
            else switch (_hit.collider.tag)
            {
                case "Player":
                    StartCoroutine(nameof(MoveTo));
                    break;
                //Check if enemy is in attack range
                case "Enemy" when playerCollider.Distance(_hit.collider).distance <= _playerAttackRange:
                    //Attack
                    StartCoroutine(nameof(Attack));
                    break;
                case "Enemy":
                    //Move to attack range & fire
                    StartCoroutine(nameof(MoveToAttackRange));
                    break;
                case "Terrain":
                    StartCoroutine(nameof(MoveTo));
                    break;
                default:
                    print("ERROR: PlayerMovement.cs: _hit.collider.tag switch");
                    print("Tag:" + _hit.collider.tag);
                    break;
            }
            
        }
        //Player right click
        else if (Input.GetMouseButtonDown(1))
        {
            //Cancel previous move command
            StopAllActions();

            _mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            //Check if right-clicking enemy collider
            _hit = Physics2D.Raycast(_mousePos, Vector2.up, 0f);
            
            //Set Animation
            PlayAnimation();

            //Check if hit enemy collider
            if (_hit.collider is null)
            {
                //Didn't click enemy so move to location
                StartCoroutine(nameof(MoveTo));

                //Play click animation
                _clickAnimationClone = Instantiate(clickAnimation, new Vector3(_mousePos.x, _mousePos.y, 0),
                    Quaternion.identity);
                Destroy(_clickAnimationClone, 2f);
            }
            //Different behavior based on tag of clicked collider
            else switch (_hit.collider.tag)
            {
                    case "Player":
                        StartCoroutine(nameof(MoveTo));
                        break;
                    //Check if enemy is in attack range
                    case "Enemy" when playerCollider.Distance(_hit.collider).distance <= _playerAttackRange:
                        //Attack
                        StartCoroutine(nameof(Attack));
                        break;
                    case "Enemy":
                        //Move to attack range & fire
                        StartCoroutine(nameof(MoveToAttackRange));
                        break;
                    case "PlayerGrenade":
                        StartCoroutine(nameof(MoveTo));
                        break;
                    case "Terrain":
                        StartCoroutine(nameof(MoveTo));
                        //Play click animation
                        _clickAnimationClone = Instantiate(clickAnimation, new Vector3(_mousePos.x, _mousePos.y, 0),
                            Quaternion.identity);
                        Destroy(_clickAnimationClone, 2f);
                        break;
                    case "EnemyProjectile":
                        StartCoroutine(nameof(MoveTo));
                        break;
                    default:
                        print("ERROR: PlayerMovement.cs: _hit.collider.tag switch");
                        print("Tag:" + _hit.collider.tag);
                        break;
            }
        }

        //Stop all player actions
        if (Input.GetKeyDown("s"))
        {
            StopAllActions();
        }
    }
    
    //Move the player to mousePos at a constant speed
    private IEnumerator MoveTo()
    {
        float distance = Vector2.Distance(transform.position, _mousePos);
        while (distance > .01)
        {
            transform.position = Vector2.MoveTowards(transform.position, _mousePos, Time.deltaTime * speed);
            distance = Vector2.Distance(transform.position, _mousePos);
            yield return null;
        }
        transform.position = _mousePos;
        SetToIdle();
        yield return null;
    }

    private IEnumerator MoveToAttackRange()
    {
        while (_hit.collider is not null)
        {
            float distance = playerCollider.Distance(_hit.collider).distance;
            while (distance > _playerAttackRange)
            {
                transform.position = Vector2.MoveTowards(transform.position, _hit.collider.gameObject.transform.position, Time.deltaTime * speed);
                distance = playerCollider.Distance(_hit.collider).distance;
                PlayAnimation();
                yield return null;
                SetToIdle();
            }
            if (_canAttack)
            {
                _playerAttack.FireProjectile(_hit);
            }
            yield return new WaitForSeconds(.1f);
            SetToIdle();
        }
        SetToIdle();
        yield return null;
    }

    private IEnumerator Attack()
    {
        SetToIdle();
        while (_hit.collider is not null)
        {
            float distance = playerCollider.Distance(_hit.collider).distance;
            if (distance <= _playerAttackRange)
            {
                if (_canAttack)
                {
                    _playerAttack.FireProjectile(_hit);
                }
            }
            else
            {
                StartCoroutine(nameof(MoveToAttackRange));
                StopCoroutine(nameof(Attack));
            }
            yield return null;
        }

        SetToIdle();
        yield return null;
    }
    
    private void PlayAnimation()
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
                _animator.SetBool(Up, true);
            }
            else if (angle is > -45f and <= 45f)
            {
                //Left
                _animator.SetBool(Left, true);
            }
            else
            {
                //Down
                _animator.SetBool(Down, true);
            }
        }
        else //Target is right of current pos
        {
            if (angle <= -45f)
            {
                //Down
                _animator.SetBool(Down, true);
            }
            else if (angle is > -45f and <= 45f)
            {
                //Right
                _animator.SetBool(Right, true);
            }
            else
            {
                //Up
                _animator.SetBool(Up, true);
            }
        } 
    }

    private void SetToIdle()
    {
        _animator.SetBool(Up, false);
        _animator.SetBool(Down, false);
        _animator.SetBool(Right, false);
        _animator.SetBool(Left, false);
    }

    public Vector2 GetMousePos()
    {
        return _mousePos;
    }

    public RaycastHit2D Get_hit()
    {
        return _hit;
    }

    public void StopAllActions()
    {
        StopCoroutine(nameof(MoveTo));
        StopCoroutine(nameof(MoveToAttackRange));
        StopCoroutine(nameof(Attack));
        SetToIdle();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Terrain"))
        {
            StopAllActions();   
        }
    }
}
