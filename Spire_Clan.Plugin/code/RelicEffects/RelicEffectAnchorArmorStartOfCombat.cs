using System.Collections;
using TrainworksReloaded.Base;

namespace Spire_Clan.Plugin.Code.RelicEffects;

public sealed class RelicEffectAnchorArmorStartOfCombat : RelicEffectBase, ITurnPhaseStartOfBattleRelicEffect
{
    private StatusEffectStackData[] statusEffects = [];

    public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
    {
        base.Initialize(relicState, relicData, relicEffectData);
        statusEffects = relicEffectData.GetParamStatusEffects();
        if (statusEffects.Length == 0)
        {
            throw new InvalidOperationException("Anchor's start-of-combat relic effect requires at least one status effect.");
        }
    }

    public override PropDescriptions CreateEditorInspectorDescriptions() => new();

    public bool TestEffectStartOfBattle(RelicEffectParams relicEffectParams, ICoreGameManagers coreGameManagers)
    {
        RoomManager roomManager = coreGameManagers.GetRoomManager();
        for (int roomIndex = 0; roomIndex < roomManager.GetNumRooms(); roomIndex++)
        {
            RoomState? room = roomManager.GetRoom(roomIndex);
            if (room?.GetFirstFilledMonsterPoint()?.GetCharacterState() != null)
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerator ApplyEffectStartOfBattle(RelicEffectParams relicEffectParams, ICoreGameManagers coreGameManagers)
    {
        RoomManager roomManager = coreGameManagers.GetRoomManager();
        RelicManager relicManager = coreGameManagers.GetRelicManager();

        for (int roomIndex = 0; roomIndex < roomManager.GetNumRooms(); roomIndex++)
        {
            RoomState? room = roomManager.GetRoom(roomIndex);
            CharacterState? character = room?.GetFirstFilledMonsterPoint()?.GetCharacterState();
            if (character == null)
            {
                continue;
            }

            foreach (StatusEffectStackData statusEffect in statusEffects)
            {
                character.AddStatusEffect(statusEffect.statusId, statusEffect.count, null, true, false);
            }

            NotifyRelicTriggered(relicManager, character);
            yield return null;
        }
    }
}
