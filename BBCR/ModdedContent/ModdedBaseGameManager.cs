using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace BBCR.ModdedContent
{
    class ModdedBaseGameManager : MonoBehaviour
    {
        private static readonly Dictionary<int, int> frames = new Dictionary<int, int>()
        {
            {0, 0}, {1, 0}, {2, 0}, {3, 0},
            {4, 1}, {5, 1}, {6, 1}, {7, 1},
            {8, 2}, {9, 2}, {10, 2}, {11, 2},
            {12, 3}, {13, 3}, {14, 3},
            {15, 4}, {16, 4}, {17, 4}, {18, 4},
            {19, 5}, {20, 5}, {21, 5}, {22, 5},
            {23, 6}, {24, 6}, {25, 6}, {26, 5},
            {27, 7}, {28, 7}, {29, 7},
            {30, 8}, {31, 8}, {32, 8}, {33, 8},
            {34, 9}, {35, 9}, {36, 9}, {37, 9},
            {38, 10}, {39, 10}, {40, 10}, {41, 10},
            {42, 11}, {43, 11}, {44, 11},

        };
        private bool animatorSetuped;
        private Sprite[] toPlay;
        private Sprite[] notebookCounter = BasePlugin.assets.Get<Sprite[]>("NotebookCounter");
        private Sprite[] exitCounter = BasePlugin.assets.Get<Sprite[]>("ExitCounter");
        private int animatorIndex;
        private EnvironmentController ec;
        private BaseGameManager bgm;
        private GameObject animator;
        public void Initialize(EnvironmentController environmentController, BaseGameManager baseGameManager)
        {
            animatorIndex = -1;
            ec = environmentController;
            bgm = baseGameManager;
            animatorSetuped = false;
        }
        public void PlayAnimation()
        {
            if (VariablesStorage.styleIsEndless || bgm.FoundNotebooks < ec.notebookTotal)
                toPlay = notebookCounter;
            else
                toPlay = exitCounter;

            if (animatorIndex == -1)
                animatorIndex = 0;
        }
        private IEnumerator TrySetupAnimator()
        {
            while (true)
            {
                try
                {
                    HudManager hud = CoreGameManager.Instance.GetHud(0);
                    // 640, 360
                    animator = new GameObject("Animator");
                    animator.transform.SetParent(hud.transform);

                    Image image = animator.AddComponent<Image>();
                    image.sprite = notebookCounter[0];
                    image.rectTransform.anchoredPosition3D = hud.transform.Find("Notebook Text").GetComponent<RectTransform>().anchoredPosition3D;
                    animator.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                    animator.transform.SetSiblingIndex(0);
                    hud.transform.Find("Notebook Text").GetComponent<RectTransform>().anchoredPosition = new Vector2(50, -12);
                    image.rectTransform.anchoredPosition = new Vector2(-280, 150);
                    animatorSetuped = true;
                    yield break;
                }
                catch (NullReferenceException)
                {
                }
                yield return null;
            }
        }
        private void SetupAnimator()
        {
            StartCoroutine(TrySetupAnimator());
        }
        void Update()
        {
            if (ec != null && bgm != null)
            {
                if (animatorIndex > frames.Last().Key)
                {
                    animator.GetComponent<Image>().sprite = toPlay[0];
                    animatorIndex = -1;
                }
                if (animatorIndex >= 0)
                {
                    animator.GetComponent<Image>().sprite = toPlay[frames[animatorIndex]];
                    animatorIndex++;
                }
                if (!animatorSetuped)
                    SetupAnimator();

                if (VariablesStorage.styleIsEndless)
                    CoreGameManager.Instance.GetHud(0).UpdateText(0, bgm.FoundNotebooks.ToString());

                else
                {
                    try
                    {
                        if (bgm.FoundNotebooks >= ec.notebookTotal)
                            CoreGameManager.Instance.GetHud(0).UpdateText(0, $"{bgm.elevatorsClosed}/{bgm.ec.elevators.Count}");
                        else
                            CoreGameManager.Instance.GetHud(0).UpdateText(0, $"{bgm.FoundNotebooks}/{ec.notebookTotal}");
                    }
                    catch (NullReferenceException)
                    {
                    }
                }
            }
        }
    }
}
