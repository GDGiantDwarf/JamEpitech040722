using UnityEngine;

public class load_properties : MonoBehaviour
{
    public button_acceuil Button_acceuil;
    public parser_opt Parser;
    void Start() {
        var arg = Parser.parsing("option/properties");
        Button_acceuil.create_button(string.Format("option/option_{0}", arg.language));
    }
}