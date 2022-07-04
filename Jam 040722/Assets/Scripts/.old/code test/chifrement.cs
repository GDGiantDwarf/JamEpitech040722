using UnityEngine;

public class chifrement : MonoBehaviour
{
    private string Key = "8F54ceT5P1N00M47vTmkO356zA";

    //if encrypted return clear value else return encrypted value
    public string encdecrypt(string data)
    {
        string message = "";
        if (data == null) {
            return (null);
        }
        for (int i = 0; i < data.Length; i++) {
            message += (char)(data[i] ^ Key[i % Key.Length]);
        }
        return (message);
    }
}