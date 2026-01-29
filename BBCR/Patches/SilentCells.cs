using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BBCR.Patches
{
    [HarmonyPatch(typeof(EnvironmentController))]
    public static class SilentCells
    {
        private static HashSet<TileController> silentCells = new HashSet<TileController>();
        
        public static void MakeSilent(this TileController tile, bool silent)
        {
            if (silent)
                silentCells.Add(tile);
            else
                silentCells.Remove(tile);
        }


        [HarmonyPatch(nameof(EnvironmentController.MakeNoise))]
        [HarmonyPrefix]
        private static bool CancelNoise(Vector3 position, EnvironmentController __instance)
        {
            TileController tile = __instance.TileFromPos(position);
            return !silentCells.Contains(tile);
        }
    }
}
