using System;
using System.Collections.Generic;
using System.Text;
using BBCR.API;
using HarmonyLib;
using UnityEngine;

namespace BBCR.Patches
{
    [HarmonyPatch(typeof(RoomController))]
    class CustomPrincipalDoor
    {
        [HarmonyPatch(nameof(RoomController.Start))]
        [HarmonyPostfix]
        private static void ReplaceDoor(RoomController __instance)
        {
            if (__instance.category == RoomCategory.Office)
                __instance.doorMats = ObjectsCreator.CreateDoor("OfficeDoor", BasePlugin.assets.Get<Texture2D>("OfficeOpen"), BasePlugin.assets.Get<Texture2D>("OfficeClosed"));
        }
    }
}
