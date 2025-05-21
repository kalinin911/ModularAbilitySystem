using UnityEngine;

namespace Assets.Scripts.AbilitySystem.Core
{
    public abstract class ParameterKey : ScriptableObject
    {
        [SerializeField] private string _description;

        public string Description => _description;
    }
}
