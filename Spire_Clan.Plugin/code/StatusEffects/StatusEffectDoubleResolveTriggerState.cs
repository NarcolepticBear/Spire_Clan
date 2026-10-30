using Conductor.Interfaces;

namespace Spire_Clan.Plugin.Code.StatusEffects;

public sealed class StatusEffectDoubleResolveTriggerState : StatusEffectState, IStatusEffectModifyTriggerFireCount
{
    public int GetTriggerFireCount(
        CharacterTriggerData.Trigger trigger,
        CharacterTriggerState? triggerStateContext)
    {
        return trigger == CharacterTriggerData.Trigger.PostCombat ? 1 : 0;
    }
}
