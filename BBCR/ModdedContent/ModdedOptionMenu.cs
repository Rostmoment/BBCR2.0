using BBCR.API;
using BepInEx.Configuration;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace BBCR.ModdedContent
{
    static class ModdedOptionMenu
    {
        public static bool PartyEventEnabled => partyEventConfig.Value;
        public static bool MoreSlotsEnabled => moreSlotsConfig.Value;
        public static bool MrsPompEnabled => mrsPompConfig.Value;
        public static bool BlueLockersEnabled => blueLockersConfig.Value;
        public static bool WaterFountainsEnabled => waterFountainsConfig.Value;
        public static bool NewWDEnabled => newWDConfig.Value;
        public static bool SpawnThrowItemConfigEnabled => swapThrowItemKeyConfig.Value;

        private static MenuToggle partyEvent;        private static ConfigEntry<bool> partyEventConfig;
        private static MenuToggle moreSlots;         private static ConfigEntry<bool> moreSlotsConfig;
        private static MenuToggle mrsPomp;           private static ConfigEntry<bool> mrsPompConfig;
        private static MenuToggle blueLockers;       private static ConfigEntry<bool> blueLockersConfig;
        private static MenuToggle waterFountains;    private static ConfigEntry<bool> waterFountainsConfig;
        private static MenuToggle newWD;             private static ConfigEntry<bool> newWDConfig;
        private static MenuToggle swapThrowItemKey;  private static ConfigEntry<bool> swapThrowItemKeyConfig;

        public static void InitializeGlobal()
        {
            partyEventConfig = BasePlugin.Instance.Config.Bind("Options", "PartyEvent", false, "Enable to have Party Event in Demo style");
            moreSlotsConfig = BasePlugin.Instance.Config.Bind("Options", "MoreSlots", false, "Enable to have 5 item slots instead of 3");
            mrsPompConfig = BasePlugin.Instance.Config.Bind("Options", "MrsPomp", false, "Enable to have Mrs. Pomp in Demo style");
            blueLockersConfig = BasePlugin.Instance.Config.Bind("Options", "BlueLockers", false, "Enable to have blue lockers");
            waterFountainsConfig = BasePlugin.Instance.Config.Bind("Options", "WaterFountains", false, "Enable to have water fountains");
            newWDConfig = BasePlugin.Instance.Config.Bind("Options", "New WD", false, "Enable to make WD work like in newest BBPlus versions");
            swapThrowItemKeyConfig = BasePlugin.Instance.Config.Bind("Options", "SwapThrowItemKey", false, "Enable to swap throw item key");
        }
        public static void Initialize(OptionsMenu menu)
        {
            OptionsCategory category = OptionsAPI.CreateCategory(menu, "Modded");
            partyEvent = category.CreateMenuToggle("Party Event", "If enabled, Party Even will be in Demo style", new Vector2(30, 55), partyEventConfig.Value);
            moreSlots = category.CreateMenuToggle("More Slots", "If enabled, you will have 5 slots instead of 3", new Vector2(30, 25), moreSlotsConfig.Value);
            mrsPomp = category.CreateMenuToggle("Mrs. Pomp", "If enabled, Mrs. Pomp will spawn in Demo style", new Vector2(30, -5), mrsPompConfig.Value);
            blueLockers = category.CreateMenuToggle("Blue Lockers", "If enabled, blue lockers will spawn", new Vector2(30, -35), blueLockersConfig.Value);
            waterFountains = category.CreateMenuToggle("Water Fountains", "If enabled, water fountains will spawn", new Vector2(30, -65), waterFountainsConfig.Value);
            newWD = category.CreateMenuToggle("New WD", "If enabled, WD will work like in newest BB+ version", new Vector2(30, -95), newWDConfig.Value);
            swapThrowItemKey = category.CreateMenuToggle("Swap Throw Item Key", "If enabled, throw item key will be use item, instead of interact key", new Vector2(30, -125), swapThrowItemKeyConfig.Value);

        }

        public static void Save(OptionsMenu menu)
        {
            partyEventConfig.Value = partyEvent.Value;
            moreSlotsConfig.Value = moreSlots.Value;
            mrsPompConfig.Value = mrsPomp.Value;
            blueLockersConfig.Value = blueLockers.Value;
            waterFountainsConfig.Value = waterFountains.Value;
            newWDConfig.Value = newWD.Value;
            swapThrowItemKeyConfig.Value = swapThrowItemKey.Value;
        }
    }
}
