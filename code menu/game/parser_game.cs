using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public class parser_game : MonoBehaviour
{

    /* struct for args */
    public struct arg_cr_button
    {
        public int money;

        // déclaration de recupération d'initialisateur
        public arg_cr_button(int mone)
        {
            money = mone;
        }
    }

    public arg_cr_button stru_gen;
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
        foreach (var c in str)
            if (c == comp)
                nbr_c++;
        return (nbr_c);
    }

    /* parse all arguments of *.button file */
    public arg_cr_button parsing(string path)
    {
        string cont = open_read(path);
        int nbr_line = my_count(cont, '\n');
        // init an array of struct
        arg_cr_button test = new arg_cr_button();
        // parse argument on all new line
        string[] all = cont.Split('\n');
        string[] args = new string[2];
        int location = 0;
        for (int i = 0; i < nbr_line; i++)
        {
            // parse all line tu get the name of section and the arg
            args = all[location].Split(':');
            switch (args[0])
            {
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