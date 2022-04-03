using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    static DoNotDestroy instance;


    private void Awake()
    {
        if (GetComponent<AudioManager>() == true)
        {
            if (instance != null) Destroy(gameObject);
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}


