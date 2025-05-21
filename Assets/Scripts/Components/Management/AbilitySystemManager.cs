using Assets.Scripts.AbilitySystem.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Components.Management
{
    public class AbilitySystemManager : MonoBehaviour, IAbilitySystem
    {
        private List<Ability> _currentlyExecutingAbilities = new List<Ability>();
        public bool IsAnyAbilityExecuting => _currentlyExecutingAbilities.Count > 0;

        public void NotifyAbilityStarted(Ability ability)
        {
            if (_currentlyExecutingAbilities.Contains(ability))
            {
                return;
            }

            _currentlyExecutingAbilities.Add(ability);
        }

        public void NotifyAbilityEnded(Ability ability)
        {
            _currentlyExecutingAbilities.Remove(ability);
        }
    }
}
