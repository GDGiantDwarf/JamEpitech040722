using UnityEngine;

public class sound_handler : MonoBehaviour
{
    public parser_opt Parser_opt;
    public AudioSource Audio;
    public AudioClip[] list_sound;
    public void Load_properties_sound()
    {
        if (Parser_opt.stru_gen.mute)
            Audio.mute = true;
        else
            Audio.mute = false;
        Audio.volume = Parser_opt.stru_gen.sound / 100f;
    }

    private void make_sound_panel()
    {
        GameObject Audio_panel = new GameObject();
        Audio_panel.AddComponent<AudioSource>();
        Audio_panel.AddComponent<parser_opt>();
        Audio_panel.AddComponent<sound_handler>();
        Audio_panel.name = "SoundHandle";
        Audio = Audio_panel.GetComponent<AudioSource>();
        Audio.loop = false;
        Audio.Stop();
        Audio.playOnAwake = false;
    }
    void Start()
    {
        Parser_opt = GameObject.Find("Parser").GetComponent<parser_opt>();
        if (GameObject.Find("SoundHandle") != null) {
            GameObject Audio_panel = GameObject.Find("SoundHandle");
            Audio = Audio_panel.GetComponent<AudioSource>();
        } else {
            var arg = Parser_opt.parsing("option/properties");
            make_sound_panel();
            Load_properties_sound();
            DontDestroyOnLoad(GameObject.Find("SoundHandle"));
        }
        list_sound = new AudioClip[5];
        list_sound[0] = Resources.Load("sound/click") as AudioClip;
        if (list_sound[0] == null) {
            Debug.LogError("sound click not load");
        }
    }

    public void music_handle(float value)
    {
        if (Audio.mute == true)
            Audio.mute = false;
        Audio.volume = value / 100f;
    }

    public void sound_setvol(float value)
    {
        if (Audio.mute == true)
            Audio.mute = false;
        if (Audio == null) {
            Debug.LogError("sound not load");
            return;
        }
        Parser_opt.stru_gen.mute = false;
        Parser_opt.stru_gen.sound = value;
        Audio.volume = value / 100f;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) {
            Audio.clip = list_sound[0];
            Audio.Play();
        }
    }
}