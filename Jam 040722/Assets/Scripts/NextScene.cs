using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public void changeScene()
    {
        int act = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("enter");
        if (act + 1 == SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(0);
        else
            SceneManager.LoadScene(act + 1);
    }
}
