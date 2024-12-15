using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;


//EffectManager.Vignette
public partial class EffectManager
{
    private Vignette _vignette;
    private float _defaultVignetteIntensity = 0;

    private void InitVignette()
    {
        if (postProcessingVolume.profile.TryGet(out _vignette))
        {
            _defaultVignetteIntensity = _vignette.intensity.value;
        }
    }

    private async UniTaskVoid SetVignetteIntensityAsync(float intensity, float duration)
    {
        duration /= 2;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _vignette.intensity.value = Mathf.Lerp(_defaultVignetteIntensity, intensity, elapsed / duration);
            await UniTask.Yield();
        }
        elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _vignette.intensity.value = Mathf.Lerp(intensity, _defaultVignetteIntensity, elapsed / duration);
            await UniTask.Yield();
        }

        _vignette.intensity.value = _defaultVignetteIntensity;
    }

    public void SetVignetteIntensity(float intensity, float duration)
    {
        SetVignetteIntensityAsync(intensity, duration).Forget();
    }
}
