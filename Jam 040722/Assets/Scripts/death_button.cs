using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class death_button : MonoBehaviour
{
    public Button[] btns;
    public bool isActive;

    public state charact;
    void Start() {
        // charact = GameObject.Find("octopus_animated_yellow");
        isActive = false;
        foreach (var btn in btns) {
            btn.onClick.AddListener(delegate {button_switch(btn.name); });
        }
    }

    void button_switch(string name) {
        switch (name) {
            case "try_again":
                charact.isActive = false;
                SceneManager.LoadScene(sceneName:SceneManager.GetActiveScene().name);
                break;
            case "back_to_menu":
                charact.isActive = false;
                SceneManager.LoadScene(sceneName:"MenuScene");
                break;
        }
    }
}
