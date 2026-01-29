using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BBCR.Patches.Styles
{
    [HarmonyPatch(typeof(ClassicNullManager))]
    class EndlessNullStyle
    {

        [HarmonyPatch(nameof(ClassicNullManager.CollectNotebook))]
        [HarmonyPrefix]
        private static void RespawnNotebookInEndless(ClassicNullManager __instance, Notebook notebook)
        {
            if (VariablesStorage.styleIsEndless)
            {
                __instance.StartCoroutine(ResetNotebook(notebook, 60));
                __instance.AngerBaldi(0.25f);
            }
        }

        [HarmonyPatch(nameof(ClassicNullManager.AllNotebooks))]
        [HarmonyPrefix]
        private static bool DontAllNotebooks() => !VariablesStorage.styleIsEndless;

        private static IEnumerator ResetNotebook(Notebook notebook, float time)
        {
            yield return new WaitForSeconds(time);
            notebook.Hide(false);
            notebook.activity.ReInit();
            notebook.activity.audMan.PlaySingle(notebook.activity.audRespawn);
        }
    }
}
