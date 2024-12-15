using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshManager : Singleton<NavMeshManager>
{
    public NavMeshSurface surface;

    [ContextMenu("Build NavMesh")]
    public void Build()
    {
        //Deprecated
        //surface.BuildNavMesh();
    }
}
