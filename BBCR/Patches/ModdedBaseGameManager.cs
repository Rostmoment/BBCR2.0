using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBCR.API;
using BBCR.ModdedContent;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace BBCR.Patches
{
    
    [HarmonyPatch(typeof(BaseGameManager))]
    class AddModdedBaseGameManager
    {
        private static ModdedBaseGameManager mbgm;

        [HarmonyPatch(nameof(BaseGameManager.ElevatorClosed))]
        [HarmonyPostfix]
        private static void Animation(Elevator elevator)
        {
            mbgm.PlayAnimation();
        }
        [HarmonyPatch(nameof(BaseGameManager.Initialize))]
        [HarmonyPostfix]
        private static void Add(BaseGameManager __instance)
        {
            mbgm = __instance.gameObject.AddComponent<ModdedBaseGameManager>();
            mbgm.Initialize(__instance.ec, __instance);
        }
        [HarmonyPatch(nameof(BaseGameManager.CollectNotebooks))]
        [HarmonyPostfix]
        private static void Animation(int count)
        {
            if (count > 0)
            {
                CoreGameManager.Instance.audMan.PlaySingle(BasePlugin.assets.Get<SoundObject>("NotebookCollect"));
                mbgm.PlayAnimation();
            }
        }
    }
}
