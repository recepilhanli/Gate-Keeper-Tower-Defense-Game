using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;


//EffectManager.SpeedLines
public partial class EffectManager
{
    [Header("SpeedLines")]
    public GameObject speedLinesGameObject;

    public void SetEnableSpeedLines(bool enable)
    {
        speedLinesGameObject.SetActive(enable);
    }
}
