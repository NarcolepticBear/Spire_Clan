public sealed class CardTraitScalingUpgradeUnitAttackNumUnitsOnFloor : CardTraitState
    {
        public override PropDescriptions CreateEditorInspectorDescriptions()
        {
            return new PropDescriptions
            {
                [CardTraitFieldNames.ParamInt.GetFieldName()] = new PropDescription("(Required) Attack multiplier applied to the TrackedValue and added BonusDamage of the CardUpgrade."),
                [CardTraitFieldNames.ParamCardUpgradeData.GetFieldName()] = new PropDescription("(Required) Restrict scaling upgrades to this one provided, all other upgrades that are not this one provided will not be scaled."),
            };
        }

        public override void OnApplyingCardUpgradeToUnit(CardState thisCard, CharacterState targetUnit, CharacterTriggerState? characterTriggerState, CardUpgradeState upgradeState, ICoreGameManagers coreGameManagers)
        {
            if (GetCardUpgradeDataParam() != upgradeState.GetSourceCardUpgradeData())
            {
                return;
            }
            int attackDamage = upgradeState.GetAttackDamage();
            int additionalDamage = GetParamInt() * GetNumUnitsOnFloor(coreGameManagers.GetRoomManager(), targetUnit.GetCurrentRoomIndex(), targetUnit.GetTeamType());
            upgradeState.SetAttackDamage(attackDamage + additionalDamage);
        }

        private int GetNumUnitsOnFloor(RoomManager roomManager, int roomIndex, Team.Type onTeam)
        {
            RoomState? room = roomManager.GetRoom(roomIndex);
            if (room == null) return 0;
            return room.GetNumCharacters(onTeam);
        }
    }