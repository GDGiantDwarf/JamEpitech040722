using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class click_event : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public test_placement place_bat = null;
    public GameObject Text_Box = null;
    public GameObject textText_Box = null;
    private first_game_handle fgh;
    public bool is_hover = false;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (fgh.is_paused) {
            return;
        }
        Vector3 Mousepos = Input.mousePosition;
        RectTransform rect = Text_Box.GetComponent<RectTransform>();
        is_hover = true;
        foreach (var bat in place_bat.loadersaver.parser_bat_prop.stru_gen) {
            if (bat.id == int.Parse(gameObject.name)) {
                textText_Box.GetComponent<Text>().text = string.Format("Name: {0}\nDescriptin: \"{1}\"\nPrice: {2}$", bat.name, bat.description, bat.value.ToString());
                break;
            }
        }
        RectTransform rect_text = textText_Box.GetComponent<RectTransform>();
        Text_Box.GetComponent<RectTransform>().sizeDelta = new Vector3(rect.sizeDelta.x, rect_text.sizeDelta.y + 200, 0f);
        Text_Box.transform.position = new Vector3(Mousepos.x + (rect.sizeDelta.x * Screen.width / 3840) / 2, Mousepos.y + (rect.sizeDelta.y * Screen.height / 2160) + 30, 0f);
        Text_Box.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        is_hover = false;
        Text_Box.SetActive(false);
    }

    private void Start()
    {
        fgh = GameObject.Find("ScriptLoader").GetComponent<first_game_handle>();
        place_bat = GameObject.Find("ScriptLoader").GetComponent<test_placement>();
        foreach (GameObject box in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) {
            if (box.name == "TextBox") {
                Text_Box = box;
            } else if (box.name == "text_TextBox") {
                textText_Box = box;
            }
        }
        if (place_bat != null) {
            Debug.Log("correctly init place_bat");
        } else {
            Debug.LogError("cant load place_bat");
        }
        if (Text_Box == null) {
            Debug.LogError("no txt box");
        }
        Text_Box.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (fgh.is_paused) {
            return;
        }
        if (eventData.button == PointerEventData.InputButton.Left) {
            if (place_bat == null) {
                Debug.LogError("place_bat == null");
                return;
            }
            place_bat.placement_bat(int.Parse(gameObject.name));
        }
    }

    private void Update() {
        if (fgh.is_paused) {
            return;
        }
        if (is_hover) {
            Vector3 newpos = new Vector3((Input.GetAxis("Mouse X") > 0) ? 1 * Time.deltaTime * 4 : (Input.GetAxis("Mouse X") < 0) ? -1 * Time.deltaTime * 4 : 0 * Time.deltaTime * 4, (Input.GetAxis("Mouse Y") > 0) ? -1 * Time.deltaTime * 4 : (Input.GetAxis("Mouse Y") < 0) ? 1 * Time.deltaTime * 4 : 0 * Time.deltaTime * 4, 0f);
            Text_Box.transform.Translate(newpos);
        }
    }
}