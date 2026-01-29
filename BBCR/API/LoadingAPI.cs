using BBCR.API.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BBCR.API
{

    class LoadingSceen : MonoBehaviour
    {
        private static LoadingSceen Instance;
        private int finished = 0;
        private bool inProgress;
        public MainMenu mainMenu;

        // UI элементы
        private GameObject loadingCanvas;
        private TMP_Text loadingText;
        private TMP_Text progressText;
        private Slider progressBar;
        private Image backgroundPanel;

        private void Awake()
        {
            inProgress = false;
            Instance = this;
            CursorController.Instance.DisableClick(true);

            if (LoadingAPI.enumerators.EmptyOrNull())
            {
                Destroy(this);
                return;
            }
            if (LoadingAPI.Finished)
            {
                Destroy(this);
                return;
            }

            CreateLoadingUI();
        }

        private void CreateLoadingUI()
        {
            loadingCanvas = new GameObject("LoadingCanvas");
            Canvas canvas = loadingCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 9999; // Поверх всего

            CanvasScaler scaler = loadingCanvas.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            loadingCanvas.AddComponent<GraphicRaycaster>();

            GameObject bgObject = new GameObject("Background");
            bgObject.transform.SetParent(loadingCanvas.transform, false);
            backgroundPanel = bgObject.AddComponent<Image>();
            backgroundPanel.color = new Color(0, 0, 0, 0.95f);

            RectTransform bgRect = bgObject.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;

            GameObject textObject = new GameObject("LoadingText");
            textObject.transform.SetParent(loadingCanvas.transform, false);
            loadingText = textObject.AddComponent<TextMeshProUGUI>();
            loadingText.text = "Loading Mod...";
            loadingText.fontSize = 48;
            loadingText.alignment = TextAlignmentOptions.Center;
            loadingText.color = Color.white;

            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.5f, 0.6f);
            textRect.anchorMax = new Vector2(0.5f, 0.6f);
            textRect.sizeDelta = new Vector2(800, 100);
            textRect.anchoredPosition = Vector2.zero;

            GameObject barBgObject = new GameObject("ProgressBarBackground");
            barBgObject.transform.SetParent(loadingCanvas.transform, false);
            Image barBg = barBgObject.AddComponent<Image>();
            barBg.color = new Color(0.2f, 0.2f, 0.2f, 1f);

            RectTransform barBgRect = barBgObject.GetComponent<RectTransform>();
            barBgRect.anchorMin = new Vector2(0.5f, 0.5f);
            barBgRect.anchorMax = new Vector2(0.5f, 0.5f);
            barBgRect.sizeDelta = new Vector2(600, 40);
            barBgRect.anchoredPosition = Vector2.zero;

            GameObject sliderObject = new GameObject("ProgressBar");
            sliderObject.transform.SetParent(barBgObject.transform, false);
            progressBar = sliderObject.AddComponent<Slider>();

            RectTransform sliderRect = sliderObject.GetComponent<RectTransform>();
            sliderRect.anchorMin = Vector2.zero;
            sliderRect.anchorMax = Vector2.one;
            sliderRect.sizeDelta = Vector2.zero;

            GameObject fillArea = new GameObject("Fill Area");
            fillArea.transform.SetParent(sliderObject.transform, false);
            RectTransform fillAreaRect = fillArea.AddComponent<RectTransform>();
            fillAreaRect.anchorMin = Vector2.zero;
            fillAreaRect.anchorMax = Vector2.one;
            fillAreaRect.sizeDelta = new Vector2(-10, -10);

            GameObject fill = new GameObject("Fill");
            fill.transform.SetParent(fillArea.transform, false);
            Image fillImage = fill.AddComponent<Image>();
            fillImage.color = new Color(0.2f, 0.8f, 0.2f, 1f);

            RectTransform fillRect = fill.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.sizeDelta = Vector2.zero;

            progressBar.fillRect = fillRect;
            progressBar.minValue = 0;
            progressBar.maxValue = 1;
            progressBar.value = 0;

            GameObject progressTextObject = new GameObject("ProgressText");
            progressTextObject.transform.SetParent(loadingCanvas.transform, false);
            progressText = progressTextObject.AddComponent<TextMeshProUGUI>();
            progressText.text = "0%";
            progressText.fontSize = 32;
            progressText.alignment = TextAlignmentOptions.Center;
            progressText.color = Color.white;

            RectTransform progressTextRect = progressTextObject.GetComponent<RectTransform>();
            progressTextRect.anchorMin = new Vector2(0.5f, 0.4f);
            progressTextRect.anchorMax = new Vector2(0.5f, 0.4f);
            progressTextRect.sizeDelta = new Vector2(400, 60);
            progressTextRect.anchoredPosition = Vector2.zero;

            DontDestroyOnLoad(loadingCanvas);
        }

        private void Update()
        {
            try
            {
                if (LoadingAPI.enumerators == null || LoadingAPI.enumerators.Count == 0)
                {
                    Destroy(this);
                    return;
                }

                if (finished >= LoadingAPI.enumerators.Count)
                {
                    Destroy(this);
                    return;
                }

                float progress = (float)finished / LoadingAPI.enumerators.Count;
                if (progressBar != null)
                    progressBar.value = progress;

                if (progressText != null)
                    progressText.text = $"{Mathf.RoundToInt(progress * 100)}%";

                if (!inProgress)
                    StartLoading(LoadingAPI.enumerators[finished]);
            }
            catch (ArgumentOutOfRangeException)
            {
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            Instance = null;
            LoadingAPI.Finished = true;
            CursorController.Instance.DisableClick(false);

            if (loadingCanvas != null)
                Destroy(loadingCanvas);
        }

        private void StartLoading(IEnumerator toLoad)
        {
            inProgress = true;
            StartCoroutine(Loading(toLoad));
        }

        private IEnumerator Loading(IEnumerator toLoad)
        {
            while (toLoad.MoveNext())
            {
                if (toLoad.Current != null && toLoad.Current.GetType() == typeof(string))
                {
                    if (loadingText != null)
                        loadingText.text = (string)toLoad.Current;
                }
                yield return null;
            }
            inProgress = false;
            finished++;
            yield break;
        }
    }
    class LoadingAPI
    {
        public static bool Finished { set; get; }
        public static List<IEnumerator> enumerators;
        public static void AddLoadingEvent(IEnumerator enumerator)
        {
            Finished = false;
            enumerators ??= new List<IEnumerator>();

            if (!enumerators.Contains(enumerator)) 
                enumerators.Add(enumerator);
        }

    }
}
