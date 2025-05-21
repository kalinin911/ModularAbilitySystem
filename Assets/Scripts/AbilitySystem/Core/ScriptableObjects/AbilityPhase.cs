using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.AbilitySystem.Core
{
    [CreateAssetMenu(fileName = "New Ability Phase", menuName = "Ability System/Ability Phase")]
    public class AbilityPhase : ScriptableObject
    {
        [Serializable]
        public class TimedConsequence
        {
            [Range(0, 1f)]
            public float NormalizedTime;
            public List<Consequence> Consequences = new List<Consequence>();
        }

        [SerializeField] private Stat _duration;
        [SerializeField] private List<TimedConsequence> _timedConsequences = new List<TimedConsequence>();

        private List<TimedConsequence> _sortedConsequences =>_timedConsequences.OrderBy(tc => tc.NormalizedTime).ToList();

        public async UniTask ExecuteAsync(AbilityExecutionContext context)
        {
            float phaseTimer = 0;
            float phaseDuration = _duration.Value;

            Debug.Log($"Starting phase with duration: {phaseDuration}");

            var sortedConsequences = _sortedConsequences;
            int nextConsequenceIndex = 0;

            while (phaseTimer < phaseDuration)
            {
                float normalizedTime = phaseTimer / phaseDuration;

                while(nextConsequenceIndex < sortedConsequences.Count && normalizedTime >= sortedConsequences[nextConsequenceIndex].NormalizedTime)
                {
                    var timedConsequence = sortedConsequences[nextConsequenceIndex];
                    Debug.Log($"Triggering consequences at normalized time {timedConsequence.NormalizedTime}");

                    foreach (var c in timedConsequence.Consequences)
                    {
                        await c.ExecuteAsync(context);
                    }

                    nextConsequenceIndex++;
                }

                await UniTask.Yield();
                phaseTimer += Time.deltaTime;
            }
        }
    }
}
