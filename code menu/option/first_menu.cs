using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class first_menu : MonoBehaviour
{
    public scene_preced sc_pr;
    public button_acceuil Button_acceuil;
    public Resolution init_size_screen;
    public parser_opt parser;
    public GameObject panel_lang;
    private bool is_pressed = false;

    void make_lang_panel()
    {
        GameObject panel = new GameObject();
        panel.name = "langpanel";
        panel.transform.position = new Vector3(1920 * (Screen.width / 3840f), 1080 * (Screen.height / 2160f), 0);
        panel.transform.SetParent(transform);
        panel.AddComponent<RectTransform>();
        panel.AddComponent<Image>();
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(3840 * (Screen.width / 1980f), 3160  * (Screen.height / 1080f));
        panel.GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.5f);
        Button_acceuil.create_button("option/lang_" + parser.stru_gen.language);
        panel.SetActive(false);
        panel_lang = panel;
    }

    void make_resolution()
    {
        GameObject list = GameObject.Find("Lang_Dropbox");
        if (list == null) {
            Debug.LogError("list not found");
            return;
        }
        Dropdown drop = list.GetComponent<Dropdown>();
        if (drop == null) {
            Debug.LogError("dropdown not find");
            return;
        }
        Text text = GameObject.Find("Label").GetComponent<Text>();
        if (text == null) {
            Debug.LogError("textbox not found");
            return;
        }
        Resolution[] resolutions = Screen.resolutions;
        if (resolutions == null) {
            Debug.LogError("resolutions cant be get");
            return;
        }
        int index = 0;
        drop.options.Clear();
        foreach (var res in resolutions) {
            drop.options.Add(new Dropdown.OptionData() { text = res.ToString()});
            if (res.ToString().Contains(string.Format("{0} x {1} @ {2}", Screen.width.ToString(), Screen.height.ToString(), Screen.currentResolution.refreshRate.ToString()))){
                text.text = res.ToString();
                drop.value = index;
            }
            index++;
        }
        drop.onValueChanged.AddListener(delegate {dropdown_item_selected(list);});
    }

    void dropdown_item_selected(GameObject list)
    {
        Dropdown drop = list.GetComponent<Dropdown>();
        if (drop == null) {
            Debug.LogError("dropdown not find");
            return;
        }
        int index = drop.value;
        Text text = GameObject.Find("Label").GetComponent<Text>();
        Resolution[] resolution = Screen.resolutions;
        if (text == null) {
            Debug.LogError("textbox not found");
            return;
        }
        text.text = drop.options[index].text;
        foreach (var res in resolution) {
            if (text.text == res.ToString()) {
                string[] args = res.ToString().Trim(' ', 'H', 'z', 'h', 'Z').Replace('x', '@').Split('@');
                Screen.SetResolution(int.Parse(args[0]), int.Parse(args[1]), Screen.fullScreen, int.Parse(args[2]));
                SceneManager.LoadScene(sceneName:"option");
                break;
            }
        }
    }

    void init_dropdown_pos()
    {
        GameObject list = GameObject.Find("Lang_Dropbox");
        if (list == null) {
            Debug.LogError("list not found");
            return;
        }
        Text text = GameObject.Find("Label").GetComponent<Text>();
        if (text == null) {
            Debug.LogError("textbox not found");
            return;
        }
        list.GetComponent<RectTransform>().sizeDelta = new Vector2(list.GetComponent<RectTransform>().sizeDelta.x * (Screen.width / 3840f), list.GetComponent<RectTransform>().sizeDelta.y * (Screen.height / 2160f));
        list.transform.position = new Vector3(758f * (Screen.width / 3840f), 1696f * (Screen.height / 2160f), 0);
        text.fontSize = (int)(text.fontSize * (Screen.height / 2160f));
    }

    void Start()
    {
        init_size_screen = Screen.currentResolution;
        sc_pr = GameObject.Find("scene_preced").GetComponent<scene_preced>();
        init_dropdown_pos();
        make_lang_panel();
        make_resolution();
    }

    void Update()
    {
        // if escape is pressed then change scene for start
        if (Input.GetKeyDown(KeyCode.S)) {
            parser.save_opt();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(sceneName:sc_pr.get_last());
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.W) && !is_pressed) {
            string[] args = init_size_screen.ToString().Trim(' ', 'H', 'z', 'h', 'Z').Replace('x', '@').Split('@');
            Screen.SetResolution(int.Parse(args[0]), int.Parse(args[1]), Screen.fullScreen, int.Parse(args[2]));
            SceneManager.LoadScene(sceneName:"option");
            is_pressed = true;
        } else if ((Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.W)) && is_pressed) {
            is_pressed = false;
        }
    }
}