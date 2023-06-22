using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public class CinemachineCameraShake : MonoBehaviour
    {
        [SerializeField] private float shakeAmplitude;
        [SerializeField] private float shakeFrequence;
        [SerializeField] private float shakeDuration;

        private CinemachineVirtualCamera _virtualCam;
        private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;
        private float _shakeTimer;
        private float _originAmplitude;
        private float _originFrequency;
        private Tween _amplitudeTween;
        private Tween _frequencyTween;

        private void Awake()
        {
            _virtualCam = GetComponent<CinemachineVirtualCamera>();
            _multiChannelPerlin = _virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Start()
        {
            _originAmplitude = _multiChannelPerlin.m_AmplitudeGain;
            _originFrequency = _multiChannelPerlin.m_FrequencyGain;
        }

        private void Update()
        {
            ShakeCountDown();
        }

        public void Shake(float amplitude, float frequency, float time)
        {
            _multiChannelPerlin.m_AmplitudeGain = amplitude;
            _multiChannelPerlin.m_FrequencyGain = frequency;
            _shakeTimer = time;
        }

        public void ShakeSmooth()
        {
            if (_amplitudeTween != null && _amplitudeTween.IsPlaying())
            {
                _amplitudeTween.Rewind();
                _amplitudeTween.Kill();
            }

            if (_frequencyTween != null && _frequencyTween.IsPlaying())
            {
                _frequencyTween.Rewind();
                _frequencyTween.Kill();
            }

            _amplitudeTween = DOTween.To(() => _multiChannelPerlin.m_AmplitudeGain, x => _multiChannelPerlin.m_AmplitudeGain = x, shakeAmplitude, shakeDuration / 2).SetLoops(2, LoopType.Yoyo);
            _frequencyTween = DOTween.To(() => _multiChannelPerlin.m_FrequencyGain, x => _multiChannelPerlin.m_FrequencyGain = x, shakeFrequence, shakeDuration / 2).SetLoops(2, LoopType.Yoyo);
        }

        public void Shake()
        {
            Shake(shakeAmplitude, shakeFrequence, shakeDuration);
        }

        private void ShakeCountDown()
        {
            if (_shakeTimer > 0f)
            {
                _shakeTimer -= Time.deltaTime;

                if (_shakeTimer <= 0f)
                {
                    _multiChannelPerlin.m_AmplitudeGain = _originAmplitude;
                    _multiChannelPerlin.m_FrequencyGain = _originFrequency;
                }
            }
        }
    }
}
