using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Modes;
using PrimeTween;
using UnityEngine;
using UnityEngine.Rendering.Universal;


//EffectManager.Saturation
public partial class EffectManager
{
    private ColorAdjustments _colorAdjustments;
    private float _defaultSaturation = 0;

    private void InitSaturation()
    {
        if (postProcessingVolume.profile.TryGet(out _colorAdjustments))
        {
            _defaultSaturation = _colorAdjustments.saturation.value;
        }
    }

    private async UniTaskVoid SetSaturationAsync(float saturation, float duration)
    {

        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _colorAdjustments.saturation.value = Mathf.Lerp(_defaultSaturation, saturation, elapsed / duration);
            await UniTask.Yield();
        }

    }

    public void SetSaturation(float saturation, float duration)
    {
        SetSaturationAsync(saturation, duration).Forget();
    }

    public void DeathEffect()
    {
        Time.timeScale = 0.3f;
        SetSaturation(-100, 1);
        GameManager.instance.deathPanel.SetActive(true);
        Tween.ShakeScale(GameManager.instance.deathPanel.transform, new Vector3(.5f, .5f, .5f), 1.25f, 10);
    }

}
