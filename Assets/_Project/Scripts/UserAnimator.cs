using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UserAnimator : MonoBehaviour
{
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Walk()
    {
        GetComponent<Animator>().SetBool("isWalk", true);
        GetComponent<Animator>().SetBool("isIdle", false);
    }
    public void Run()
    {
        GetComponent<Animator>().SetBool("isRun", true);
        GetComponent<Animator>().SetBool("isIdle", false);
    }
    public void Idle()
    {
        _animator.SetBool("isIdle", true);
        _animator.SetBool("isWalk", false);
        _animator.SetBool("isRun", false);
    }
    public void Angry()
    {
        _animator.SetBool("Angry", true);
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isWalk", false);
        _animator.SetBool("isRun", false);
    }
    public void EndAngry()
    {
        _animator.SetBool("Angry", false);
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isWalk", false);
        _animator.SetBool("isRun", false);
    }
}
