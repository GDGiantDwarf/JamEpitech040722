using UnityEngine;
using System;
using System.Collections.Generic;

public class handle_menu : MonoBehaviour
{
    public parser_arg_bat parser_bat_prop;
    public button_acceuil buttongen;
    public string active_string;
    List<String> cat = new List<String>();
    List<String> obj_name = new List<String>();
    List<GameObject> obj_list= new List<GameObject>();
    List<GameObject> Button_list = new List<GameObject>();
    Vector2 scale = new Vector2((float)((Screen.width / 3840f)), (float)((Screen.height / 2160f)));
    Vector3 position = new Vector3(-1601f + 1920f, -730f + 1080f, 0);
    bool is_first = true;

    void make_cat(Vector2 scale, parser_arg_bat.args_stru bats)
    {
        Vector2 size = new Vector2((float)(500f * scale.x), (float)(100f * scale.y));
        var prefab = Resources.Load("prefab/menu/image") as GameObject;
        var obj = Instantiate(prefab, new Vector3(9f, -900f, 0), Quaternion.identity);
        var panel = GameObject.Find("cat").transform;
        Debug.Log("type = " + bats.type);
        obj.name = bats.type;
        obj.transform.SetParent(transform.parent);
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(obj.GetComponent<RectTransform>().sizeDelta.x * scale.x, obj.GetComponent<RectTransform>().sizeDelta.y * scale.y);
        obj.transform.position = new Vector3(9f * (scale.x) + transform.position.x, -900f * (scale.y) + transform.position.y, 0);
        GameObject.Find("content").name = bats.type + "_cont";
        obj_list.Add(obj);
        buttongen.CreateButton(null, bats.type + "_button", "image/acceuil/button", "button_37", 51, panel, position, size, bats.type, (int)(60f * scale.x), "Arial", Color.black);
        position.x += (size.x + size.x / 4);
    }

    public void active_cat(string to_active)
    {
        GameObject to_end = null;
        bool as_cat = false;
        bool verif_end = false;
        if (to_active.Replace("_button", "") == active_string) {
            return;
        }
        foreach (var obj in obj_list) {
            if (obj.name == active_string) {
                to_end = obj;
                verif_end = true;
                if (as_cat) {
                    break;
                }
            } else if (obj.name == to_active.Replace("_button", "")) {
                obj.SetActive(true);
                as_cat = true;
                if (verif_end) {
                    break;
                }
            }
        }
        if (to_end != null && as_cat) {
            active_string = to_active.Replace("_button", "");
            to_end.SetActive(false);
        }
    }

    void Start()
    {
        parser_opt parseropt;
        gameObject.AddComponent<parser_opt>();
        parseropt = gameObject.GetComponent<parser_opt>();
        parseropt.parsing("option/properties");
        Vector2 size = new Vector2((float)(250f * scale.x), (float)(150f * scale.y));
        parser_bat_prop = GameObject.Find("ScriptLoader").GetComponent<parser_arg_bat>();
        buttongen = GameObject.Find("ScriptLoader").GetComponent<button_acceuil>();
        GameObject obj = null;
        if (parser_bat_prop.stru_gen == null) {
            parser_bat_prop.parse_arg_prop_bat("game/batiment_" + parseropt.stru_gen.language);
        }
        var all_bats = parser_bat_prop.stru_gen;
        foreach(var bats in all_bats) {
            if (!obj_name.Contains(bats.name)) {
                if (!cat.Contains(bats.type)) {
                    cat.Add(bats.type);
                    make_cat(scale, bats);
                }
                obj = (buttongen.CreateButton(bats.id.ToString(), bats.id.ToString(), bats.sprite_path.Trim('\n', '\r'), bats.sprite_name.Trim('\n', '\r'), 50, GameObject.Find(bats.type + "_cont").transform, new Vector3(0, 0, 0), size, null, 0, null, Color.black));
                obj_name.Add(bats.name);
                obj.AddComponent<click_event>();
                Button_list.Add(obj);
            }
        }
        foreach (var s in obj_list) {
            if (!is_first) {
                s.SetActive(false);
            } else {
                active_string = s.name;
                is_first = false;
            }
        }
    }
}
