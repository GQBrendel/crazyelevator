using UnityEngine;

public class FloorMarker : MonoBehaviour
{
    [SerializeField] private int m_index;
    public int Index { get { return m_index; } set { m_index = value; }  }
}
