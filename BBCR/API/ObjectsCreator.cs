using BBCR.API.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BBCR.API
{
    public static class ObjectsCreator
    {
        public static StandardDoorMats CreateDoor(string name, Texture2D openTex, Texture2D closeTex)
        {
            StandardDoorMats template = AssetsAPI.LoadAsset<StandardDoorMats>("ClassDoorSet");
            StandardDoorMats res = ScriptableObject.CreateInstance<StandardDoorMats>();
            res.open = new Material(template.open);
            res.open.SetMainTexture(openTex);
            res.shut = new Material(template.shut);
            res.shut.SetMainTexture(closeTex);
            res.name = name;
            return res;
        }

        public static SoundObject CreateSoundObject(AudioClip clip, SoundType type, Color? color = null, float sublength = -1f, string subtitle = "Rost")
        {
            SoundObject obj = ScriptableObject.CreateInstance<SoundObject>();
            obj.soundClip = clip;
            obj.subtitle = true;
            if (sublength == 0f) obj.subtitle = false;
            obj.subDuration = sublength == -1 ? clip.length + 1f : sublength;
            obj.soundType = type;
            obj.soundKey = subtitle;
            obj.color = color ?? Color.white;
            obj.name = subtitle;
            return obj;
        }

        public static T CreateText<T>(BaldiFonts font, string text, Transform parent, Vector3 position, bool correctPosition = false) where T : TMP_Text =>
            CreateText<T>(font, text, parent, position, Color.white, correctPosition);
        
        public static T CreateText<T>(BaldiFonts font, string text, Transform parent, Vector3 position, Color? color = null, bool correctPosition = false) where T : TMP_Text
        {
            T tmp = new GameObject().AddComponent<T>();
            tmp.name = "Text";
            tmp.gameObject.layer = LayerMask.NameToLayer("UI");
            tmp.transform.SetParent(parent);
            tmp.gameObject.transform.localScale = Vector3.one;
            tmp.fontSize = font.FontSize();
            tmp.font = font.FontAsset();
            tmp.color = color ?? Color.white;
            if (correctPosition)
            {
                tmp.transform.localPosition = new Vector3(-240f, 180f) + (new Vector3(position.x, position.y * -1f));
            }
            else
            {
                tmp.transform.localPosition = position;
            }
            tmp.text = text;
            return tmp;
        }
        public static Image CreateImage(Sprite spr, Transform parent, Vector3 position, bool correctPosition = false, float scale = 1f)
        {
            Image img = new GameObject().AddComponent<Image>();
            img.gameObject.layer = LayerMask.NameToLayer("UI");
            img.transform.SetParent(parent);
            img.sprite = spr;
            img.gameObject.transform.localScale = Vector3.one;
            img.rectTransform.offsetMin = new Vector2(-spr.rect.width / 2f, -spr.rect.height / 2f);
            img.rectTransform.offsetMax = new Vector2(spr.rect.width / 2f, spr.rect.height / 2f);
            img.rectTransform.anchorMin = new Vector2(0f, 1f);
            img.rectTransform.anchorMax = new Vector2(0f, 1f);
            if (correctPosition)
            {
                img.transform.localPosition = new Vector3(-240f, 180f) + (new Vector3(position.x, position.y * -1f));
            }
            else
            {
                img.transform.localPosition = position;
            }
            img.transform.localScale *= scale;
            return img;
        }
    }
}
