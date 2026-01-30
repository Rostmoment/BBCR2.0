using BBCR.API;
using BBCR.ModdedContent;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BBCR
{
	[BepInPlugin("rost.moment.baldiremaster.better", "Baldi's Basics Classic Remastered 2.0", "1.0")]
	public class BasePlugin : BaseUnityPlugin
	{
        public static string ModPath => "BALDI_Data/StreamingAssets/Modded/rost.moment.baldiremaster.better/";
        public static AssetManager assets = new AssetManager();
        public static new ManualLogSource Logger { get; private set; }
        public static BasePlugin Instance { get; private set; }

        #region loading
        private IEnumerator LoadTextures()
        {
            yield return "Loading textures...";

            assets.Add("OfficeOpen", AssetsAPI.CreateTexture("Principal_Open.png"));
            yield return null;

            assets.Add("OfficeClosed", AssetsAPI.CreateTexture("Principal_Closed.png"));
            yield return null;

            assets.Add("NotebookCounter", AssetsAPI.CreateSpriteSheetFromLeftTop(AssetsAPI.CreateTexture("NotebookIconSheet.png"), 4, 3, 10));
            yield return null;

            assets.Add("ExitCounter", AssetsAPI.CreateSpriteSheetFromLeftTop(AssetsAPI.CreateTexture("ExitIconSheet.png"), 4, 3, 10));
            yield return null;

            assets.Add("NewChalkCloud", AssetsAPI.CreateTexture("NewChalk.png"));
            yield return null;

            assets.AddFromResources<Texture2D>("ItemSlot5");
            yield return null;
        }

        private IEnumerator LoadSounds()
        {
            yield return "Loading sounds...";

            assets.Add("ZestyBarEat", ObjectsCreator.CreateSoundObject(AssetsAPI.AudioClipFromFile("ChipCrunch.wav"), SoundType.Effect, Color.white, 0.5f, "*CRUNCH*"));
            yield return null;

            assets.Add("NotebookCollect", ObjectsCreator.CreateSoundObject(AssetsAPI.AudioClipFromFile("NotebookCollect.wav"), SoundType.Effect, sublength: 0));
            yield return null;

            assets.Add("GetOutWhileStillCan", ObjectsCreator.CreateSoundObject(AssetsAPI.AudioClipFromFile("GETOUT.wav"), SoundType.Voice, Color.white, -1, "GET OUT WHILE YOU STILL CAN!!!"));
            yield return null;
        }

        private IEnumerator LoadPrefabs()
        {
            yield return "Loading prefabs...";

            assets.AddFromResources<HideableLocker>("BlueLockerPrefab", 0);
            yield return null;

            assets.AddFromResources<WaterFountain>("WaterFountainPrefab", 0);
            yield return null;

            assets.AddFromResources<TMP_FontAsset>();
            yield return null;
        }

        private IEnumerator LoadLanguages()
        {
            yield return "Loading languages...";

            LanguageAPI.LoadAll();
            yield return null;
        }
        #endregion

        void Awake()
		{
			Harmony harmony = new Harmony("rost.moment.baldiremaster.better");
			harmony.PatchAll();
			Logger = base.Logger;
			Instance = this;

			ModdedOptionMenu.InitializeGlobal();
            OptionsAPI.onInitialize += ModdedOptionMenu.Initialize;
			OptionsAPI.OnClose += ModdedOptionMenu.Save;


            LoadingAPI.AddLoadingEvent(LoadTextures());
            LoadingAPI.AddLoadingEvent(LoadSounds());
            LoadingAPI.AddLoadingEvent(LoadPrefabs());
            LoadingAPI.AddLoadingEvent(LoadLanguages());


        }
	}
}
