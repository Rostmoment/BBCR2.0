using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace BBCR.API.Extensions
{
    public static class EnumsExtensions
    {
        public static ItemObject Get(this Items item)
        {
            return Resources.FindObjectsOfTypeAll<ItemObject>().Where(x => x.itemType == item).First();
        }
        public static bool IsNullOrGlitch(this Style style) => style == Style.Null || style == Style.Glitch;

        public static RandomEvent Get(this RandomEventType eventType)
        {
            return (from x in Resources.FindObjectsOfTypeAll<RandomEvent>()
                    where x.Type == eventType
                    select x).First();
        }
        public static NPC Get(this Character character)
        {
            return (from x in Resources.FindObjectsOfTypeAll<NPC>()
                    where x.Character == character
                    select x).First();
        }
        public static float FontSize(this BaldiFonts font)
        {
            return font switch
            {
                BaldiFonts.ComicSans12 => 12f,
                BaldiFonts.BoldComicSans12 => 12f,
                BaldiFonts.ComicSans18 => 18f,
                BaldiFonts.ComicSans24 => 24f,
                BaldiFonts.BoldComicSans24 => 24f,
                BaldiFonts.ComicSans36 => 36f,
                BaldiFonts.SmoothComicSans12 => 12f,
                BaldiFonts.SmoothComicSans18 => 18f,
                BaldiFonts.SmoothComicSans24 => 24f,
                BaldiFonts.SmoothComicSans36 => 36f,
                _ => throw new NotImplementedException(),
            };
        }

        public static TMP_FontAsset FontAsset(this BaldiFonts font)
        {
            return font switch
            {
                BaldiFonts.ComicSans12 => BasePlugin.assets.Get<TMP_FontAsset>("COMIC_12_Pro"),
                BaldiFonts.BoldComicSans12 => BasePlugin.assets.Get<TMP_FontAsset>("COMIC_BOLD_12_Pro"),
                BaldiFonts.ComicSans18 => BasePlugin.assets.Get<TMP_FontAsset>("COMIC_18_Pro"),
                BaldiFonts.ComicSans24 => BasePlugin.assets.Get<TMP_FontAsset>("COMIC_24_Pro"),
                BaldiFonts.BoldComicSans24 => BasePlugin.assets.Get<TMP_FontAsset>("COMIC_BOLD_24_Pro"),
                BaldiFonts.ComicSans36 => BasePlugin.assets.Get<TMP_FontAsset>("COMIC_36_Pro"),
                BaldiFonts.SmoothComicSans12 => BasePlugin.assets.Get<TMP_FontAsset>("COMIC_12_Smooth_Pro"),
                BaldiFonts.SmoothComicSans18 => BasePlugin.assets.Get<TMP_FontAsset>("COMIC_18_Smooth_Pro"),
                BaldiFonts.SmoothComicSans24 => BasePlugin.assets.Get<TMP_FontAsset>("COMIC_24_Smooth_Pro"),
                BaldiFonts.SmoothComicSans36 => BasePlugin.assets.Get<TMP_FontAsset>("COMIC_36_Smooth_Pro"),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
