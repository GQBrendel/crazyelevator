using System.Collections;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float _timeUntilDestroy = 1f;

    void Start()
    {
        StartCoroutine(AutoDestroyRoutine());
    }
    private IEnumerator AutoDestroyRoutine()
    {
        yield return new WaitForSeconds(_timeUntilDestroy);
        Destroy(gameObject);
    }
}
