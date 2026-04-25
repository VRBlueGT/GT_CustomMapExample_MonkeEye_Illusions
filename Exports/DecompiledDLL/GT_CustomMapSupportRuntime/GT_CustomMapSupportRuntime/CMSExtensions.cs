using System;
using UnityEngine;

namespace GT_CustomMapSupportRuntime;

public static class CMSExtensions
{
    public static string GetHierarchyPath(this Transform transform)
    {
        string text = ((Object)transform).name;
        while (Object.op_Implicit((Object)(object)transform.parent))
        {
            transform = transform.parent;
            text = ((Object)transform).name + "/" + text;
        }
        return "/" + text;
    }

    public static string GetHierarchyPath(this Transform transform, int maxDepth)
    {
        string text = ((Object)transform).name;
        int num = 0;
        while (Object.op_Implicit((Object)(object)transform.parent) && num < maxDepth)
        {
            transform = transform.parent;
            text = ((Object)transform).name + "/" + text;
            num++;
        }
        return "/" + text;
    }

    public static string GetHierarchyPath(this Transform transform, Transform stopper)
    {
        string text = ((Object)transform).name;
        while (Object.op_Implicit((Object)(object)transform.parent) && (Object)(object)transform.parent != (Object)(object)stopper)
        {
            transform = transform.parent;
            text = ((Object)transform).name + "/" + text;
        }
        return "/" + text;
    }

    public static string GetHierarchyPath(this GameObject gameObject)
    {
        return gameObject.transform.GetHierarchyPath();
    }

    public static string GetHierarchyPath(this GameObject gameObject, int limit)
    {
        return gameObject.transform.GetHierarchyPath(limit);
    }

    public static string[] GetHierarchyPaths(this GameObject[] gobj)
    {
        string[] array = new string[gobj.Length];
        for (int i = 0; i < gobj.Length; i++)
        {
            array[i] = gobj[i].GetHierarchyPath();
        }
        return array;
    }

    public static string[] GetHierarchyPaths(this Transform[] xform)
    {
        string[] array = new string[xform.Length];
        for (int i = 0; i < xform.Length; i++)
        {
            array[i] = xform[i].GetHierarchyPath();
        }
        return array;
    }

    public static T? GetComponentByHierarchyPath<T>(this GameObject root, string path) where T : Component
    {
        string[] array = path.Split(new string[1] { "/->/" }, StringSplitOptions.None);
        if (array.Length < 2)
        {
            return default(T);
        }
        string[] array2 = array[0].Split(new string[1] { "/" }, StringSplitOptions.RemoveEmptyEntries);
        Transform val = root.transform;
        for (int i = 1; i < array2.Length; i++)
        {
            string text = array2[i];
            val = val.Find(text);
            if ((Object)(object)val == (Object)null)
            {
                return default(T);
            }
        }
        Type type = Type.GetType(array[1].Split('#')[0]);
        if (type == null)
        {
            return default(T);
        }
        Component component = ((Component)val).GetComponent(type);
        if ((Object)(object)component == (Object)null)
        {
            return default(T);
        }
        return (T)(object)((component is T) ? component : null);
    }

    public static int GetHierarchyDepth(this Transform xform)
    {
        int num = 0;
        Transform parent = xform.parent;
        while ((Object)(object)parent != (Object)null)
        {
            num++;
            parent = parent.parent;
        }
        return num;
    }
}
