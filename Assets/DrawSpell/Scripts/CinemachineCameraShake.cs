using Cinemachine;
using FriendsGamesTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrawSpell
{
    public class CinemachineCameraShake : MonoBehaviourHasInstance<CinemachineCameraShake>
    {
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private float amplitude = 1.2f;
        [SerializeField] private float frequency = 2.0f;
        [SerializeField] private CinemachineVirtualCamera[] virtualCameras;
        [SerializeField] private bool enableMultiShake;
        private CinemachineBasicMultiChannelPerlin[] noises;
        private bool canShake = true;

        protected override void Awake()
        {
            base.Awake();
            noises = new CinemachineBasicMultiChannelPerlin[virtualCameras.Length];
            for (int i = 0; i < virtualCameras.Length; i++)
            {
                noises[i]=virtualCameras[i].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
         
        }

        public void Shake()
        {
            if (!enableMultiShake && !canShake)
            {
                return;
            }

            foreach (var noise in noises)
            {
                noise.m_AmplitudeGain = amplitude;
                noise.m_FrequencyGain = frequency;
            }
         

            StartCoroutine(StopShake());
        }

        private IEnumerator StopShake()
        {
            canShake = false;

            yield return new WaitForSeconds(duration);


            foreach (var noise in noises)
            {
                noise.m_AmplitudeGain = 0;
            }
         
            canShake = true;
        }
    }
}