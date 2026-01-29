using BBCR.API;
using BBCR.API.Extensions;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace BBCR
{
    public enum Style
    {
        Unknown = -1,
        Classic = 0,
        Party = 1,
        Demo = 2,
        Null = 3,
        Glitch = 4
    }
    public enum BaldiFonts
    {
        ComicSans12,
        BoldComicSans12,
        ComicSans18,
        ComicSans24,
        BoldComicSans24,
        ComicSans36,
        SmoothComicSans12,
        SmoothComicSans18,
        SmoothComicSans24,
        SmoothComicSans36
    }
    public static class AssetsAPI
    {
        public static readonly Dictionary<AudioType, string[]> audioExtensions = new Dictionary<AudioType, string[]>
        {
            { AudioType.MPEG, new string[] { "mp3", "mp2" } },
            { AudioType.OGGVORBIS, new string[] { "ogg" } },
            { AudioType.WAV, new string[] { "wav" } },
            { AudioType.AIFF, new string[] { "aif", "aiff" } },
            { AudioType.MOD, new string[] { "mod" } },
            { AudioType.IT, new string[] { "it" } },
            { AudioType.S3M, new string[] { "s3m" } },
            { AudioType.XM, new string[] { "xm" } },
            { AudioType.XMA, new string[] { "xma" } }
        };
        private static readonly string[] fallbacks = new string[]
        {
            "",
            "file://",
            "file:///",
            Path.Combine("File:///",""),
            Path.Combine("File://","")
        };
        public static T[] Find<T>() where T : UnityEngine.Object => Resources.FindObjectsOfTypeAll<T>();
        public static T Find<T>(int index = 0) where T : UnityEngine.Object
        {
            try { return Resources.FindObjectsOfTypeAll<T>()[index]; }
            catch { return null; }

        }

        public static Texture2D CreateTexture(byte[] bytes) => CreateTexture(TextureFormat.RGBA32, bytes);
        public static Texture2D CreateTexture(TextureFormat format, byte[] bytes)
        {
            Texture2D texture2D = new Texture2D(2, 2, format, false);
            ImageConversion.LoadImage(texture2D, bytes);
            texture2D.filterMode = FilterMode.Point;
            texture2D.name = "NoName";
            return texture2D;
        }
        public static Texture2D CreateTexture(params string[] path) => CreateTexture(TextureFormat.RGBA32, path);
        public static Texture2D CreateTexture(TextureFormat format, params string[] path)
        {
            Texture2D texture2D = CreateTexture(File.ReadAllBytes(BasePlugin.ModPath+Path.Combine(path)));
            texture2D.name = Path.GetFileNameWithoutExtension(path.Last());
            return texture2D;
        }
        public static AudioType GetAudioType(string path)
        {
            string extension = Path.GetExtension(path).ToLower().Remove(0, 1).Trim();
            foreach (AudioType target in audioExtensions.Keys)
            {
                if (audioExtensions[target].Contains(extension)) 
                    return target;
            }

            throw new NotImplementedException("Unknown audio file type:" + extension + "!");
        }
        public static T LoadAsset<T>(int index = 0) where T : UnityEngine.Object => Resources.FindObjectsOfTypeAll<T>()[index];
        public static T LoadAsset<T>(Func<T, bool> predicate) where T : UnityEngine.Object => (from x in Resources.FindObjectsOfTypeAll<T>() where predicate(x) select x).First();
        public static T LoadAsset<T>(string name) where T : UnityEngine.Object => (from x in Resources.FindObjectsOfTypeAll<T>() where x.name.ToLower() == name.ToLower() select x).First();
        public static AudioClip AudioClipFromFile(params string[] path)
        {
            AudioClip clip;
            UnityWebRequest audioClip;
            string errorMessage = "";
            foreach (string fallback in fallbacks)
            {
                using (audioClip = UnityWebRequestMultimedia.GetAudioClip(fallback + BasePlugin.ModPath + Path.Combine(path), GetAudioType(BasePlugin.ModPath + Path.Combine(path))))
                {
                    audioClip.SendWebRequest();
                    while (!audioClip.isDone) { }
                    if (audioClip.result != UnityWebRequest.Result.Success)
                    {
                        errorMessage = audioClip.responseCode.ToString() + ": " + audioClip.error;
                        continue;
                    }
                    clip = DownloadHandlerAudioClip.GetContent(audioClip);
                    clip.name = Path.GetFileNameWithoutExtension(path.Last());
                    return clip;
                }
            }
            throw new Exception(errorMessage);
        }
        public static Texture2D MakeReadable(this Texture2D original)
        {
            RenderTexture rt = new RenderTexture(original.width, original.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(original, rt);
            RenderTexture.active = rt;
            Texture2D copy = new Texture2D(original.width, original.height, original.format, false);
            copy.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            copy.Apply();
            RenderTexture.active = null;
            rt.Release();
            return copy;
        }
        public static Sprite[] CreateSpriteSheetFromLeftTop(Texture2D texture, int tilesByX, int tilesByY, float pixelsPerUnit = 1f)
        {
            List<Sprite> result = new List<Sprite>();
            int XSize = texture.width / tilesByX;
            int YSize = texture.height / tilesByY;
            for (int y = tilesByY - 1; y >= 0; y--)
            {
                for (int x = 0; x < tilesByX; x++)
                {
                    result.Add(Sprite.Create(texture, new Rect(x * XSize, y * YSize, XSize, YSize), Vector2.one / 2f, pixelsPerUnit, 0, SpriteMeshType.FullRect));
                }
            }
            return result.ToArray();
        }
        public static Sprite[] CreateSpriteSheetFromLeftDown(Texture2D texture, int tilesByX, int tilesByY, float pixelsPerUnit = 1f)
        {
            List<Sprite> result = new List<Sprite>();
            int XSize = texture.width / tilesByX;
            int YSize = texture.height / tilesByY;
            for (int y = 0; y < tilesByY; y++)
            {
                for (int x = 0; x < tilesByX; x++)
                {
                    result.Add(Sprite.Create(texture, new Rect(x * XSize, y * YSize, XSize, YSize), Vector2.one / 2f, pixelsPerUnit, 0, SpriteMeshType.FullRect));
                }
            }
            return result.ToArray();
        }
        public static Sprite ToSprite(this Texture2D tex, float pixelsPerUnit = 1f) => tex.ToSprite(new Vector2(0.5f, 0.5f), pixelsPerUnit);
        public static Sprite ToSprite(this Texture2D tex, Vector2 center, float pixelsPerUnit = 1)
        {
            Sprite sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), center, pixelsPerUnit, 0, SpriteMeshType.FullRect);
            sprite.name = "Sprite_" + tex.name;
            return sprite;
        }
    }
}
