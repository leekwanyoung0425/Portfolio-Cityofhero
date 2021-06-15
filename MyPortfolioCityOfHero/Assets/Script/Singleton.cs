using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{    
    private static T instance = null;
    public static T I
    {
        get
        {
            if(null == instance)
            {
                instance = FindObjectOfType<T>();
                if (null == instance)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).ToString();
                    instance = obj.AddComponent<T>();
                }
            }            
            return instance;
        }
    }
}
