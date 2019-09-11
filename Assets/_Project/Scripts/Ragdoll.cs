using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    bool addForce;
    [SerializeField] private Rigidbody rb;
    private void OnEnable()
    {
        StartCoroutine(Forceroutine());
    }

    private IEnumerator Forceroutine()
    {
        addForce = true;
        yield return new WaitForSeconds(2f);
        addForce = false;
    }
    private void FixedUpdate()
    {
        if (addForce)
        {
            rb.AddForce(transform.forward * 100f);
        }
    }
}
