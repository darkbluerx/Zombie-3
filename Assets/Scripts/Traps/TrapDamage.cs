using System;
using UnityEngine;

namespace audio 
{
    public enum TrapType
    {
        Axe,
        Spike,
        Cutter,
        Saw
    }

    [RequireComponent(typeof(AudioSource))]
    public class DamageTrap : MonoBehaviour
    {
        public event Action OnCallAudioEvent;

        [Header("Trap AudioEvents")]
        public AudioEvent[] trapAudioEvents;
        [Space]

        [SerializeField] AudioSource trapSfxAudioSource;

        [SerializeField] Collider[] colliders;

        [Header("Set Trap damage")]
        [SerializeField, Range(0, 1000)] int _damage = 100;

        [Header("Select a tarp type, play the sound accordingly")]
        public TrapType trapType;

        private void Awake()
        {
            trapSfxAudioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            OnCallAudioEvent += CallSound;
        }

        private void OnTriggerEnter(Collider collision)
        {
            OnCallAudioEvent?.Invoke();

            //Check if the trap hit a game object that has a Health component
            Health health = collision.gameObject.GetComponent<Health>();

            if (health != null)
            {
                //Call the TakeDamage method and pass the damage amount
                int remainingHealth = health.TakeDamage(_damage);
            }
        }

        private void PlaySound(TrapType trapType) //Play the sound related to the trapType
        {
            trapAudioEvents[(int)trapType].Play(trapSfxAudioSource);
        }

        private void CallSound()
        {
            GetSound(trapType);
        }

        private void GetSound(TrapType trapType) // Choose the sound based on the trapType
        {
            switch (trapType)
            {
                case TrapType.Axe:
                    PlaySound(trapType);
                    break;
                case TrapType.Spike:
                    PlaySound(trapType);
                    break;
                case TrapType.Cutter:
                    PlaySound(trapType);
                    break;
                case TrapType.Saw:
                    PlaySound(trapType);
                    break;
                default:
                    break;
            }
        }

        public void ActivateTrap()
        {        
            foreach (Collider collider in colliders) // Enable the collider, make damage trap active
            {
                collider.enabled = true;
            }
        }

        private void OnDisable()
        {
            OnCallAudioEvent -= CallSound;
        }
    }
}
