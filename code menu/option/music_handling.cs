using UnityEngine;

public class music_handling : MonoBehaviour
{
    public parser_opt Parser_opt;
    public AudioSource Audio;
    public void Load_properties_sound()
    {
        if (Parser_opt.stru_gen.mute)
            Audio.mute = true;
        else
            Audio.mute = false;
        Audio.volume = Parser_opt.stru_gen.music / 100f;
    }

    private void make_sound_panel()
    {
        GameObject Audio_panel = new GameObject();
        Parser_opt = GameObject.Find("Parser").GetComponent<parser_opt>();
        Audio_panel.AddComponent<AudioSource>();
        Audio_panel.AddComponent<AudioListener>();
        Audio_panel.AddComponent<music_handling>();
        Audio_panel.name = "MusicHandle";
        Audio = Audio_panel.GetComponent<AudioSource>();
        Audio.loop = true;
        Audio.clip = Resources.Load(Parser_opt.stru_gen.music_path) as AudioClip;
        if (Audio.clip == null) {
            Debug.LogError(string.Format("Audio clip not loaded path : {0}", Parser_opt.stru_gen.music_path));
        }
    }
    void Start()
    {
        Parser_opt = GameObject.Find("Parser").GetComponent<parser_opt>();
        if (Parser_opt == null) {
            Debug.LogError(string.Format("Parser_opt not Loaded"));
        }
        if (GameObject.Find("MusicHandle") != null) {
            GameObject Audio_panel = GameObject.Find("MusicHandle");
            Audio = Audio_panel.GetComponent<AudioSource>();
            return;
        }
        var arg = Parser_opt.parsing("option/properties");
        make_sound_panel();
        Audio.Play();
        Load_properties_sound();
        DontDestroyOnLoad(GameObject.Find("MusicHandle"));
    }

    public void music_handle(float value)
    {
        if (Audio.mute == true)
            Audio.mute = false;
        if (Parser_opt == null) {
            Debug.LogError("parser opt not loaded");
            return;
        }
        Parser_opt.stru_gen.music = value;
        Parser_opt.stru_gen.mute = false;
        Audio.volume = value / 100f;
    }
}