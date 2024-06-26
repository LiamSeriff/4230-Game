using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{
    public GameObject grenade;
    public Image qCooldownIndicator;
    public Image wCooldownIndicator;
    public Image eCooldownIndicator;
    public Image rCooldownIndicator;
    public Image rActivated;
    private Camera _mainCamera;
    private PlayerMovement _playerMovement;
    private PlayerAttack _playerAttack;
    private bool _canQ;
    private bool _canW;
    private bool _canE;
    private bool _canR;
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAttack = GetComponent<PlayerAttack>();
        _canQ = true;
        _canW = true;
        _canE = true;
        _canR = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            if (_canQ)
            {
                Instantiate(grenade, transform.position, Quaternion.identity);
                StartCoroutine(nameof(Q_Cooldown));
                StartCoroutine(AbilityCooldownIndicator(qCooldownIndicator, 5f));
            }
        } 
        else if (Input.GetKeyDown("w"))
        {
            if (_canW)
            {
                BlastAway();
                StartCoroutine(nameof(W_Cooldown));
                StartCoroutine(AbilityCooldownIndicator(wCooldownIndicator, 12f));
            }
        } 
        else if (Input.GetKeyDown("e"))
        {
            if (_canE)
            {
                StartCoroutine(nameof(Dash));
                StartCoroutine(nameof(E_Cooldown));
                StartCoroutine(AbilityCooldownIndicator(eCooldownIndicator, 2f));
            }
        } 
        else if (Input.GetKeyDown("r"))
        {
            if (_canR)
            {
                StartCoroutine(nameof(RapidFire));
                StartCoroutine(nameof(R_Cooldown));
                StartCoroutine(AbilityCooldownIndicator(rActivated, 10f));
                StartCoroutine(AbilityCooldownIndicator(rCooldownIndicator, 30f));
            }
        }
    }

    private IEnumerator Dash()
    {
        _playerMovement.StopAllActions();
        Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        float distance = 650f;
        while (distance > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, mousePos, Time.deltaTime * 25f);
            distance -= 25f;
            yield return null;
        }
        yield return null;
    }

    private void BlastAway()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 5f);
        foreach (Collider2D _collider in enemiesHit)
        {
            if (_collider.gameObject.CompareTag("Enemy"))
            {
                Rigidbody2D colliderRb = _collider.gameObject.GetComponent<Rigidbody2D>();
                Vector2 difference = -(transform.position - colliderRb.gameObject.transform.position).normalized;
                Vector2 force = difference * 5;
                colliderRb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }

    private IEnumerator RapidFire()
    {
        float _defaultAttackSpeed = _playerAttack.GetAttackSpeed();
        _playerAttack.SetAttackSpeed(_defaultAttackSpeed * 1.5f);
        rActivated.fillAmount = 1;
        yield return new WaitForSeconds(10);
        _playerAttack.SetAttackSpeed(_defaultAttackSpeed);
    }

    private IEnumerator AbilityCooldownIndicator(Image cooldownIndicator, float cooldown)
    {
        while (cooldownIndicator.fillAmount > 0)
        {
            cooldownIndicator.fillAmount -= 1 / cooldown * Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    private IEnumerator Q_Cooldown()
    {
        _canQ = false;
        qCooldownIndicator.fillAmount = 1;
        yield return new WaitForSeconds(5f);
        _canQ = true;
    }
    
    private IEnumerator W_Cooldown()
    {
        _canW = false;
        wCooldownIndicator.fillAmount = 1;
        yield return new WaitForSeconds(12f);
        _canW = true;
    }
    
    private IEnumerator E_Cooldown()
    {
        _canE = false;
        eCooldownIndicator.fillAmount = 1;
        yield return new WaitForSeconds(2f);
        _canE = true;
    }
    
    private IEnumerator R_Cooldown()
    {
        _canR = false;
        rCooldownIndicator.fillAmount = 1;
        yield return new WaitForSeconds(30f);
        _canR = true;
    }
}
