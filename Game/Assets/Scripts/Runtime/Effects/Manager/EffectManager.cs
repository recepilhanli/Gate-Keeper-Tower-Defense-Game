using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

//EffectManager - Base class of all partial classes
public partial class EffectManager : Singleton<EffectManager>
{
    public Volume postProcessingVolume; //Global Volume

    private void Start()
    {
        InitChromaticAbernation();
        InitVignette();
    }
}
