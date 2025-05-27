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

        public bool IsFullHealth => _currentHealth >= _maxHealth;
        public float HealthPercentage => _currentHealth / _maxHealth;

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

        public void Heal(float amount)
        {
            float oldHealth = _currentHealth;
            _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth); // Don't exceed max health
            float actualHealing = _currentHealth - oldHealth;

            Debug.Log($"{gameObject.name} healed for {actualHealing} HP. Health: {_currentHealth}/{_maxHealth}");
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
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
