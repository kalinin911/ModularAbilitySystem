using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AbilitySystem.Core
{
    public class AbilityExecutionContext
    {
        private Dictionary<ParameterKey, object> _parameters = new Dictionary<ParameterKey, object>();
        private Transform _caster;
        public AbilityExecutionContext(Transform caster)
        {
            _caster = caster;
        }

        public Transform Caster => _caster;

        public void SetParameter<T>(ParameterKey key, T value)
        {
            if (_parameters.ContainsKey(key))
            {
                _parameters[key] = value;
            }
            else
            {
                _parameters.Add(key, value);
            }
        }

        public bool TryGetParameter<T>(ParameterKey key, out T value)
        {
            if (_parameters.TryGetValue(key, out object objectValue) && objectValue is T typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }

        public void ResetParameters()
        {
            _parameters.Clear();
        }
    }
}
