using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (!_instance)
                {
                    GameObject obj = new GameObject(nameof(T) + " Object");
                    _instance = obj.AddComponent(typeof(T)) as T;
                }
            }

            return _instance;
        }
    }
}