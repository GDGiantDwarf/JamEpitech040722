using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_placement : MonoBehaviour
{
    private bool is_placement = false;
    private bool is_movement = false;
    [Range(1.0f, 20f)]
    public float movespd = 10f;
    [Range(0.1f, 1f)]
    public float deplabatspd = 0.35f;
    private string Name = null;
    private bool pass = false;
    public Loadermap loadersaver;
    private info_plaer infoplayer;
    public GameObject act;
    private float cost_bat_act = 0f;
    public parser_arg_bat.args_stru act_prop;
    private first_game_handle fgh;

    void Start()
    {
        loadersaver = GameObject.Find("ScriptLoader").GetComponent<Loadermap>();
        infoplayer = GameObject.Find("ScriptLoader").GetComponent<info_plaer>();
        fgh = GameObject.Find("ScriptLoader").GetComponent<first_game_handle>();
    }

    public void placement_bat(int id)
    {
        foreach (var bats in loadersaver.parser_bat_prop.stru_gen) {
            if (bats.id == id) {
                if (!infoplayer.strugen.as_monney(bats.value)) {
                    return;
                }
                act_prop = bats;
                cost_bat_act = bats.value;
                is_placement = true;
                Vector3 pos = Camera.main.transform.TransformPoint(Vector3.forward * 10);
                pos.y = 0;
                act = Instantiate(bats.obj, pos, Quaternion.identity);
                act.name = id.ToString();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pass = false;
                return;
            }
        }
    }

    public void check_tag(string tag)
    {
        Name = (tag == "null") ? null : tag;
    }

    public void deplacement_bat()
    {
        if (is_placement && (Input.GetAxis("Mouse X") != 0.0f || Input.GetAxis("Mouse Y") != 0.0f)) {
            if (Camera.main == null) {
                Debug.LogError("camera not present in scene");
                return;
            }
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            // Debug.Log((Camera.main.transform.eulerAngles.y < 90) ? "nord est" : (Camera.main.transform.eulerAngles.y < 180) ? "sud est" : (Camera.main.transform.eulerAngles.y < 270) ? "sud ouest" : "nord ouest");
            if (x > 0) {
                act.transform.Translate(10 * ((Camera.main.transform.eulerAngles.y < 90) ? 1 : (Camera.main.transform.eulerAngles.y < 180) ? -1 : (Camera.main.transform.eulerAngles.y < 270) ? -1 : 1) * Time.deltaTime, 0, 0);
            }
            if (y > 0) {
                act.transform.Translate(0, 0, -10 * ((Camera.main.transform.eulerAngles.y < 90) ? 1 : (Camera.main.transform.eulerAngles.y < 180) ? -1 : (Camera.main.transform.eulerAngles.y < 270) ? -1 : 1) * Time.deltaTime);
            }
            if (x < 0) {
                act.transform.Translate(-10 * ((Camera.main.transform.eulerAngles.y < 90) ? 1 : (Camera.main.transform.eulerAngles.y < 180) ? -1 : (Camera.main.transform.eulerAngles.y < 270) ? -1 : 1) * Time.deltaTime, 0, 0);
            }
            if (y < 0) {
                act.transform.Translate(0, 0, 10 * ((Camera.main.transform.eulerAngles.y < 90) ? 1 : (Camera.main.transform.eulerAngles.y < 180) ? -1 : (Camera.main.transform.eulerAngles.y < 270) ? -1 : 1) * Time.deltaTime);
            }
        }
    }

    private void Update()
    {
        if (fgh.is_paused) {
            return;
        }
        if (Name != null) {
            if (Input.GetMouseButtonDown(0)) {
                placement_bat(int.Parse(Name));
                Name = null;
            }
        }
        // Debug.Log(Camera.main.transform.rotation);
        deplacement_bat();
        if (is_placement && Input.GetMouseButtonUp(0)) {
            collidechecker collides = act.GetComponent<collidechecker>();
            if (collides == null) {
                Debug.LogError("collides not found");
            }
            if (collides.is_triger == true) {
                Destroy(act);
            } else {
                infoplayer.strugen.RemoveMoney(cost_bat_act);
                // collides.remove_triger_effect();
            }
            // if (collides.is_triger) {
            //     Destroy(act);
            // } else {
            //     collides.remove_triger_effect();
            //     act.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            // }
            is_placement = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}