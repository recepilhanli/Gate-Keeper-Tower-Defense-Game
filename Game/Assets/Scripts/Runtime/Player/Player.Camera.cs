using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Game.PlayerOperations
{


    //Player.Input
    public partial class Player
    {
        [Header("Camera")]
        [SerializeField] private CinemachineVirtualCamera _playerCamera;
        [SerializeField] private CinemachineImpulseSource _impulseSource;
        private CinemachineBasicMultiChannelPerlin _noise;

        private void InitCamera()
        {
            _noise = _playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            ShakeCamera(1, .35f);
        }

        public void CameraImpulse(Vector3 velocity, float duration)
        {
            _impulseSource.m_DefaultVelocity = velocity;
            _impulseSource.m_ImpulseDefinition.m_ImpulseDuration = duration;
            _impulseSource.GenerateImpulse();
        }

        public void ShakeCamera(float amplitudeGain, float frequencyGain)
        {
            _noise.m_AmplitudeGain = amplitudeGain;
            _noise.m_FrequencyGain = frequencyGain;
        }

        public CinemachineVirtualCamera GetCamera()
        {
            return _playerCamera;
        }

    }

}
