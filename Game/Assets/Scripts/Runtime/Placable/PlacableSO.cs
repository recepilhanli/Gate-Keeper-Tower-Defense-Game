using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{

    [CreateAssetMenu(fileName = "NewPlacable", menuName = "Game/Placable Object", order = 0)]
    public class PlacableSO : ScriptableObject
    {
        public GameObject prefab;
        public int price;
        public Mesh silhouetteMesh;
        public Vector3 silhouetteLocalRotation;
        public Vector3 silhouetteLocalScale = Vector3.one;
        public Vector3 silhouettePositionOffset;
    }

}