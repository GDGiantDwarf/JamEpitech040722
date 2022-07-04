using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class first_game_handle : MonoBehaviour {
    public button_acceuil button_maker;
    public GameObject pause_panel;
    public camera Camera;
    public bool is_paused;

    private void make_pause_panel()
    {
        parser_opt parseropt;
        gameObject.AddComponent<parser_opt>();
        parseropt = gameObject.GetComponent<parser_opt>();
        parseropt.parsing("option/properties");
        GameObject panel = new GameObject();
        panel.name = "pause_panel";
        panel.layer = 5;
        panel.transform.position = new Vector3(1920 * (Screen.width / 3840f), 1080 * (Screen.height / 2160f), 0);
        panel.transform.SetParent(transform);
        panel.AddComponent<Canvas>();
        panel.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        panel.AddComponent<GraphicRaycaster>();
        panel.AddComponent<Image>();
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(20 * (Screen.width / 1980f), 20  * (Screen.height / 1080f));
        panel.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
        button_maker.create_button("game/pause_" + parseropt.stru_gen.language);
        panel.SetActive(false);
        pause_panel = panel;
    }

    private void Start() {
        button_maker = GameObject.Find("ScriptLoader").GetComponent<button_acceuil>();
        pause_panel = GameObject.Find("pause_panel_canvas");
        Camera = GameObject.Find("Camerascript").GetComponent<camera>();
        make_pause_panel();
        pause_panel.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyUp(KeyCode.Escape) && !is_paused) {
            Cursor.lockState = CursorLockMode.None;
            is_paused = true;
            Cursor.visible = true;
            pause_panel.SetActive(true);
        } else if (Input.GetKeyUp(KeyCode.Escape) && is_paused) {
            if (Camera.followplayer) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            is_paused = false;
            pause_panel.SetActive(false);
        }
    }
}