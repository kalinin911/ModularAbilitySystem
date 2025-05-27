using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Components.Character;

namespace Assets.Scripts.AbilitySystem.Core.Consequences
{
    [CreateAssetMenu(fileName = "New AOEHeal", menuName = "Ability System/Consequences/AOEHeal")]
    public class AOEHealConsequence : Consequence
    {
        [SerializeField] private Stat _healAmount;
        [SerializeField] private Stat _radius;
        [SerializeField] private LayerMask _allyLayers;
        [SerializeField] private bool _includeCaster = true;
        [SerializeField] private bool _showHealEffect = true;
        [SerializeField] private GameObject _healEffectPrefab;
        [SerializeField] private bool _onlyHealDamaged = false; // Only heal if not at full HP

        protected override async UniTask ExecuteImplementationAsync(AbilityExecutionContext context)
        {
            Transform caster = context.Caster;
            Vector3 center = caster.position;

            // Find all allies in radius
            Collider[] hits = Physics.OverlapSphere(center, _radius.Value, _allyLayers);

            List<Transform> healTargets = new List<Transform>();

            foreach (var hit in hits)
            {
                // Skip caster if not including self
                if (!_includeCaster && hit.transform == caster)
                    continue;

                var health = hit.GetComponent<Health>();
                if (health != null)
                {
                    // Skip if only healing damaged and target is at full health
                    if (_onlyHealDamaged && health.IsFullHealth)
                        continue;

                    healTargets.Add(hit.transform);

                    // Apply healing
                    health.Heal(_healAmount.Value);
                    Debug.Log($"AOE Heal: {hit.name} healed for {_healAmount.Value} HP");

                    // Show individual heal effect
                    if (_showHealEffect && _healEffectPrefab != null)
                    {
                        GameObject effect = Instantiate(_healEffectPrefab,
                            hit.transform.position + Vector3.up * 1f,
                            Quaternion.identity);

                        // Auto-destroy effect after 2 seconds
                        Destroy(effect, 2f);
                    }
                }
            }

            Debug.Log($"AOE Heal affected {healTargets.Count} allies");

            await UniTask.CompletedTask;
        }
    }
}