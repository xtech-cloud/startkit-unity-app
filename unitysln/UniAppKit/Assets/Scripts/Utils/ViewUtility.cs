using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using XTC.MVCS;
using XTC.Logger;

public class ViewUtility
{
    public delegate void ErrorCallback(string _error);
    public delegate void FinishCallback(UnityEngine.Object _obj);
    public delegate void ReadyCallback();
    public enum LoadType
    {
        Material,
        Texture,
        TextAsset,
        GameObject,
        AudioClip,
    }

    public static void ResetTransform(Transform _transform)
    {
        _transform.position = Vector3.zero;
        _transform.rotation = Quaternion.identity;
        _transform.localScale = Vector3.one;
    }

    public static void DestroyActiveChildren(GameObject _gameobject)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in _gameobject.transform)
        {
            if (!child.gameObject.activeSelf)
                continue;
            children.Add(child.gameObject);
        }

        foreach (GameObject child in children)
        {
            GameObject.Destroy(child);
        }
    }

    public static void DestroyAllChildren(GameObject _gameobject)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in _gameobject.transform)
        {
            children.Add(child.gameObject);
        }

        foreach (GameObject child in children)
        {
            GameObject.Destroy(child);
        }
    }

    public static void ResetScale(Transform _transform)
    {
        _transform.localScale = Vector3.one;
    }
}
