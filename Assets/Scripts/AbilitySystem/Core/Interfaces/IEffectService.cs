using UnityEngine;

namespace Assets.Scripts.AbilitySystem.Core
{
    public interface IEffectService
    {
        void PlayCircleSlashEffect(Vector3 position);
        void PlayQuickSlashEffect(Vector3 position, Vector3 direction);
        void PlayStompEffect(Vector3 position);
        void ShowDamageText(Vector3 position, float amount);
    }
}
