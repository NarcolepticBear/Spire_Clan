using System.Collections;

namespace Spire_Clan.Plugin.Code.CardEffects;

public sealed class CardEffectDamagePerUnitsOnFloor : CardEffectBase
{
    public override bool TestEffect(
        CardEffectState cardEffectState,
        CardEffectParams cardEffectParams,
        ICoreGameManagers coreGameManagers)
    {
        return cardEffectParams.targets.Count > 0;
    }

    public override IEnumerator ApplyEffect(
        CardEffectState cardEffectState,
        CardEffectParams cardEffectParams,
        ICoreGameManagers coreGameManagers,
        ISystemManagers sysManagers)
    {
        int targetRoomIndex = cardEffectParams.targets[0].GetCurrentRoomIndex();
        RoomState? targetRoom = coreGameManagers.GetRoomManager()?.GetRoom(targetRoomIndex);
        if (targetRoom == null)
        {
            yield break;
        }

        List<CharacterState> friendlyUnits = [];
        targetRoom.AddCharactersToList(friendlyUnits, Team.Type.Monsters);
        int unitCount = friendlyUnits.Count;

        int damage = cardEffectState.GetParamInt() * unitCount;
        if (damage <= 0)
        {
            yield break;
        }

        foreach (CharacterState target in cardEffectParams.targets)
        {
            yield return coreGameManagers.GetCombatManager().ApplyDamageToTarget(
                damage,
                target,
                new CombatManager.ApplyDamageToTargetParameters
                {
                    playedCard = cardEffectParams.playedCard,
                    finalEffectInSequence = cardEffectParams.finalEffectInSequence,
                    appliedVfx = cardEffectState.GetAppliedVFX(),
                    appliedVfxId = cardEffectParams.appliedVfxId
                }
            );
        }
    }

    public override PropDescriptions CreateEditorInspectorDescriptions() => new()
    {
        [CardEffectFieldNames.ParamInt.GetFieldName()] =
            new PropDescription("Damage dealt per friendly unit on this floor.")
    };
}
