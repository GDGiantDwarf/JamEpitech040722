using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class option_button : MonoBehaviour
{
    public Button[] btns;
    public state st;
    public camera cam;

    void Start () {
        foreach (var btn in btns) {
            btn.onClick.AddListener(delegate {button_switch(btn.name);});
        }
    }

    void button_switch (string name) {
        switch (name) {
            case "back_to_start":
                st.character.transform.position = st.char_pos;
                Cursor.visible = false;
                st.isActive = false;
                cam.is_paused = false;
                break;
            case "back_to_menu":
                SceneManager.LoadScene(0);
                break;
            default:
                break;
        }
    }
}
