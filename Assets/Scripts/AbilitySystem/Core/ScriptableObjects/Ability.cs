using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace Assets.Scripts.AbilitySystem.Core
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "Ability System/Ability")]
    public class Ability : ScriptableObject
    {
        [SerializeField] private string _abilityId;
        [SerializeField] private AbilityBehaviour _behaviour;
        [SerializeField] private Stat _cooldownDuration;
        [SerializeField] private KeyCode _inputKey;

        private float _currentCooldown = 0f;
        private bool _isOnCooldown => _currentCooldown > 0f;

        public bool IsExecuting { get; private set; } = false;
        public string AbilityId => _abilityId;
        public KeyCode InputKey => _inputKey;

        public event Action<Ability> OnAbilityExecutionStateChanged;

        public async UniTask<bool> TryExecuteAsync(AbilityExecutionContext context, IAbilitySystem abilitySystem)
        {
            if (_isOnCooldown || IsExecuting || abilitySystem.IsAnyAbilityExecuting)
            {
                return false;
            }

            IsExecuting = true;
            abilitySystem.NotifyAbilityStarted(this);
            OnAbilityExecutionStateChanged?.Invoke(this);

            try
            {
                await _behaviour.ExecuteAsync(context);
                _currentCooldown = _cooldownDuration.Value;
            }

            catch (Exception ex)
            {
                Debug.LogError($"Error executing ability {AbilityId}: {ex.Message}");
                IsExecuting = false;
                abilitySystem.NotifyAbilityEnded(this);
                OnAbilityExecutionStateChanged?.Invoke(this);
                return false;
            }

            finally
            {
                IsExecuting = false;
                abilitySystem.NotifyAbilityEnded(this);
                OnAbilityExecutionStateChanged?.Invoke(this);
            }

            return true;
        }

        public void UpdateCooldown(float deltaTime)
        {
            if (_currentCooldown > 0f)
            {
                _currentCooldown -= deltaTime;
                if (_currentCooldown < 0f)
                    _currentCooldown = 0f;
            }
        }

        public float GetCooldownPercentage()
        {
            return Mathf.Clamp01(_currentCooldown / _cooldownDuration.Value);
        }
    }
}
