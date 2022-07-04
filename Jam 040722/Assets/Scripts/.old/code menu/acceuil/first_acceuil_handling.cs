using UnityEngine;

public class first_acceuil_handling : MonoBehaviour
{
    public parser_opt Parser;
    public button_acceuil Button_acceuil;

    void Start()
    {
        DontDestroyOnLoad(GameObject.Find("Parser"));
        if (GameObject.Find("scene_preced") == null) {
            GameObject sc_preced = new GameObject();
            sc_preced.AddComponent<scene_preced>();
            sc_preced.name = "scene_preced";
            DontDestroyOnLoad(sc_preced);
        }
    }

    void Update()
    {
        // check if key is escape to quit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // check if is in editor
            if (Application.isEditor) {
                // UnityEditor.EditorApplication.isPlaying = false;
            } else {
                Application.Quit();
            }
        }
    }
}
