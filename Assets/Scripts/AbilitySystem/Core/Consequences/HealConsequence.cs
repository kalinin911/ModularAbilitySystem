using Cysharp.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Components.Character;

namespace Assets.Scripts.AbilitySystem.Core.Consequences
{
    [CreateAssetMenu(fileName = "New Heal", menuName = "Ability System/Consequences/Heal")]
    public class HealConsequence : Consequence
    {
        [SerializeField] private Stat _healAmount;
        [SerializeField] private TargetKey _targetKey;
        [SerializeField] private bool _showHealText = true;
        [SerializeField] private Color _healTextColor = Color.green;

        protected override async UniTask ExecuteImplementationAsync(AbilityExecutionContext context)
        {
            if (context.TryGetParameter<Transform>(_targetKey, out var target))
            {
                var health = target.GetComponent<Health>();
                if (health != null)
                {
                    Debug.Log($"Healing {target.name} for {_healAmount.Value} HP");
                    health.Heal(_healAmount.Value);

                    if (_showHealText)
                    {
                        // TODO: Show green healing numbers above target
                        Debug.Log($"+{_healAmount.Value} HP", target);
                    }
                }
                else
                {
                    Debug.LogWarning($"Target {target.name} has no Health component to heal!");
                }
            }
            else
            {
                Debug.LogWarning($"HealConsequence couldn't find target with key {_targetKey.name}");
            }

            await UniTask.CompletedTask;
        }
    }
}