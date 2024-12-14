using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    void LateUpdate()
    {
        transform.localEulerAngles += new Vector3(0, _speed * Time.deltaTime, 0);
    }
}
