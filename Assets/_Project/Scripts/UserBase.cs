using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserBase : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer m_MeshRenderer;
    public SkinnedMeshRenderer MeshRenderer { get { return m_MeshRenderer; } }
}
