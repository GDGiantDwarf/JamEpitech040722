using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class info_plaer : MonoBehaviour
{
    public float waiting_for_save = 60f;
    public float next_time = 0f;
    public string path_save = "";
    public string path_folder = "";
    private bool is_load = false;
    public struct Player_info {
        public float money;
        public int days_past;
        public float time_of_day;
        public string name;
        public Player_info(float mone, int days_pas, float time_of_da, string nam) {
            money = mone;
            days_past = days_pas;
            time_of_day = time_of_da;
            name = nam;
        }

        public void RemoveMoney(float monney_to_remove)
        {
            money -= monney_to_remove;
        }

        public bool as_monney(float to_check)
        {
            if (to_check > money)
                return (false);
            return (true);
        }
    }

    public parser_arg_bat parser_bat_prop;
    public Player_info strugen;
    public popup Popup;

    private void parse_player_info(string path) {
        string cont = parser_bat_prop.open_read(path, false);
        int nbr_line = parser_bat_prop.my_count(cont, '\n');
        Player_info test = new Player_info(0, 0, 0, "debug");
        string[] all = cont.Split('\n');
        string[] args = new string[2];
        int location = 0;
        for (int i = 0; i <= nbr_line; i++)
        {
            args = all[location].Split(':');
            switch (args[0].ToLower().Trim('\t'))
            {
                case "money":
                    test.money = float.Parse(args[1].Trim('\n', '\0', '\r', '\t').Replace('.', ','));
                    break;
                case "day_past":
                    test.days_past = int.Parse(args[1].Trim('\n', '\0', '\r', '\t'));
                    break;
                case "time_past":
                    test.time_of_day = float.Parse(args[1].Trim('\n', '\0', '\r', '\t').Replace('.', ','));
                    break;
                case "name":
                    test.name = args[1].Trim('\n', '\0', '\r', '\t');
                    break;
                default:
                    if (!Regex.IsMatch(args[0].Replace("\n", "").Replace("\r", ""), @"^[\p{L}]+$"))
                        break;
                    Debug.LogError("Error line non assigned : " + all[location]);
                    break;
            }
            location++;
        }
        strugen = test;
    }

    public bool as_monney(float to_check)
    {
        if (to_check > strugen.money)
            return (false);
        return (true);
    }

    public void save_player_info(string path, string path_s)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        string message = "";
        string Debug = "";
        message += string.Format("name\t:\t{0}\nmoney\t:\t{1}\ntime_past\t:\t{2}\nday_past\t:\t{3}", strugen.name, strugen.money.ToString(), strugen.time_of_day.ToString(), strugen.days_past.ToString());
        if (!File.Exists(path + path_s + ".txt")) {
            using (FileStream doc = new FileStream(path + path_s + ".txt", FileMode.Create)) {
                if (doc == null) {
                    Debug += "doc == null\n message == " + message + "\n";
                }
            }
            using (StreamWriter writer = new StreamWriter(path + path_s + ".txt", true)) {
                writer.WriteLine(message);
            }
        } else {
            using (FileStream docs = new FileStream(path + path_s + ".txt", FileMode.Truncate)) {
                if (docs == null) {
                    Debug += "doc == null\n message == " + message + "\n";
                }
            }
            using (StreamWriter writer = new StreamWriter(path + path_s + ".txt", true)) {
                writer.WriteLine(message);
            }
        }
    }

    private void Start()
    {
        path_save = "/Player1";
        path_folder = "game";
        parser_bat_prop = GameObject.Find("ScriptLoader").GetComponent<parser_arg_bat>();
        parse_player_info(path_folder + path_save);
        is_load = true;
    }

    private void Update()
    {
        if (Time.time > next_time && is_load == true) {
            next_time = Time.time + waiting_for_save;
            save_player_info(Application.persistentDataPath + "/" + path_folder, path_save);
        }
    }
}