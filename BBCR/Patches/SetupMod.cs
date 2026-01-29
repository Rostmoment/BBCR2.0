using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BBCR.API;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BBCR.Patches
{
    [HarmonyPatch]
    class SetupMod
    {
        [HarmonyPatch(typeof(GameLoader), nameof(GameLoader.Initialize))]
        [HarmonyPostfix]
        private static void SetupFunSetting(int mode, GameLoader __instance)
        {
            VariablesStorage.styleIsEndless = mode != 0;
            CoreGameManager.Instance.mirrorMode = __instance.mirrorMode;
            CoreGameManager.Instance.lightsOut = __instance.lightsOut;
            CoreGameManager.Instance.hardMode = __instance.hardMode;

        }
        [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Start))]
        [HarmonyPostfix]
        private static void AddLoadingScreen(MainMenu __instance)
        {
            if (!LoadingAPI.Finished)
                __instance.gameObject.AddComponent<LoadingSceen>().mainMenu = __instance;
        }
        [HarmonyPatch(typeof(GameLoader), nameof(GameLoader.StartGame))]
        [HarmonyPrefix]
        private static void SetupStyle(GameLoader __instance)
        {
            switch (__instance.style)
            {
                case ClassicStyle.Classic:
                    VariablesStorage.CurrentStyle = Style.Classic;
                    break;
                case ClassicStyle.Party:
                    VariablesStorage.CurrentStyle = Style.Party;
                    break;
                case ClassicStyle.Demo:
                    VariablesStorage.CurrentStyle = Style.Demo;
                    break;
                case ClassicStyle.Null:
                    if (PlayerFileManager.Instance.flags[4]) 
                        VariablesStorage.CurrentStyle = Style.Glitch;
                    else 
                        VariablesStorage.CurrentStyle = Style.Null;
                    break;
                default:
                    break;
            }
        }
    }
}
