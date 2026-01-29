using BBCR.API.Extensions;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace BBCR.Patches.Styles
{
    [HarmonyPatch(typeof(ClassicNullManager))]
    class NullStyle
    {
        public static bool bossActive = false;
        public static bool elevatorsOpenedSecondTime = false;
        public static void OpenExit(ClassicNullManager classicNullManager)
        {
            classicNullManager.elevatorsToClose = 0;
            classicNullManager.freezeAllElevators = false;
            classicNullManager.elevatorsNullified = false;
        }
        public static IEnumerator RageNull(NullNPC nullNPC)
        {
            WaitForSeconds delay = new WaitForSeconds(5f);
            while (true)
            {
                nullNPC.GetAngry(1);
                yield return delay;
            }
        }
        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static void UpdateFixer(ClassicNullManager __instance)
        {
            if ((__instance.finalElevator != null && __instance.finalElevatorTargetTile != null) && ((!CoreGameManager.Instance.hardMode) || (CoreGameManager.Instance.hardMode && elevatorsOpenedSecondTime)))
            {
                NullStyle.bossActive = __instance.bossActive;
                NullNPC nullNpc = (NullNPC)__instance.ec.GetBaldi();
                if (nullNpc.transform.position.x == __instance.finalElevatorTargetTile.transform.position.x && nullNpc.transform.position.z == __instance.finalElevatorTargetTile.transform.position.z && __instance.finalElevator.IsOpen)
                {
                    __instance.finalElevator.Close();
                    __instance.elevatorsClosed += 1;
                    __instance.ElevatorClosed(__instance.finalElevator);

                    if (elevatorsOpenedSecondTime)
                        nullNpc.SpeechCheck(NullPhrase.Haha, 100f);
                    
                    else
                    {
                        __instance.bossActive = true;
                        nullNpc.StartBossIntro();
                    }
                }
            }
        }
        [HarmonyPatch("NullHit")]
        [HarmonyPrefix]
        private static void OnNullHit(ClassicNullManager __instance)
        {
            if (CoreGameManager.Instance.hardMode)
                __instance.nullNpc.GetAngry(0.05f);

            if (__instance.health == 1 && (CoreGameManager.Instance.freeRun || CoreGameManager.Instance.hardMode))
            {
                if (!elevatorsOpenedSecondTime && CoreGameManager.Instance.hardMode && !CoreGameManager.Instance.freeRun)
                {
                    __instance.allNotebooksFound = false;
                    elevatorsOpenedSecondTime = true;
                    __instance.elevatorsToClose = __instance.ec.elevators.Count - 1;
                    __instance.elevatorsClosed = 0;
                    __instance.freezeAllElevators = false;
                    __instance.forceCloseAllElevators = false;
                    __instance.elevatorsNullified = false;
                    __instance.bossActive = true;
                    __instance.nullNpc.controlOverride = false;
                    __instance.ec.standardDarkLevel = new Color(0.2f, 0f, 0f);
                    __instance.ec.FlickerLights(true);
                    __instance.gameObject.DeleteComponent<LanternMode>();
                    LanternMode lanternMode = __instance.gameObject.AddComponent<LanternMode>();
                    lanternMode.Initialize(__instance.ec);
                    lanternMode.AddSource(CoreGameManager.Instance.GetPlayer(0).transform, 12f, Color.red);
                    __instance.AllNotebooks();
                    __instance.StartCoroutine(RageNull(__instance.nullNpc));
                    __instance.finalElevator = null;
                    __instance.finalElevatorTargetTile = null;
                    __instance.ec.audMan.PlaySingle(AssetsAPI.LoadAsset<SoundObject>(x => x.additionalKeys.Where(x => x.key == "Vfx_Null_ClassicSpeech4") != null));
                }
                __instance.SpawnProjectile();
                __instance.health += 1;
            }
        }
        [HarmonyPatch("Initialize")]
        [HarmonyPostfix]
        private static void InitializePostfix(ClassicNullManager __instance)
        {
            elevatorsOpenedSecondTime = false;
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

            if (!CoreGameManager.Instance.hardMode) 
                __instance.elevatorsNullified = false; 

        }
        

        [HarmonyPatch("ElevatorClosed")]
        [HarmonyPostfix]
        private static void AllowGoToExitAndFixBug(ClassicNullManager __instance)
        {
            if (!CoreGameManager.Instance.freeRun && __instance.elevatorsClosed == 3 && (!CoreGameManager.Instance.hardMode ^ elevatorsOpenedSecondTime)) 
                OpenExit(__instance);
        }
    }
}
