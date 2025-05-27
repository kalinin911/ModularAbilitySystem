using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AbilitySystem.Core.Consequences
{
    [CreateAssetMenu(fileName = "New ClickTarget", menuName = "Ability System/Consequences/ClickTarget")]
    public class ClickTargetConsequence : Consequence
    {
        [SerializeField] private LayerMask _targetLayers = -1; // What can be targeted
        [SerializeField] private TargetKey _outputTargetKey;   // Where to store the target
        [SerializeField] private Stat _maxRange;               // Max targeting range
        [SerializeField] private bool _requireLineOfSight = true;

        protected override async UniTask ExecuteImplementationAsync(AbilityExecutionContext context)
        {
            // Cast ray from camera through mouse position
            Camera camera = Camera.main;
            if (camera == null)
            {
                Debug.LogError("No main camera found for click targeting!");
                return;
            }

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _targetLayers))
            {
                Transform clickedTarget = hit.transform;
                Vector3 casterPosition = context.Caster.position;
                Vector3 targetPosition = clickedTarget.position;

                // Check if target is within range
                float distance = Vector3.Distance(casterPosition, targetPosition);
                if (distance > _maxRange.Value)
                {
                    Debug.Log($"Target {clickedTarget.name} is too far away: {distance:F1} > {_maxRange.Value}");
                    return;
                }

                // Optional: Check line of sight
                if (_requireLineOfSight)
                {
                    Vector3 direction = (targetPosition - casterPosition).normalized;
                    if (Physics.Raycast(casterPosition, direction, distance, ~_targetLayers))
                    {
                        Debug.Log($"No line of sight to {clickedTarget.name}");
                        return;
                    }
                }

                // Valid target found!
                Debug.Log($"Targeted {clickedTarget.name} for healing");
                context.SetParameter(_outputTargetKey, clickedTarget);
            }
            else
            {
                Debug.Log("No valid target clicked");
            }

            await UniTask.CompletedTask;
        }
    }
}