using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserBase : MonoBehaviour
{

    public delegate void UserTransportedHandler(UserBase user);
    public UserTransportedHandler OnUserTransported;

    public delegate void UserDiedHandler();
    public UserDiedHandler OnUserDied;

    public delegate void ReachedDestinationHandler(UserBase user);
    public ReachedDestinationHandler OnReachedDestination;

    protected FloorData DesiredFloor { get; set; }

    [SerializeField] private SkinnedMeshRenderer m_MeshRenderer;
    [SerializeField] protected GameObject _destroyEffectPrefab;
    [SerializeField] protected GameObject _root;

    public bool _ragDoll;

    public SkinnedMeshRenderer MeshRenderer { get { return m_MeshRenderer; } }
    public int FinalFloor { get { return DesiredFloor.Index; } }

    public void TurnInvisible()
    {
        MeshRenderer.enabled = false;
    }

    public virtual void SetSpawnPosition(SpawnPosition spawnPosition)
    {
    }

    public virtual void Spawn(FloorData spawnedFloor, FloorData desiredFloor, ElevatorController elevator, Material material, GameManager gameManager)
    {
    }
    public virtual void Despawn()
    {
        if (_ragDoll)
        {
            return;
        }
        if (this)
        {
            Instantiate(_destroyEffectPrefab, transform.position, _destroyEffectPrefab.transform.rotation);
            Destroy(_root);
        }
    }
}
