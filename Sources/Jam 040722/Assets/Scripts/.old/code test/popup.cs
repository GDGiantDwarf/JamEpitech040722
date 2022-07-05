using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popup : MonoBehaviour
{
    public Text text;
    void Start()
    {
    }

    public void Set_Debug_Text(string to_apply)
    {
        text.text = to_apply;
        text.color = Color.red;
    }
}
