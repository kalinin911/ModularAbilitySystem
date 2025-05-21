using UnityEngine;

namespace Assets.Scripts.AbilitySystem.Core
{
    [CreateAssetMenu(fileName = "New Stat", menuName = "Ability System/Stat")]
    public class Stat : ScriptableObject
    {
        [SerializeField] private float _value = 1f;
        [SerializeField] private string _description;

        public float Value => _value;
        public string Description => _description;
    }
}
