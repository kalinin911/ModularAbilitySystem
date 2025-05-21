using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AbilitySystem.Core
{
    [CreateAssetMenu(fileName = "New Ability Behaviour", menuName = "Ability System/Ability Behaviour")]
    public class AbilityBehaviour : ScriptableObject
    {
        [SerializeField] private List<AbilityPhase> _phases = new List<AbilityPhase>();

        public async UniTask ExecuteAsync(AbilityExecutionContext context)
        {
            context.ResetParameters();

            Debug.Log($"Executing behaviour with {_phases.Count} phases");

            foreach (var phase in _phases)
            {
                await phase.ExecuteAsync(context);
            }
        }
    }
}
