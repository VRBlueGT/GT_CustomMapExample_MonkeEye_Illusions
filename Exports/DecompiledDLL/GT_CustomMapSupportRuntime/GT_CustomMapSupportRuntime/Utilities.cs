using UnityEngine;

namespace GT_CustomMapSupportRuntime;

public static class Utilities
{
    public static string SanitizeString(string str)
    {
        string text = "";
        for (int i = 0; i < str.Length; i++)
        {
            if (char.IsLetterOrDigit(str[i]))
            {
                text += str[i];
            }
            else if (char.IsWhiteSpace(str[i]))
            {
                text += "-";
            }
        }
        return text;
    }

    private static void StripMeshesForObjectsOfType<T>(GameObject rootObject)
    {
        T[] componentsInChildren = rootObject.GetComponentsInChildren<T>();
        foreach (object obj in componentsInChildren)
        {
            Component val = (Component)((obj is Component) ? obj : null);
            if (!((Object)(object)val == (Object)null))
            {
                if ((Object)(object)val.gameObject.GetComponent<Renderer>() != (Object)null)
                {
                    Object.DestroyImmediate((Object)(object)val.gameObject.GetComponent<Renderer>());
                }
                if ((Object)(object)val.gameObject.GetComponent<MeshFilter>() != (Object)null)
                {
                    Object.DestroyImmediate((Object)(object)val.gameObject.GetComponent<MeshFilter>());
                }
            }
        }
    }

    public static string GetSceneNameFromFilePath(string filePath, bool sanitizeName = true)
    {
        string[] array = filePath.Split('/')[^1].Split('.');
        string text = "";
        for (int i = 0; i < array.Length - 1; i++)
        {
            text += array[i];
            if (i < array.Length - 2)
            {
                text += ".";
            }
        }
        if (!sanitizeName)
        {
            return text;
        }
        return SanitizeString(text);
    }
}
