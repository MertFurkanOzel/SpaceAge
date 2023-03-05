using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singletonsc : MonoBehaviour
{
    private static singletonsc original;
    private void Awake()
    {
        if(original!=this)
        {
            if (original != null)
                Destroy(original.gameObject);
            DontDestroyOnLoad(this);
            original = this;
        }
    }
}
