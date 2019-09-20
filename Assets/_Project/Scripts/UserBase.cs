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
}
