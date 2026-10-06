using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TrainworksReloaded.Core;
using TrainworksReloaded.Core.Extensions;

namespace Spire_Clan.Plugin
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger = new(MyPluginInfo.PLUGIN_GUID);
        
        // Plugin startup logic. This function is automatically called when your plugin initializes
        public void Awake()
        {
            Logger = base.Logger;

            var builder = Railhead.GetBuilder();
            builder.Configure(
                MyPluginInfo.PLUGIN_GUID,
                c =>
                {
                    // Be sure to include any new json files if you add more.
                    c.AddMergedJsonFile(
                        "json/global.json",
                        // Clan Definition
                        "json/clan/clan.json",
                        // Required: Clan Map Banner Node definition and Card Pool
                        "json/clan/clan_banner.json",
                        // Optional: Custom Card Border (if removed edit clan.json)
                        "json/clan/clan_card_frame.json",
                        // Optional: Custom Clan Subtypes used by units.
                        "json/clan/clan_subtypes.json",
                        // Required: A Clan Champion with one Upgrade Path.
                        "json/champions/basic_champion.json",
                        // Spell Cards
                        // Starter (required).
                        "json/spells/basic_starter.json",
                        "json/spells/basic_common.json",
                        "json/spells/spire_railspike.json",
                        "json/spells/scaling_tome.json",
                        "json/spells/echo.json",
                        "json/spells/book_of_stabbing.json",
                        "json/spells/gas_bomb.json",
                        "json/spells/relax.json",
                        "json/spells/knowledge_overwhelming.json",
                        "json/spells/war_chant.json",
                        "json/spells/pulsate.json",
                        "json/spells/gremlin_gang.json",
                        "json/spells/gremlin_ambush.json",
                        "json/spells/call_for_backup.json",
                        "json/spells/metallicize.json",
                        "json/spells/body_block_minion_sacrifice.json",
                        // Units
                        "json/units/basic_ability_unit.json",
                        "json/units/basic_banner_unit.json",
                        "json/units/awakened_one.json",
                        "json/units/the_champ.json",
                        "json/units/hexaghost.json",
                        "json/units/collector.json",
                        "json/units/entomancer.json",
                        "json/units/bronze_automaton.json",
                        "json/units/time_eater.json",
                        "json/units/slime_boss.json",
                        "json/units/gremlin_leader.json",
                        // Non Banner Unit, that is draftable as a Battle Reward.
                        "json/units/basic_draft_unit.json",
                        "json/units/scroll_of_biting.json",
                        "json/units/cultist.json",
                        "json/units/torch_head.json",
                        "json/units/twig_slime.json",
                        "json/units/fat_gremlin.json",
                        "json/units/mad_gremlin.json",
                        "json/units/shield_gremlin.json",
                        "json/units/sneaky_gremlin.json",
                        "json/units/gremlin_wizard.json",
                        // Equipment
                        "json/equipment/basic_equipment.json",
                        // Rooms
                        "json/rooms/basic_room.json",
                        // Shop Upgrades (Enhancer)
                        "json/enhancers/basic_enhancer.json",
                        // Artifacts
                        "json/relics/basic_artifact.json"
                    );
                }
            );

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            // Uncomment if you need Harmony Patch support.
            //var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            //harmony.PatchAll();

        }
    }
}
