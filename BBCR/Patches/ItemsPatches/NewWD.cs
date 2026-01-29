using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BBCR.Patches.ItemsPatches
{
    [HarmonyPatch(typeof(ITM_NoSquee))]
    class NewWD
    {
        [HarmonyPatch("Use")]
        [HarmonyPrefix]
        private static bool NewEffect(ITM_NoSquee __instance, PlayerManager pm, ref bool __result)
        {
            if (!ModdedOptionMenu.NewWDEnabled)
                return true;

            __result = true;
            CoreGameManager.Instance.audMan.PlaySingle(__instance.sound);
            __instance.StartCoroutine(Coroutine(GetSquare5x5(pm.ec.TileFromPos(pm.transform.position)), __instance));
            return false;

        }
        private static IEnumerator Coroutine(List<TileController> tiles, ITM_NoSquee __instance)
        {
            foreach (TileController tile in tiles)
                tile.MakeSilent(true);
            yield return new WaitForSeconds(300);
            foreach (TileController tile in tiles)
                tile.MakeSilent(false);

            GameObject.Destroy(__instance.gameObject);
        }
        private static List<TileController> GetSquare5x5(TileController centerTile)
        {
            List<TileController> result = new List<TileController>();
            Vector3 centerPos = centerTile.transform.position;

            for (int x = -2; x <= 2; x++)
            {
                for (int z = -2; z <= 2; z++)
                {
                    Vector3 pos = centerPos + new Vector3(x * 10, 0f, z * 10);

                    TileController tile = BaseGameManager.Instance.ec.TileFromPos(pos);
                    if (tile != null) 
                        result.Add(tile);
                }
            }
            return result;
        }

    }
}
