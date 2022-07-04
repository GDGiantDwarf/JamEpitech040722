using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public class parser : MonoBehaviour
{

    /* struct for args */
    public struct arg_cr_button
    {
        public string button_name;
        public string txt;
        public string font;
        public string sprite_path;
        public string sprite_name;
        public string scene;
        public int funcid;
        public int sizetxt;
        public Transform panel;
        public Vector3 position;
        public Vector2 size;
        public Color col;

        // déclaration de recupération d'initialisateur
        public arg_cr_button(string button_nam, string sprite_pat, string sprite_nam, string tx, string fon, string scen, int funci, int sizetx, Transform panl, Vector3 positin, Vector2 siz, Color co)
        {
            button_name = button_nam;
            txt = tx;
            font = fon;
            funcid = funci;
            sizetxt = sizetx;
            panel = panl;
            position = positin;
            size = siz;
            col = co;
            sprite_path = sprite_pat;
            sprite_name = sprite_nam;
            scene = scen;
        }
    }

    /* open and read a document and return a string */
    public string open_read(string path)
    {
        TextAsset mytxtData = (TextAsset)Resources.Load(path);
        // AssetDatabase.Refresh();
        if (mytxtData == null) {
            Debug.LogError("Could not Find " + path);
            return (null);
        }
        string txt = mytxtData.text;
        Resources.UnloadAsset(mytxtData);
        return (txt);
        // open the file
        // using (FileStream fs = File.Open(path, FileMode.Open))
        // {
        //     byte[] b = new byte[fs.Length + 1];
        //     string Sa = "";
        //     UTF8Encoding temp = new UTF8Encoding(true);
        //     // get all char of the file
        //     while (fs.Read(b, 0, b.Length) > 0)
        //         Sa += temp.GetString(b);
        //     return (Sa);
        // }
    }

    /* count the occurence of a specific char */
    public int my_count(string str, char comp)
    {
        int nbr_c = 0;
        if (str == null) {
            return (0);
        }
        foreach (var c in str)
            if (c == comp)
                nbr_c++;
        return (nbr_c);
    }

    /* parse all arguments of *.button file */
    public arg_cr_button[] parsing(string path)
    {
        string cont = open_read(path);
        int nbr_button = my_count(cont, '{');
        int nbr_button_c = my_count(cont, '}');
        // check if all of the button container are corectly closed
        if (nbr_button != nbr_button_c)
        {
            Debug.LogError("close all button before launch");
            return (null);
        }
        // init an array of struct
        arg_cr_button[] test = new arg_cr_button[nbr_button];
        // parse argument on all new line
        string[] all = cont.Split('\n');
        string[] args = new string[2];
        int location = 0;
        // loop until i not egal to the number of button
        for (int i = 0; i < nbr_button; i++)
        {
            // init all struc with null or 0 to avoid a miss argument
            test[i] = new arg_cr_button(null, null, null, null, null, null, 0, 0, transform, new Vector3(0, 0, 0), new Vector2(0, 0), Color.black);
            // ruen while not the end of a button container
            while (!all[location].Replace("\n", "").Replace("\r", "").Contains("}") && !all[location].Replace("\n", "").Replace("\r", "").Contains("}{"))
            {
                // parse all line tu get the name of section and the arg
                args = all[location].Split(':');
                switch (args[0])
                {
                    case "id":
                        test[i].funcid = int.Parse(args[1].Trim('\n', '\r', '\0'));
                        break;
                    case "size_txt":
                        test[i].sizetxt = (int)(float.Parse(args[1].Replace(".", ",")) * (Screen.width / 1980f));
                        break;
                    case "pos_x":
                        test[i].position.x = transform.position.x + float.Parse(args[1].Replace(".", ",")) * (Screen.width / 1980f);
                        test[i].position.z = 0;
                        break;
                    case "pos_y":
                        test[i].position.y = transform.position.y + float.Parse(args[1].Replace(".", ",")) * (Screen.height / 1080f);
                        break;
                    case "width":
                        test[i].size.x = float.Parse(args[1].Replace(".", ",")) * (Screen.width / 1980f);
                        break;
                    case "heigth":
                        test[i].size.y = float.Parse(args[1].Replace(".", ",")) * (Screen.height / 1080f);
                        break;
                    case "name":
                        test[i].button_name = args[1];
                        break;
                    case "txt":
                        test[i].txt = args[1];
                        break;
                    case "font":
                        test[i].font = args[1].Replace("\n", "").Replace("\r", "");
                        break;
                    case "spr_path":
                        test[i].sprite_path = args[1].Replace("\n", "").Replace("\r", "");
                        break;
                    case "spr_name":
                        test[i].sprite_name = args[1].Replace("\n", "").Replace("\r", "");
                        break;
                    case "color_txt":
                        test[i].col = Color.clear;
                        ColorUtility.TryParseHtmlString(args[1].Replace("\n", "").Replace("\r", ""), out test[i].col);
                        break;
                    case "panel":
                        test[i].panel = (args[1].Trim('\n', '\r', '\0') == "transform") ? transform : GameObject.Find(args[1].Trim('\n', '\r', '\0')).transform;
                        break;
                    case "scene":
                        test[i].scene = args[1].Replace("\n", "").Replace("\r", "");
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
        return (test);
    }
}