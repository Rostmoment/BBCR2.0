using BBCR.API.Extensions;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BBCR.Patches.Styles
{
    [HarmonyPatch(typeof(ClassicNullManager))]
    class HardModeNullStyle
    {
        private static bool _elevatorOpenedSecondTime;
        public static bool ElevatorsOpenedSecondTime
        {
            set => _elevatorOpenedSecondTime = value;
            get
            {
                if (BaseGameManager.Instance == null)
                    return false;
                if (BaseGameManager.Instance is ClassicNullManager)
                    return _elevatorOpenedSecondTime;
                return false;
            }
        }


        private static IEnumerator RageNull(NullNPC nullNPC)
        {
            WaitForSeconds delay = new WaitForSeconds(5f);
            while (true)
            {
                nullNPC.GetAngry(1);
                yield return delay;
            }
        }
        [HarmonyPatch(nameof(ClassicNullManager.Initialize))]
        [HarmonyPrefix]
        private static void Initialize()
        {
            ElevatorsOpenedSecondTime = false;
        }

        [HarmonyPatch(nameof(ClassicNullManager.NullHit))]
        [HarmonyPrefix]
        private static void OpenElevators(ClassicNullManager __instance)
        {
            if (!CoreGameManager.Instance.hardMode || __instance.health != 1)
                return;

            __instance.SpawnProjectile();
            __instance.health += 1;

            if (ElevatorsOpenedSecondTime)
                return;

            ElevatorsOpenedSecondTime = true;

            __instance.allNotebooksFound = false;
            __instance.elevatorsToClose = __instance.ec.elevators.Count - 1;
            __instance.elevatorsClosed = 0;

            __instance.freezeAllElevators = false;
            __instance.forceCloseAllElevators = false;
            __instance.elevatorsNullified = false;

            __instance.finalElevator = null;
            __instance.finalElevatorTargetTile = null;

            __instance.allNotebooksFound = false;
            __instance.AllNotebooks();

            CoreGameManager.Instance.audMan.PlaySingle(BasePlugin.assets.Get<SoundObject>("GetOutWhileStillCan"));

            foreach (NPC npc in __instance.ec.Npcs)
            {
                if (npc is NullNPC nullNPC)
                    __instance.StartCoroutine(RageNull(nullNPC));
            }
             // Easiest way to do it
            __instance.gameObject.DeleteComponent<LanternMode>();
            LanternMode lanternMode = __instance.gameObject.AddComponent<LanternMode>();
            lanternMode.Initialize(__instance.ec);
            lanternMode.AddSource(CoreGameManager.Instance.GetPlayer(0).transform, 12f, Color.red);
        }


        [HarmonyPatch(nameof(ClassicNullManager.ElevatorClosed))]
        [HarmonyPrefix]
        private static bool ElevatorClosedPatch(ClassicNullManager __instance, Elevator elevator)
        {
            List<Elevator> list = new List<Elevator>();
            list.AddRange(__instance.ec.elevators);
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].IsOpen)
                {
                    list.RemoveAt(i);
                    i--;
                }
            }

            if (list.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, list.Count);
                __instance.ec.MakeNoise(list[index].transform.position + list[index].dir.ToVector3() * 10f, 31);
            }

            if (__instance.elevatorsClosed == 1 || __instance.elevatorsClosed == 2)
                return false;



            if (__instance.elevatorsClosed == 3)
            {
                __instance.elevatorsNullified = false;
                int index = UnityEngine.Random.Range(0, list.Count);
                __instance.nullNpc.controlOverride = true;
                __instance.nullNpc.SetSlideMode(val: true);
                __instance.nullNpc.GetAngry(100f);
                __instance.finalElevator = list[index];
                __instance.nullNpc.Navigator.passableObstacles.Clear();

                __instance.nullNpc.Navigator.FindPathAvoid(new Vector3(195f, 0f, 215f), Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position);
                if (__instance.nullNpc.Navigator.NewListOfDestinationPoints().Count < 5)
                    __instance.nullNpc.Navigator.FindPath(__instance.nullNpc.transform.position, new Vector3(195f, 0f, 215f));

                __instance.nullNpc.HideSprite(true);
                __instance.finalElevatorTargetTile = __instance.ec.TileFromPos(list[index].transform.position + list[index].dir.ToVector3() * 10f);
                __instance.prebossActive = true;

                if (CoreGameManager.Instance.hardMode && !ElevatorsOpenedSecondTime)
                {
                    __instance.elevatorsNullified = true;
                    __instance.freezeAllElevators = true;
                    __instance.elevatorsToClose++;
                }
            }
            else if (__instance.elevatorsClosed == 4)
            {
                __instance.nullNpc.SetSlideMode(true);

                if (!ElevatorsOpenedSecondTime)
                {
                    __instance.nullNpc.Pause(999999f);
                    __instance.SpawnInitialProjectiles();
                    Singleton<MusicManager>.Instance.PlayMidi("BossIntro", loop: true);
                    Singleton<MusicManager>.Instance.QueueMidi("BossLoop", emptyQueue: true);
                    Singleton<MusicManager>.Instance.SetSpeed(__instance.initMusicSpeed);
                    Singleton<CoreGameManager>.Instance.GetPlayer(0).itm.disabled = true;
                    Singleton<CoreGameManager>.Instance.GetHud(0).Hide(val: true);
                    if (!Singleton<PlayerFileManager>.Instance.bossSeen)
                    {
                        Singleton<PlayerFileManager>.Instance.bossSeen = true;
                        Singleton<PlayerFileManager>.Instance.Save();
                    }
                }
            }

            return false;
        }

        [HarmonyPatch(nameof(ClassicNullManager.Update))]
        [HarmonyPrefix]
        private static void CloseElevator(ClassicNullManager __instance)
        {
            bool? b = !__instance?.nullNpc?.Navigator?.HasDestination;
            if (b.FalseIfNull() && ElevatorsOpenedSecondTime)
                __instance.elevatorsToClose++;
        }
    }
}
