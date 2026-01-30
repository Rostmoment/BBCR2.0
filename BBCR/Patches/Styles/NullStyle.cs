using BBCR.API.Extensions;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace BBCR.Patches.Styles
{
    [HarmonyPatch(typeof(ClassicNullManager))]
    class NullStyle
    {
        [HarmonyPatch("Initialize")]
        [HarmonyPostfix]
        private static void InitializePostfix(ClassicNullManager __instance)
        {
            MirrorMode mirrorMode = __instance.gameObject.GetOrAddComponent<MirrorMode>();
            LanternMode lanternMode = __instance.gameObject.GetOrAddComponent<LanternMode>();

            Color lightsOutColor = new Color(0.887f, 0.765f, 0.498f, 1f);
            float strenght = 6f;
            if (!CoreGameManager.Instance.lightsOut)
            {
                strenght = 69f;
                lightsOutColor = new Color(0.45f, 0.45f, 0.45f, 1f);
            }
            lanternMode.Initialize(__instance.ec);
            lanternMode.AddSource(CoreGameManager.Instance.GetPlayer(0).transform, strenght, lightsOutColor);


            if (CoreGameManager.Instance.mirrorMode) 
                mirrorMode.Initialize();
        }
        

    }
}
