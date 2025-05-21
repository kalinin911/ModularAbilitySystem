using Assets.Scripts.Components.Character;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AbilitySystem.Core.Consequences
{
    [CreateAssetMenu(fileName = "New DealDamage", menuName = "Ability System/Consequences/DealDamage")]
    public class DealDamageConsequence : Consequence
    {
        [SerializeField] private Stat _damageAmount;
        [SerializeField] private TargetKey _targetKey;
        [SerializeField] private bool _showDamageText = true;

        protected override async UniTask ExecuteImplementationAsync(AbilityExecutionContext context)
        {
            // Try to get the target from context
            if (context.TryGetParameter<Transform>(_targetKey, out var target))
            {
                // Try to apply damage to the target
                var health = target.GetComponent<Health>();
                if (health != null)
                {
                    Debug.Log($"Dealing {_damageAmount.Value} damage to {target.name}");

                    // Show damage text if enabled and effect service is available
                    if (_showDamageText && context.EffectService != null)
                    {
                        context.EffectService.ShowDamageText(target.position, _damageAmount.Value);
                    }

                    health.TakeDamage(_damageAmount.Value);
                }
            }
            else if (context.TryGetParameter<List<Transform>>(_targetKey, out var targets))
            {
                // If we got a list of targets instead
                foreach (var t in targets)
                {
                    var health = t.GetComponent<Health>();
                    if (health != null)
                    {
                        Debug.Log($"Dealing {_damageAmount.Value} damage to {t.name}");

                        // Show damage text if enabled and effect service is available
                        if (_showDamageText && context.EffectService != null)
                        {
                            context.EffectService.ShowDamageText(t.position, _damageAmount.Value);
                        }

                        health.TakeDamage(_damageAmount.Value);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"DealDamageConsequence couldn't find target with key {_targetKey.name}");
            }

            await UniTask.CompletedTask;
        }
    }
}
