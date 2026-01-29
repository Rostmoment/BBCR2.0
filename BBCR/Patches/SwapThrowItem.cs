using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBCR.Patches
{
    [HarmonyPatch(typeof(NullProjectile))]
    class SwapThrowItem
    {
        [HarmonyPatch(nameof(NullProjectile.Update))]
        [HarmonyPrefix]
        private static void RebindKey(NullProjectile __instance)
        {
            if (__instance.held && !__instance.clickBuffer && Singleton<InputManager>.Instance.GetDigitalInput("UseItem", onDown: true) && ModdedOptionMenu.SpawnThrowItemConfigEnabled)
                __instance.Throw();
        }
        [HarmonyPatch(nameof(NullProjectile.Throw))]
        [HarmonyPrefix]
        private static bool DisableFalseThrow(NullProjectile __instance)
        {
            if (Singleton<InputManager>.Instance.GetDigitalInput("Interact", onDown: true) && ModdedOptionMenu.SpawnThrowItemConfigEnabled)
                return false;

            return true;
        }
    }
}
