using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AbilitySystem.Core
{
    [CreateAssetMenu(fileName = "New RectOverlap", menuName = "Ability System/Consequences/RectOverlap")]
    public class RectOverlapConsequence : Consequence
    {
        private const float DIMENSION_MULTIPLIER = 0.5f;

        [SerializeField] private Stat _width;
        [SerializeField] private Stat _height;
        [SerializeField] private Stat _depth;
        [SerializeField] private float _forwardOffest = 1f;
        [SerializeField] private LayerMask _targetLayers;
        [SerializeField] TargetsListKey _outputTargetsKey;
        [SerializeField] bool _showEffect = true;

        protected override async UniTask ExecuteImplementationAsync(AbilityExecutionContext context)
        {
            Transform casterTransform = context.Caster;
            Vector3 center = casterTransform.position + casterTransform.forward * _forwardOffest;

            Vector3 halfExtents = new Vector3(_width.Value * DIMENSION_MULTIPLIER, _height.Value * DIMENSION_MULTIPLIER, _depth.Value * DIMENSION_MULTIPLIER);

            Debug.DrawLine(center - halfExtents, center + halfExtents, Color.red, 1f);

            if(_showEffect && context.EffectService != null)
            {
                context.EffectService.PlayQuickSlashEffect(center, casterTransform.forward);
            }

            // Find targets using Physics.OverlapBox
            Collider[] hits = Physics.OverlapBox(center, halfExtents, casterTransform.rotation, _targetLayers);

            // Process targets
            List<Transform> targets = new List<Transform>();

            foreach(var hit in hits)
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
                foreach (var chainedConequence in _chainedConsequences)
                {
                    await chainedConequence.ExecuteAsync(context);
                }
            }

            await UniTask.CompletedTask;

        }
    }
}
