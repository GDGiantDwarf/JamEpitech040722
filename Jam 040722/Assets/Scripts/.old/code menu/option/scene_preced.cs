using UnityEngine;
using System.Collections.Generic;
using System;

public class scene_preced : MonoBehaviour
{
    private List<string> scene_preceds = new List<string>();

    void Start()
    {
        DontDestroyOnLoad(GameObject.Find("scene_preced"));
    }

    public string get_last()
    {
        return (scene_preceds[scene_preceds.Count - 1]);
    }

    public void add_last(string name)
    {
        scene_preceds.Add(name);
    }

    public void reset()
    {
        scene_preceds.Clear();
    }
}