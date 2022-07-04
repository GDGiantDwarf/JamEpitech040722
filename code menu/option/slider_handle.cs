using UnityEngine;
using UnityEngine.UI;

public class slider_handle : MonoBehaviour
{
    private sound_handler sound_handling;
    private music_handling music_script;
    public Slider music_slider;
    public Slider sound_slider;

    public void set_pos()
    {
        music_slider.GetComponent<RectTransform>().sizeDelta = new Vector2(music_slider.GetComponent<RectTransform>().sizeDelta.x * (Screen.width / 3840f), music_slider.GetComponent<RectTransform>().sizeDelta.y * Screen.height / 2160f);
        music_slider.transform.position = new Vector3(Screen.width / 2 + -1253.5f * (Screen.width / 3840f), Screen.height / 2 + 334f * (Screen.height / 2160f), 0f);
        sound_slider.GetComponent<RectTransform>().sizeDelta = new Vector2(sound_slider.GetComponent<RectTransform>().sizeDelta.x * (Screen.width / 3840f), sound_slider.GetComponent<RectTransform>().sizeDelta.y * Screen.height / 2160f);
        sound_slider.transform.position = new Vector3(Screen.width / 2 + -1253.5f * (Screen.width / 3840f), Screen.height / 2 + 444f * (Screen.height / 2160f), 0f);
    }

    void Start()
    {
        set_pos();
        music_script = GameObject.Find("MusicHandle").GetComponent<music_handling>();
        sound_handling = GameObject.Find("SoundHandle").GetComponent<sound_handler>();
        if (music_script == null) {
            Debug.LogError("music_script not loaded");
            return;
        }
        if (sound_handling == null) {
            Debug.LogError("sound_script not loaded");
            return;
        }
        if (GameObject.Find("Parser").GetComponent<parser_opt>().stru_gen.mute) {
            music_script.Audio.mute = true;
            sound_handling.Audio.mute = true;
        }
        if (music_script.Audio.mute == false) {
            music_slider.value = music_script.Audio.volume * 100;
        } else {
            music_slider.value = 0;
        }
        if (sound_handling.Audio.mute == false) {
            sound_slider.value = sound_handling.Audio.volume * 100;
        } else {
            sound_slider.value = 0;
        }
    }

    public void slider_handlig_music(float value)
    {
        if (music_script == null) {
            Debug.LogError("music_script not charged");
            return;
        }
        music_script.music_handle(value);
    }

    public void slider_handlig_sound(float value)
    {
        sound_handling = GameObject.Find("SoundHandle").GetComponent<sound_handler>();
        if (sound_handling == null) {
            Debug.LogError("sound_script not charged");
            return;
        }
        sound_handling.sound_setvol(value);
    }
}