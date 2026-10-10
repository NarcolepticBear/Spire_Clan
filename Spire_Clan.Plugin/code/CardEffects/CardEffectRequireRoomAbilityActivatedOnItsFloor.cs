using System.Collections;

namespace Spire_Clan.Plugin.Code.CardEffects;

public sealed class CardEffectRequireRoomAbilityActivatedOnItsFloor : CardEffectBase
{
    public override bool TestEffect(
        CardEffectState cardEffectState,
        CardEffectParams cardEffectParams,
        ICoreGameManagers coreGameManagers)
    {
        if (cardEffectParams.roomThatActivatedAbility == null)
        {
            return true;
        }

        return cardEffectParams.roomThatActivatedAbility.GetRoomIndex() == cardEffectParams.selectedRoom;
    }

    public override IEnumerator ApplyEffect(
        CardEffectState cardEffectState,
        CardEffectParams cardEffectParams,
        ICoreGameManagers coreGameManagers,
        ISystemManagers sysManagers)
    {
        yield break;
    }

    public override PropDescriptions CreateEditorInspectorDescriptions() => [];
}
