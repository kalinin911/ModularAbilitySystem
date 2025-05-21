using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Assets.Scripts.Components.Character;

namespace Assets.Scripts.AbilitySystem.Core.Consequences
{
    [CreateAssetMenu(fileName = "New DealDamage", menuName = "Ability System/Consequences/DealDamage")]
    public class DealDamageConsequence : Consequence
    {
        [SerializeField] private Stat _damageAmount;
        [SerializeField] private TargetsListKey _targetKey;
        [SerializeField] private bool _showDamageText = true;

        protected override async UniTask ExecuteImplementationAsync(AbilityExecutionContext context)
        {
            if (context.TryGetParameter<List<Transform>>(_targetKey, out var targets) && targets.Count > 0)
            {
                foreach (var target in targets)
                {
                    var health = target.GetComponent<Health>();
                    if (health != null)
                    {
                        Debug.Log($"Dealing {_damageAmount.Value} damage to {target.name}");
                        health.TakeDamage(_damageAmount.Value);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"DealDamageConsequence couldn't find targets with key {_targetKey.name}");
            }

            await UniTask.CompletedTask;
        }

        private void ApplyDamageToTarget(Transform target)
        {
            // Try to apply damage to the target
            var health = target.GetComponent<Health>();

            if (health != null)
            {
                Debug.Log($"Dealing {_damageAmount.Value} damage to {target.name}");

                // Apply the actual damage
                health.TakeDamage(_damageAmount.Value);
            }
            else
            {
                Debug.LogWarning($"Target {target.name} has no Health component!");
            }
        }
    }
}