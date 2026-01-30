using System;
using System.Collections.Generic;
using System.Text;
using BBCR.Patches.Styles;
using HarmonyLib;
using UnityEngine;

namespace BBCR.Patches.NPCPatches
{
    [HarmonyPatch(typeof(NullNPC))]
    class NullPatch
    {
        [HarmonyPatch(nameof(NullNPC.Update))]
        [HarmonyPostfix]
        private static void AlwaysWindow(NullNPC __instance)
        {
            if (!__instance.Navigator.passableObstacles.Contains(PassableObstacle.Window) && CoreGameManager.Instance.hardMode)
                __instance.Navigator.passableObstacles.Add(PassableObstacle.Window);
        }

        [HarmonyPatch(nameof(NullNPC.Hit))]
        [HarmonyPrefix]
        private static void OnNullHit(NullNPC __instance)
        {
            __instance.stunTime = 1f;
            if (CoreGameManager.Instance.hardMode)
            {
                __instance.GetAngry(0.05f);
                if (__instance.health > 1)
                    __instance.stunTime = 0f;
                else 
                    __instance.stunTime = 0.5f;
            }
        }

        [HarmonyPatch(nameof(NullNPC.StartBossIntro))]
        [HarmonyPrefix]
        private static bool CancelIntro(NullNPC __instance)
        {
            if (HardModeNullStyle.ElevatorsOpenedSecondTime)
            {
                __instance.audMan.QueueAudio(__instance.audHaha);
                return false;
            }
            return true;
        }
    }
}
