using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class Extensions
{
    public static string ToJson(this object obj) =>
        JsonUtility.ToJson(obj);

    public static T ToDeserialized<T>(this string json) =>
        JsonUtility.FromJson<T>(json);

    public static IEnumerable<T> GetValues<T>() =>
        Enum.GetValues(typeof(T)).Cast<T>();

    public static void ChangeImageAlpha(this Image image, float targetAlpha)
    {
        Color color = image.color;
        image.color = new Color(color.r, color.g, color.g, targetAlpha);
    }
}