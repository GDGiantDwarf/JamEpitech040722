using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class change_scene_handling : MonoBehaviour
{
    public scene_preced sc_pr;
    public parser_opt parser;
    public first_menu fm;
    public music_handling mh;
    public sound_handler sh;
    public handle_menu menu_game = null;
    public test_placement place_bat = null;
    public info_plaer infoplayer = null;
    /* action of button */

    void Start()
    {
        if (GameObject.Find("scene_preced") != null)
            sc_pr = GameObject.Find("scene_preced").GetComponent<scene_preced>();
        if (SceneManager.GetActiveScene().name == "test") {
            place_bat = gameObject.GetComponent<test_placement>();
            infoplayer = gameObject.GetComponent<info_plaer>();
            foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) {
                if (go.name == "loader") {
                    menu_game = go.GetComponent<handle_menu>();
                    break;
                }
            }
        }
        if (SceneManager.GetActiveScene().name == "test") {
            return;
        }
        parser = GameObject.Find("Parser").GetComponent<parser_opt>();
        if (sc_pr == null) {
            Debug.LogError("sc_pr == null");
        }
    }

    public void change_scene(int scene, GameObject txts, string scene_sw, GameObject obj)
    {
        switch (scene)
        {
            case 0:
                if (SceneManager.GetActiveScene().name == "option") {
                    parser.save_opt();
                } else if (SceneManager.GetActiveScene().name == "test") {
                    infoplayer.save_player_info(Application.dataPath + "/" + infoplayer.path_folder, infoplayer.path_save);
                }
                if (sc_pr != null) {
                    sc_pr.add_last(SceneManager.GetActiveScene().name);
                }
                SceneManager.LoadScene(sceneName:scene_sw);
                break;
            case 1: // quit game if click on quit button
                if (Application.isEditor) { // check if is in game or in editor
                    // UnityEditor.EditorApplication.isPlaying = false; //stop game in editor
                } else {
                    Application.Quit(); // quit game
                }
                break;
            case 30:
                GameObject parent = obj.transform.parent.gameObject;
                parent.SetActive(false);
                break;
            case 31:
                parser.stru_gen.language = obj.name;
                parser.save_opt();
                SceneManager.LoadScene(sceneName:"option");
                break;
            case 33:
                fm.panel_lang.SetActive(true);
                break;
            case 34:
                if (parser.stru_gen.mute) {
                    parser.stru_gen.mute = false;
                } else {
                    parser.stru_gen.mute = true;
                }
                SceneManager.LoadScene(sceneName:"option");
                break;
            case 35:
                SceneManager.LoadScene(sceneName:sc_pr.get_last());
                break;
            case 50: // achat must be null
                // place_bat.placement_bat(int.Parse(obj.name));
                break;
            case 51: // catégorie
                menu_game.active_cat(obj.name);
                break;
        }
    }

    void Update() {
        bool is_f11 = false;
        if (Input.GetKeyDown(KeyCode.F11)) {
            is_f11 = true;
        }
        if (is_f11 && !Screen.fullScreen) {
            Screen.fullScreen = true;
            is_f11 = false;
        } else if (is_f11) {
            Screen.fullScreen = false;
            is_f11 = false;
        }
    }
}