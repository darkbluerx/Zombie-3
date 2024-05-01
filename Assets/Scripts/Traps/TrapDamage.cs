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

    public class DamageTrap : MonoBehaviour
    {
        public event Action OnCallAudioEvent;

        [SerializeField] Collider[] colliders;

        [Header("Set Trap damage")]
        [SerializeField, Range(0, 1000)] int _damage = 100;

        [Header("Select a tarp type, play the sound accordingly")]
        public TrapType trapType;

        private void OnEnable()
        {
            OnCallAudioEvent += CallSound;
        }

        private void OnTriggerEnter(Collider collision)
        {
            OnCallAudioEvent?.Invoke();
            //Invoke("OnDisable", 0.1f);

            //Check if the trap hit a game object that has a Health component
            Health health = collision.gameObject.GetComponent<Health>();

            if (health != null)
            {
                //Call the TakeDamage method and pass the damage amount
                int remainingHealth = health.TakeDamage(_damage);
            }
        }

        private void CallSound()
        {
            AudioManager.Instance.GetSound(trapType);
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
