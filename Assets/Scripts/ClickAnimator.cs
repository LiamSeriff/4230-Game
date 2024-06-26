using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAnimator : MonoBehaviour
{
    private Animator _anim;
    private SpriteRenderer _spriteRenderer;
    private static readonly int Click = Animator.StringToHash("Click");

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        StartCoroutine(nameof(PlayClickAnimation));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayClickAnimation()
    {
        _anim.SetTrigger(Click);
        yield return new WaitForSeconds(1);
        _spriteRenderer.enabled = false;
        yield return null;
    }
}
