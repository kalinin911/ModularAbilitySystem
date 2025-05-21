using Assets.Scripts.AbilitySystem.Core;
using Assets.Scripts.Components.Management;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Components.Character
{
    public class HeroController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private List<Ability> _abilities = new List<Ability>();

        private AbilityExecutionContext _executionContext;
        private IAbilitySystem _abilitySystem;
        private IEffectService _effectService;

        //Property for abilities for UI purposes
        public IReadOnlyList<Ability> Abilities => _abilities;

        private void Awake()
        {
            _abilitySystem = FindObjectOfType<AbilitySystemManager>();

            if(_abilitySystem == null)
            {
                Debug.LogError("AbilitySystemManager not found in the scene.");

                // Create a fallback
                _abilitySystem = gameObject.AddComponent<AbilitySystemManager>();
            }

            // Find the effect service in the scene
            _effectService = FindObjectOfType<EffectService>();

            // Create the execution context
            _executionContext = new AbilityExecutionContext(transform, _effectService);
        }

        private void Update()
        {
            // Handle movement
            HandleMovement();

            // Update ability cooldowns
            foreach (var ability in _abilities)
            {
                ability.UpdateCooldown(Time.deltaTime);
            }

            // Handle ability input
            HandleAbilityActivation();
        }

        private void HandleAbilityActivation()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontal, 0f, vertical);

            // Normalize movement to prevent faster diagonal movement
            if (movement.magnitude > 1f)
            {
                movement.Normalize();
            }

            // Move the character
            transform.position += movement * _moveSpeed * Time.deltaTime;

            // Rotate character to face movement direction
            if (movement != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }
        }

        private void HandleMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontal, 0f, vertical);

            // Normalize movement to prevent faster diagonal movement
            if (movement.magnitude > 1f)
            {
                movement.Normalize();
            }

            // Move the character
            transform.position += movement * _moveSpeed * Time.deltaTime;

            // Rotate character to face movement direction
            if (movement != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }
        }

        private async UniTask ActivateAbility(Ability ability)
        {
            Debug.Log($"Attempting to use ability: {ability.AbilityId}");
            bool success = await ability.TryExecuteAsync(_executionContext, _abilitySystem);

            if (success)
            {
                Debug.Log($"Successfully used ability: {ability.AbilityId}");
            }
            else
            {
                Debug.Log($"Failed to use ability: {ability.AbilityId} (cooldown or already executing)");
            }
        }
    }
}
