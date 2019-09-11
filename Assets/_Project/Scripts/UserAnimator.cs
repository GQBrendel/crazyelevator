using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserAnimator : MonoBehaviour
{
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
        GetComponent<Animator>().SetBool("isIdle", true);
        GetComponent<Animator>().SetBool("isWalk", false);
        GetComponent<Animator>().SetBool("isRun", false);
    }
}
