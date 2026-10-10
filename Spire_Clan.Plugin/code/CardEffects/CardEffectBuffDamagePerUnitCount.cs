using System.Collections;
using TrainworksReloaded.Base;

namespace Spire_Clan.Plugin.Code.CardEffects;

public sealed class CardEffectBuffDamagePerUnitCount : CardEffectBase
{
    private static CardStatistics.TrackedValueType unitsInTargetRoomTrackedValue;
    private static CardStatistics.TrackedValueType unitsOnTrainTrackedValue;
    private static bool trackedValuesInitialized;

    internal static void ConfigureTrackedValues(GameDataManager gameDataManager)
    {
        unitsInTargetRoomTrackedValue = gameDataManager.GetTrackedValueTypeEnum(
            "NumUnitsInTargetRoom",
            Conductor.MyPluginInfo.PLUGIN_GUID);
        unitsOnTrainTrackedValue = gameDataManager.GetTrackedValueTypeEnum(
            "NumUnitsOnTrain",
            Conductor.MyPluginInfo.PLUGIN_GUID);
        trackedValuesInitialized = true;
    }

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
        List<CharacterState> allTargets = [.. cardEffectParams.targets];
        CardEffectBuffDamage buffDamageEffect = new();
        buffDamageEffect.Setup(cardEffectState);

        if (cardEffectState.GetParamBool())
        {
            int totalUnitCount = GetTrackedUnitCount(coreGameManagers, unitsOnTrainTrackedValue);
            yield return ApplyRepeatedBuff(
                buffDamageEffect,
                cardEffectState,
                cardEffectParams,
                coreGameManagers,
                sysManagers,
                allTargets,
                totalUnitCount);
            yield break;
        }

        int targetRoomUnitCount = GetTrackedUnitCount(coreGameManagers, unitsInTargetRoomTrackedValue);
        RoomManager roomManager = coreGameManagers.GetRoomManager();
        foreach (IGrouping<int, CharacterState> targetFloor in allTargets.GroupBy(target => target.GetCurrentRoomIndex()))
        {
            RoomState? room = roomManager.GetRoom(targetFloor.Key);
            if (room == null)
            {
                continue;
            }

            yield return ApplyRepeatedBuff(
                buffDamageEffect,
                cardEffectState,
                cardEffectParams,
                coreGameManagers,
                sysManagers,
                targetFloor.ToList(),
                targetRoomUnitCount);
        }
    }

    private static int GetTrackedUnitCount(
        ICoreGameManagers coreGameManagers,
        CardStatistics.TrackedValueType trackedValue)
    {
        if (!trackedValuesInitialized)
        {
            throw new InvalidOperationException("Conductor tracked values have not been configured.");
        }

        return coreGameManagers.GetCardStatistics().GetStatValue(new CardStatistics.StatValueData
        {
            trackedValue = trackedValue,
            paramTeamType = Team.Type.Monsters
        });
    }

    private static IEnumerator ApplyRepeatedBuff(
        CardEffectBuffDamage buffDamageEffect,
        CardEffectState cardEffectState,
        CardEffectParams cardEffectParams,
        ICoreGameManagers coreGameManagers,
        ISystemManagers sysManagers,
        List<CharacterState> targets,
        int count)
    {
        List<CharacterState> originalTargets = [.. cardEffectParams.targets];
        try
        {
            cardEffectParams.targets.Clear();
            cardEffectParams.targets.AddRange(targets);

            for (int i = 0; i < count; i++)
            {
                IEnumerator applyEffect = buffDamageEffect.ApplyEffect(
                    cardEffectState,
                    cardEffectParams,
                    coreGameManagers,
                    sysManagers);
                while (applyEffect.MoveNext())
                {
                    yield return applyEffect.Current;
                }
            }
        }
        finally
        {
            cardEffectParams.targets.Clear();
            cardEffectParams.targets.AddRange(originalTargets);
        }
    }

    public override PropDescriptions CreateEditorInspectorDescriptions() => new()
    {
        [CardEffectFieldNames.ParamBool.GetFieldName()] =
            new PropDescription("True uses Conductor's train-wide friendly unit count; false uses its selected-room count for each target group."),
        [CardEffectFieldNames.ParamInt.GetFieldName()] =
            new PropDescription("Attack gained per counted friendly unit.")
    };
}
