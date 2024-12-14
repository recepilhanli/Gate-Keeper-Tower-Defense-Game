using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;


//EffectManager.ChromaticAbernation
public partial class EffectManager
{
    private float _defaultChromaticAbernationIntensity = 0.5f;
    private ChromaticAberration _chromaticAbernation;
    private void InitChromaticAbernation()
    {
        if (postProcessingVolume.profile.TryGet(out _chromaticAbernation))
        {
            _defaultChromaticAbernationIntensity = _chromaticAbernation.intensity.value;
        }
    }


    private async UniTaskVoid SetChromaticAbernationIntensityAsync(float intensity, float duration)
    {
        duration /= 2;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _chromaticAbernation.intensity.value = Mathf.Lerp(_defaultChromaticAbernationIntensity, intensity, elapsed / duration);
            await UniTask.Yield();
        }
        elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _chromaticAbernation.intensity.value = Mathf.Lerp(intensity, _defaultChromaticAbernationIntensity, elapsed / duration);
            await UniTask.Yield();
        }

        _chromaticAbernation.intensity.value = _defaultChromaticAbernationIntensity;
    }

    public void SetChromaticAbernationIntensity(float intensity, float duration)
    {
        SetChromaticAbernationIntensityAsync(intensity, duration).Forget();
    }

}
