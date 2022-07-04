using UnityEngine;
using UnityEngine.UI;

public class button_acceuil : MonoBehaviour
{
    public parser Parser;
    public change_scene_handling Scene;

    /* load all picture of a sliced sprite sheet and return *
     * the good sliced part for given name */
    private Sprite Load(string imageName, string spriteName)
    {
        if (imageName == null) {
            return (null);
        }
        // load all and store in sprite tab
        Sprite[] all = Resources.LoadAll<Sprite>(imageName);
        // navigation into all sprite
        foreach (var s in all) {
            // check good sprite name
            if (s.name == spriteName) {
                return s;
            }
        }
        // if no good name return null
        return null;
    }

    /* create the text on a button */
    private void createtext(string scene, GameObject button, string txt, int sizetxt, Vector2 size, string font, Color col, int funcid)
    {
        bool sys_font = false;
        string[] all_sys_font = Font.GetOSInstalledFontNames();
        // check if is a system font beacause different load
        foreach (var s in all_sys_font) {
            if (s == font) {
                sys_font = true;
                break;
            }
        }
        // load the font for text
        Font f = (sys_font) ? Resources.GetBuiltinResource(typeof(Font), font + ".ttf") as Font : Resources.Load(font, typeof(Font)) as Font;
        // generate text
        GameObject txts = new GameObject();
        // set the parent of text to button
        txts.transform.SetParent(button.transform);
        // set name of text
        txts.name = "text_" + button.name;
        // add the text module
        txts.AddComponent<Text>();
        // set position of text box to center of button
        txts.transform.position = new Vector3(button.transform.position.x, button.transform.position.y, button.transform.position.z);
        // set size of box for text
        txts.GetComponent<RectTransform>().sizeDelta = new Vector2(button.GetComponent<RectTransform>().sizeDelta.x, button.GetComponent<RectTransform>().sizeDelta.y);
        // set the text, font, size, color and alignement of text
        txts.GetComponent<Text>().text = txt;
        txts.GetComponent<Text>().font = f;
        txts.GetComponent<Text>().color = col;
        txts.GetComponent<Text>().fontSize = sizetxt;
        txts.GetComponent<Text>().alignment = TextAnchor.MiddleCenter; ;
        // initiate the id of utilization for funct
        button.GetComponent<Button>().onClick.AddListener(delegate { Scene.change_scene(funcid, txts, scene, button); });
    }

    /* create a button from arg given as parameter */
    public GameObject CreateButton(string scene, string button_name, string sprite_path, string sprite_name, int funcid, Transform panel, Vector3 position, Vector2 size, string txt, int sizetxt, string font, Color col)
    {
        // load sprite from spritesheet
        Sprite sp = Load(sprite_path, sprite_name);
        // check if sprite is corectly loaded
        if (sp == null)
            Debug.LogWarning("sp == null for sp path = " + sprite_path + " and sp name = " + sprite_name);
        // generate button
        GameObject button = new GameObject();
        // set name of button
        button.name = button_name;
        // set the parent (direct link example a canvas)
        button.transform.parent = panel;
        // add the rect/button/script/image module
        button.AddComponent<RectTransform>();
        button.AddComponent<Button>();
        button.AddComponent<button_acceuil>();
        button.AddComponent<Image>();
        // disable script to avoid loop inf
        button.GetComponent<button_acceuil>().enabled = false;
        // set the position of button with given parameter
        button.transform.position = position;
        // set size of button
        button.GetComponent<RectTransform>().sizeDelta = size;
        // set sprite to button
        if (sp != null) {
            button.GetComponent<Image>().sprite = sp;
        }
        // add the event for click on button
        createtext(scene, button, txt, sizetxt, size, font, col, funcid);
        return (button);
    }

    /* create button in function of *.button file */
    public void create_button(string script_path)
    {
        // init struct
        var test = Parser.parsing(script_path);
        // verify if struct is empty
        if (test == null)
            return;
        // create all button
        foreach (var s in test)
        {
            CreateButton(s.scene, s.button_name, s.sprite_path, s.sprite_name, s.funcid, s.panel, s.position, s.size, s.txt, s.sizetxt, s.font, s.col);
        }
    }

    /* check if 2 string is same */
    public bool match(string str1, string str2)
    {
        if (str1.Length != str2.Length)
            return (false);
        for (int i = 0; i < str1.Length; i++)
            if (str1[i] != str2[i])
                return (false);
        return (true);
    }
}
