using System;
using System.Collections.Generic;
using System.Text;
using BBCR.Patches.Styles;
using HarmonyLib;
using UnityEngine;

namespace BBCR.Patches.NPC
{
    [HarmonyPatch(typeof(NullNPC))]
    class NullPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void AlwaysWindow(NullNPC __instance)
        {
            if (!__instance.Navigator.passableObstacles.Contains(PassableObstacle.Window) && CoreGameManager.Instance.hardMode)
                __instance.Navigator.passableObstacles.Add(PassableObstacle.Window);
        }

        [HarmonyPatch("Hit")]
        [HarmonyPrefix]
        private static void OnNullHit(NullNPC __instance)
        {
            __instance.stunTime = 1f;
            if (CoreGameManager.Instance.hardMode && __instance.health > 1)
                __instance.stunTime = 0f;

            else if (CoreGameManager.Instance.hardMode && __instance.health == 1)
                __instance.stunTime = 0.5f;
        }
        [HarmonyPatch("OnTriggerEnter")]
        [HarmonyPrefix]
        static bool OnTriggerEnterPrefix(Collider other)
        {
            if (!NullStyle.bossActive && CoreGameManager.Instance.freeRun && other.tag == "Player")
                return false;
            
            return true;
        }

    }
}
