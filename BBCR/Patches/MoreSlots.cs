using System;
using System.Collections.Generic;
using System.Text;
using BBCR.ModdedContent;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace BBCR.Patches
{
    [HarmonyPatch]
    class MoreSlots
    {
        [HarmonyPatch(typeof(ItemManager), nameof(ItemManager.Update))]
        [HarmonyPostfix]
        private static void ChooseMoreSlots(ItemManager __instance)
        {
            if (Time.timeScale != 0f && ModdedOptionMenu.MoreSlotsEnabled)
            {
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    __instance.selectedItem = 3;
                    __instance.UpdateSelect();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    __instance.selectedItem = 4;
                    __instance.UpdateSelect();
                }
            }
        }
        [HarmonyPatch(typeof(ItemManager), nameof(ItemManager.Awake))]
        [HarmonyPrefix]
        private static void AddMoreSlots(ItemManager __instance)
        {
            if (!ModdedOptionMenu.MoreSlotsEnabled) 
                return;

            __instance.maxItem = 4;
            HudManager hud = CoreGameManager.Instance.GetHud(0);
            Transform baseTransform = hud.transform.Find("ItemSlots");
            baseTransform.Find("ItemsCover").GetComponent<RawImage>().texture = BasePlugin.assets.Get<Texture2D>("ItemSlot5");
            baseTransform.Find("ItemsCover").localPosition = new Vector3(-185f, -18f, 0f);
            float x = -283;
            baseTransform.Find("ItemSlot").localPosition = new Vector3(x, -18, 0);
            for (int i = 1; i<5; i++)
            {
                x += 40;    
                baseTransform.Find("ItemSlot (" + i.ToString() + ")").localPosition = new Vector3(x, -18, 0);
            }
        }
    }
}
