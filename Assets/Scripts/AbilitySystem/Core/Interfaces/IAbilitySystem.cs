namespace Assets.Scripts.AbilitySystem.Core
{
    public interface IAbilitySystem
    {
        bool IsAnyAbilityExecuting { get; }
        void NotifyAbilityStarted(Ability ability);
        void NotifyAbilityEnded(Ability ability);
    }
}
