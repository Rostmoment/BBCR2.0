using System;
using System.Collections.Generic;
using System.Text;
using BBCR.API;
using HarmonyLib;
namespace BBCR.Patches
{
    [HarmonyPatch]
    class FixingBugs
    {
        [HarmonyPatch(typeof(YCTP), "OnDisable")]
        [HarmonyPostfix]
        private static void FixYCTPHardMode(YCTP __instance)
        {
            if (VariablesStorage.CurrentStyle.IsNullOrGlitch()) 
                CoreGameManager.Instance.GetPlayer(0).PlayerTimeScale = __instance.originalTimeScale;
        }

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Hide))]
        [HarmonyPrefix]
        private static bool FixHudHide() => !PlayerFileManager.Instance.authenticMode;

        [HarmonyPatch(typeof(EnvironmentController), nameof(EnvironmentController.StartEventTimers))]
        [HarmonyPrefix]
        private static void AlwaysRandomizeEvents(EnvironmentController __instance) => __instance.RandomizeEvents(__instance.EventsCount, 30f, 30f, 180f, new System.Random());
    }
}
