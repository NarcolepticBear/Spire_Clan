using System.Collections;

namespace Spire_Clan.Plugin.Code.CardEffects;

public sealed class CardEffectAdjustCapacityOnAllFloors : CardEffectBase
{
    public override bool TestEffect(
        CardEffectState cardEffectState,
        CardEffectParams cardEffectParams,
        ICoreGameManagers coreGameManagers)
    {
        return coreGameManagers.GetRoomManager() != null;
    }

    public override IEnumerator ApplyEffect(
        CardEffectState cardEffectState,
        CardEffectParams cardEffectParams,
        ICoreGameManagers coreGameManagers,
        ISystemManagers sysManagers)
    {
        RoomManager roomManager = coreGameManagers.GetRoomManager();
        Team.Type team = cardEffectState.GetTargetTeamType();
        int amount = cardEffectState.GetParamInt();

        for (int roomIndex = 0; roomIndex < roomManager.GetNumRooms(); roomIndex++)
        {
            RoomState? room = roomManager.GetRoom(roomIndex);
            if (room == null)
            {
                continue;
            }

            yield return room.AdjustCapacity(team, amount, showPipsEffect: true);
        }
    }

    public override PropDescriptions CreateEditorInspectorDescriptions() => new()
    {
        [CardEffectFieldNames.ParamInt.GetFieldName()] =
            new PropDescription("Capacity added to each floor.")
    };
}
