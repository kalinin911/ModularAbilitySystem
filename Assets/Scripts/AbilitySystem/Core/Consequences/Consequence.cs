using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AbilitySystem.Core
{
    public abstract class Consequence : ScriptableObject
    {
        [SerializeField] protected List<Consequence> _chainedConsequences = new List<Consequence>();

        public async UniTask ExecuteAsync(AbilityExecutionContext context)
        {
            await ExecuteImplementationAsync(context);

            foreach(var consequence in _chainedConsequences)
            {
                await consequence.ExecuteAsync(context);
            }
        }

        protected abstract UniTask ExecuteImplementationAsync(AbilityExecutionContext context);
    }
}
