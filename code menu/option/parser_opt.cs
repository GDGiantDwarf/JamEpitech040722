using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public class parser_opt : MonoBehaviour
{
    /* struct for args */
    public struct arg_cr_settings
    {
        // déclaration de recupération d'initialisateur
        public string path;
        public string music_path;
        public string language;
        public bool fulscreen;
        public bool mute;
        public float sound;
        public float music;
        public arg_cr_settings(string pat, string music_pat, string languag, bool fulscree, bool mut, int soun, int musi)
        {
            language = languag;
            fulscreen = fulscree;
            mute = mut;
            sound = soun;
            music = musi;
            path = pat;
            music_path = music_pat;
        }
    }

    public arg_cr_settings stru_gen;
    /* open and read a document and return a string */
    public string open_read(string path)
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
                return (Sa);
            }
        }
    }

    /* count the occurence of a specific char */
    public int my_count(string str, char comp)
    {
        int nbr_c = 0;
        foreach (var c in str)
            if (c == comp)
                nbr_c++;
        return (nbr_c);
    }

    private string rm_last_arg(string pat) {
        int u = 0;
        int nbr_slash = my_count(pat, '/');
        string final_arg = "";
        foreach (var c in pat) {
            if (c == '/') {
                u++;
            }
            if (u == nbr_slash) {
                break;
            }
            final_arg += c;
        }
        return (final_arg);
    }

    private string get_last_arg(string pat) {
        int u = pat.Length - 1;
        string final_arg = "";
        for (; u > 0 && pat[u] != '/'; u--) {
        }
        for (; u < pat.Length; u++) {
            final_arg += pat[u];
        }
        return (final_arg);
    }

    public void save_opt()
    {
        string path = rm_last_arg(Application.persistentDataPath + "/" + stru_gen.path);
        string path_s = get_last_arg(Application.persistentDataPath + "/" + stru_gen.path);
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
        string message = "";
        string Debugs = "";
        message += "music:" + stru_gen.music + "\n";
        message += "sound:" + stru_gen.sound + "\n";
        message += "fullscreen:" + stru_gen.fulscreen + "\n";
        message += "language:" + stru_gen.language + "\n";
        message += "mute:" + stru_gen.mute + "\n";
        message += "music_path:" + stru_gen.music_path + "\n";
        if (!File.Exists(path + path_s + ".txt")) {
            using (FileStream doc = new FileStream(path + path_s + ".txt", FileMode.Create)) {
                if (doc == null) {
                    Debugs += "doc == null\n message == " + message + "\n";
                }
            }
            using (StreamWriter writer = new StreamWriter(path + path_s + ".txt", true)) {
                writer.WriteLine(message);
            }
        } else {
            using (FileStream docs = new FileStream(path + path_s + ".txt", FileMode.Truncate)) {
                if (docs == null) {
                    Debugs += "doc == null\n message == " + message + "\n";
                }
            }
            using (StreamWriter writer = new StreamWriter(path + path_s + ".txt", true)) {
                writer.WriteLine(message);
            }
        }
    }

    /* parse all arguments of *.button file */
    public arg_cr_settings parsing(string path)
    {
        string cont = open_read(path);
        int nbr_line = my_count(cont, '\n');
        // init an array of struct
        arg_cr_settings test = new arg_cr_settings(path, null, "en", true, false, 50, 50);
        // parse argument on all new line
        string[] all = cont.Split('\n');
        string[] args = new string[2];
        int location = 0;
        // loop until i not egal to the number of button
        for (int i = 0; i <= nbr_line; i++)
        {
            // parse all line tu get the name of section and the arg
            args = all[location].Split(':');
            switch (args[0])
            {
                case "music_path":
                    test.music_path = args[1].Trim('\r', '\n', '\0');
                    break;
                case "music":
                    test.music = float.Parse(args[1].Replace('.', ','));
                    break;
                case "sound":
                    test.sound = float.Parse(args[1].Replace('.', ','));
                    break;
                case "fullscreen":
                    if (args[1].ToLower() == "false")
                        test.fulscreen = false;
                    break;
                case "language":
                    test.language = args[1].Trim('\r', '\n', '\0');
                    break;
                case "mute":
                    if (args[1].Trim('\r', '\n', '\0').ToLower() == "false")
                        test.mute = false;
                    else
                        test.mute = true;
                    break;
                default:
                    if (!Regex.IsMatch(args[0].Replace("\n", "").Replace("\r", ""), @"^[\p{L}]+$"))
                        break;
                    Debug.LogError("Error line non assigned : " + all[location]);
                    break;
            }
            location++;
        }
        stru_gen = test;
        return (test);
    }
}