using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
namespace BBCR.Patches.ItemsPatches
{
    [HarmonyPatch(typeof(ITM_ZestyBar))]
    class ZestyBarSound
    {
        [HarmonyPatch(nameof(ITM_ZestyBar.Use))]
        [HarmonyPrefix]
        private static void AddSound() => CoreGameManager.Instance.audMan.PlaySingle(BasePlugin.assets.Get<SoundObject>("ZestyBarEat"));
    }
}
