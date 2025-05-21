using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Character
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private GameObject _deathEffectPrefab;

        private float _currentHealth;

        public event Action<float, float> OnHealthChanged;
        public event Action OnDeath;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;
            Debug.Log($"{gameObject.name} took {amount} damage. Health: {_currentHealth}/{_maxHealth}");
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die() 
        {
            Debug.Log($"{gameObject.name} died!");

            OnDeath?.Invoke();

            // Spawn death VFX if available
            if (_deathEffectPrefab != null)
            {
                Instantiate(_deathEffectPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }

    }
}
