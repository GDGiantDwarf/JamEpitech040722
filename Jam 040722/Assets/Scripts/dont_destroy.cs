using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dont_destroy : MonoBehaviour
{
    public GameObject death_menu;
    private static GameObject instance;
    void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(this);
            return;
        }
        instance = this.gameObject;
        DontDestroyOnLoad(this.gameObject);
    }
}
