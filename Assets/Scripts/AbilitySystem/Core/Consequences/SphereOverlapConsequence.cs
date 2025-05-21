using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AbilitySystem.Core.Consequences
{
    [CreateAssetMenu(fileName = "New SphereOverlap", menuName = "Ability System/Consequences/SphereOverlap")]
    public class SphereOverlapConsequence : Consequence
    {
        [SerializeField] private Stat _radius;
        [SerializeField] private float _forwardOffset = 1f;
        [SerializeField] private LayerMask _targetLayers;
        [SerializeField] TargetsListKey _outputTargetsKey;
        [SerializeField] bool _showEffect = true;
        [SerializeField] private bool isStompEffect = false;

        protected override async UniTask ExecuteImplementationAsync(AbilityExecutionContext context)
        {
            Transform casterTransform = context.Caster;
            Vector3 center = casterTransform.position + (casterTransform.forward * _forwardOffset);

            Debug.DrawLine(center, center + Vector3.up * 0.1f, Color.blue, 1f);

            if (_showEffect && context.EffectService != null)
            {
                if (isStompEffect)
                {
                    context.EffectService.PlayStompEffect(center);
                }
                else
                {
                    context.EffectService.PlayCircleSlashEffect(center);
                }
            }

            // Find targets using Physics.OverlapSphere
            Collider[] hits = Physics.OverlapSphere(center, _radius.Value, _targetLayers);

            // Process targets
            List<Transform> targets = new List<Transform>();
            foreach (var hit in hits)
            {
                // Skip the caster itself
                if (hit.transform == casterTransform)
                    continue;

                Debug.Log($"Hit target: {hit.transform.name}");
                targets.Add(hit.transform);
            }

            // Store targets in context for chained consequences
            context.SetParameter(_outputTargetsKey, targets);

            // For each target, execute chained consequences with that target set in context
            foreach (var target in targets)
            {
                // Set current target for chained consequences
                context.SetParameter(_outputTargetsKey, targets);

                // Execute chained consequences for this target
                foreach (var chainedConsequence in _chainedConsequences)
                {
                    await chainedConsequence.ExecuteAsync(context);
                }
            }

            await UniTask.CompletedTask;
        }
    }
}