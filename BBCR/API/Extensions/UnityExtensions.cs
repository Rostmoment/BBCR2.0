using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace BBCR.API.Extensions
{
    public static class UnityExtensions
    {
        public static Transform[] GetChilds(this Transform parent)
        {
            List<Transform> transforms = new List<Transform>();
            for (int i = 0; i < parent.childCount; i++)
            {
                transforms.Add(parent.GetChild(i));
            }
            return transforms.ToArray();
        }
        public static void SetMainTexture(this Material me, Texture texture) => me.SetTexture("_MainTex", texture);
        public static bool DeleteComponent<T>(this GameObject obj) where T : Component
        {
            if (obj == null) return false;
            if (obj.GetComponent<T>() == null) return false;
            UnityEngine.Object.Destroy(obj.GetComponent<T>());
            return true;
        }
        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            if (obj.GetComponent<T>() == null) return obj.AddComponent<T>();
            return obj.GetComponent<T>();
        }


        public static Color Copy(this Color color, float r = float.NaN, float g = float.NaN, float b = float.NaN, float a = float.NaN)
        {
            if (float.IsNaN(r)) r = color.r;
            if (float.IsNaN(g)) g = color.g;
            if (float.IsNaN(b)) b = color.b;
            if (float.IsNaN(a)) a = color.a;
            return new Color(r, g, b, a);
        }
    }
}
