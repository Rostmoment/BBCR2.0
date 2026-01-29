using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using BBCR.Patches.Styles;
using HarmonyLib;
using UnityEngine;

namespace BBCR.Patches
{
    [HarmonyPatch(typeof(CoreGameManager))]
    class NullNotCrashGame
    {
        public static void QuitToMenu() => CoreGameManager.Instance.ReturnToMenu();
        
        [HarmonyPatch(nameof(CoreGameManager.EndSequence), MethodType.Enumerator)]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> ReplaceCrashWithQuiting(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);
            
            const int targetIndex = 191;
            
            if (instructionList.Count > targetIndex)
            {
                instructionList[targetIndex] = new CodeInstruction(
                    OpCodes.Call, 
                    AccessTools.Method(typeof(NullNotCrashGame), nameof(QuitToMenu))
                );
            }
            
            return instructionList;
        }
        
        [HarmonyPatch(nameof(CoreGameManager.HoverQuit))]
        [HarmonyPrefix]
        private static bool CancelHoverQuit() => false;
        
        [HarmonyPatch(nameof(CoreGameManager.Quit))]
        [HarmonyPrefix]
        private static bool ReplaceQuitWithCustom(CoreGameManager __instance)
        {
            Singleton<GlobalCam>.Instance.SetListener(true);
            Singleton<SubtitleManager>.Instance.DestroyAll();
            __instance.ReturnToMenu();
            return false;
        }
    }
}