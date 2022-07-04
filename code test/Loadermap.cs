using UnityEngine;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public class Loadermap : MonoBehaviour
{
    public struct args_stru {
        public int id;
        public Vector3 position;
        public Vector3 rotation;
        public args_stru(int i, Vector3 positio, Vector3 rotatio) {
            id = i;
            position = positio;
            rotation = rotatio;
        }
    }
    public parser_arg_bat parser_bat_prop;
    args_stru[] strugen;

    void parser_bat(string path)
    {
        string cont = parser_bat_prop.open_read(path, false);
        int nbr_button = parser_bat_prop.my_count(cont, '{');
        int nbr_button_c = parser_bat_prop.my_count(cont, '}');
        cont = cont.Replace("\t", "");
        if (nbr_button != nbr_button_c)
        {
            Debug.LogError("close all button before launch");
            return;
        }
        args_stru[] test = new args_stru[nbr_button];
        string[] all = cont.Split('\n');
        string[] args = new string[2];
        int location = 0;
        for (int i = 0; i < nbr_button; i++) {
            test[i] = new args_stru(0, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            while (!all[location].Replace("\n", "").Replace("\r", "").Contains("}") && !all[location].Replace("\n", "").Replace("\r", "").Contains("}{")) {
                args = all[location].Split(':');
                switch (args[0].ToLower()) {
                    case "id":
                        test[i].id = int.Parse(args[1]);
                        break;
                    case "rotation":
                        string[] temp_tab = args[1].Split(';');
                        for (int u = 0; u < temp_tab.Length; u++) {
                            switch (temp_tab[u].ToLower()) {
                                case "x":
                                    test[i].rotation.x = float.Parse(temp_tab[u + 1].Trim('\r', '\n', '\0').Replace(".", ","));
                                    break;
                                case "y":
                                    test[i].rotation.y = float.Parse(temp_tab[u + 1].Trim('\r', '\n', '\0').Replace(".", ","));
                                    break;
                                case "z":
                                    test[i].rotation.z = float.Parse(temp_tab[u + 1].Trim('\r', '\n', '\0').Replace(".", ","));
                                    break;
                            }
                        }
                        break;
                    case "position":
                        string[] temp_tabb = args[1].Split(';');
                        for (int u = 0; u < temp_tabb.Length; u++) {
                            switch (temp_tabb[u].ToLower()) {
                                case "x":
                                    test[i].position.x = float.Parse(temp_tabb[u + 1].Trim('\r', '\n', '\0').Replace(".", ","));
                                    break;
                                case "y":
                                    test[i].position.y = float.Parse(temp_tabb[u + 1].Trim('\r', '\n', '\0').Replace(".", ","));
                                    break;
                                case "z":
                                    test[i].position.z = float.Parse(temp_tabb[u + 1].Trim('\r', '\n', '\0').Replace(".", ","));
                                    break;
                            }
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
        strugen = test;
    }

    void place_bat(args_stru args)
    {
        GameObject prefab = null;
        for(int u = 0; u < parser_bat_prop.stru_gen.Length; u++) {
            if (args.id == parser_bat_prop.stru_gen[u].id) {
                prefab = parser_bat_prop.stru_gen[u].obj;
                prefab.name = args.id.ToString();
                break;
            }
        }
        prefab = Instantiate(prefab, args.position, new Quaternion(args.rotation.x, args.rotation.y, args.rotation.z, 0f));
        prefab.name = args.id.ToString();
    }

    public void Saver_Map(string path, string path_s)
    {
        GameObject[][] objects = new GameObject[2][]{GameObject.FindGameObjectsWithTag("tree"), GameObject.FindGameObjectsWithTag("bat")};
        string message = "";
        foreach (var trees in objects) {
            foreach (var tree in trees) {
                message += "{\n" + "\t\trotation:\t\tx;\t\t" + tree.transform.rotation.x + ";\t\ty;\t\t" + tree.transform.rotation.y + ";\t\tz;\t\t" + tree.transform.rotation.z + ";\n" + "\t\tposition:\t\tx;\t\t" + tree.transform.position.x + ";\t\ty;\t\t" + tree.transform.position.y + ";\t\tz;\t\t" + tree.transform.position.z + ";\n" + "\t\tid:\t\t" + tree.name + "\n}";
            }
        }
        if (!File.Exists(path + path_s + ".txt")) {
            using (FileStream doc = new FileStream(path + path_s + ".txt", FileMode.Create)) {
            }
            using (StreamWriter writer = new StreamWriter(path + path_s + ".txt", true)) {
                writer.WriteLine(message);
            }
        } else {
            using (FileStream docs = new FileStream(path + path_s + ".txt", FileMode.Truncate)) {
            }
            using (StreamWriter writer = new StreamWriter(path + path_s + ".txt", true)) {
                writer.WriteLine(message);
            }
        }
    }

    void Start()
    {
        parser_opt parseropt;
        gameObject.AddComponent<parser_opt>();
        parseropt = gameObject.GetComponent<parser_opt>();
        parseropt.parsing("option/properties");
        parser_bat_prop = GameObject.Find("ScriptLoader").GetComponent<parser_arg_bat>();
        parser_bat_prop.parse_arg_prop_bat("game/batiment_" + parseropt.stru_gen.language);
        parser_bat("game/game");
        for (int u = 0; u < strugen.Length; u++) {
            place_bat(strugen[u]);
        }
    }
}
