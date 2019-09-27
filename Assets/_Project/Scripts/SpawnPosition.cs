using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    [SerializeField] private bool m_isFree;
    public bool IsFree { get { return m_isFree; } set { m_isFree = value; } }

    private void Awake()
    {
        IsFree = true;
    }
}
