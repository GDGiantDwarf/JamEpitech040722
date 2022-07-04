using UnityEngine;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;

public class parser_arg_bat : MonoBehaviour
{
    public struct args_stru {
        public int id;
        public int value;
        public string sprite_path;
        public string sprite_name;
        public string name;
        public string type;
        public string description;
        public GameObject obj;
        public args_stru(int i, int valu, string typ, string sprite_pat, string nam, string sprite_nam, string descriptio, GameObject objs) {
            id = i;
            value = valu;
            sprite_path = sprite_pat;
            name = nam;
            description = descriptio;
            obj = objs;
            sprite_name = sprite_nam;
            type = typ;
        }
    }

    public chifrement chifre;
    public args_stru[] stru_gen;

    private void Start() {
        chifre = GameObject.Find("ScriptLoader").GetComponent<chifrement>();
    }

    public string open_read(string path, bool is_encrypt)
    {
        if (!File.Exists(Application.persistentDataPath + "/" + path + ".txt")) {
            TextAsset mytxtData = (TextAsset)Resources.Load(path);
            // AssetDatabase.Refresh();
            if (mytxtData == null) {
                Debug.LogError("Could not Find " + path);
                return (null);
            }
            string txt = mytxtData.text;
            Resources.UnloadAsset(mytxtData);
            if (is_encrypt) {
                if (chifre == null) {
                    Debug.LogError("Gameobject not found chiffre");
                    return (txt);
                }
                return (chifre.encdecrypt(txt));
            }
            return (txt);
        } else {
            // open the file
            using (FileStream fs = File.Open(Application.persistentDataPath + "/" + path + ".txt", FileMode.Open)) {
                byte[] b = new byte[fs.Length + 1];
                string Sa = "";
                UTF8Encoding temp = new UTF8Encoding(true);
                // get all char of the file
                while (fs.Read(b, 0, b.Length) > 0)
                    Sa += temp.GetString(b);
                if (is_encrypt) {
                    if (chifre == null) {
                        Debug.LogError("Gameobject not found chiffre");
                        return (Sa);
                    }
                    return (chifre.encdecrypt(Sa));
                }
                return (Sa);
            }
        }
    }

    public int my_count(string str, char comp)
    {
        int nbr_c = 0;
        foreach (var c in str)
            if (c == comp)
                nbr_c++;
        return (nbr_c);
    }

    public args_stru[] parse_arg_prop_bat(string path)
    {
        string cont = open_read(path, false);
        int nbr_button = my_count(cont, '{');
        int nbr_button_c = my_count(cont, '}');
        cont = cont.Replace(" ", "");
        if (nbr_button != nbr_button_c)
        {
            Debug.LogError("close all button before launch");
            return (null);
        }
        args_stru[] test = new args_stru[nbr_button];
        string[] all = cont.Split('\n');
        string[] args = new string[2];
        int location = 0;
        for (int i = 0; i < nbr_button; i++)
        {
            test[i] = new args_stru(0, 0, null, null, null, null, null, null);
            while (!all[location].Replace("\n", "").Replace("\r", "").Contains("}") && !all[location].Replace("\n", "").Replace("\r", "").Contains("}{"))
            {
                args = all[location].Split(':');
                switch (args[0].ToLower())
                {
                    case "id":
                        test[i].id = int.Parse(args[1]);
                        break;
                    case "name":
                        test[i].name = args[1].Replace(",", " ");
                        break;
                    case "value":
                        test[i].value = int.Parse(args[1]);
                        break;
                    case "sprite":
                        test[i].sprite_path = args[1];
                        break;
                    case "description":
                        test[i].description = args[1].Replace(",", " ").Trim('\r', '\n', '\0');
                        break;
                    case "sprite_name":
                        test[i].sprite_name = args[1].Trim('\r', '\n', '\0');
                        break;
                    case "type":
                        test[i].type = args[1].Trim('\r', '\n', '\0');
                        break;
                    case "prefab":
                        test[i].obj = Resources.Load(args[1].Trim('\r', '\n', '\0')) as GameObject;
                        if (test[i].obj == null) {
                            Debug.LogError("Gameobject not Load for " + args[1].Trim('\r', '\n', '\0'));
                        }
                        break;
                    default:
                            if (!Regex.IsMatch(args[0].Replace("\n", "").Replace("\r", ""), @"^[\p{L}]+$"))
                                break;
                            Debug.LogError("Error line non assigned : " + all[location]);
                        break;
                }
                location++;
            }
            location++;
        }
        stru_gen = test;
        return (test);
    }

    public void print_stru(args_stru[] stru)
    {
        foreach (var item in stru) {
            Debug.Log(string.Format("[id:{0}, name:{1}, width:{2}, heigth:{3}, value:{4}, sprite:{5}, description:{6}]", item.id, item.name, item.value, item.sprite_path, item.description));
        }
    }
}
