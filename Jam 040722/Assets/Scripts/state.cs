using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class state : MonoBehaviour
{
    public bool isActive;
    public GameObject prefab;
    public Vector3 char_pos;
    public GameObject character;

    void Start()
    {
        isActive = false;
        char_pos = character.transform.position;
    }

    void OnTriggerEnter(Collider other) {
        switch (other.gameObject.tag) {
            case "death":
                isActive = true;
                if (prefab == null) {
                    prefab = Resources.Load("prefab/dead_body") as GameObject;
                }
                GameObject new_cl = Instantiate(prefab, character.transform.position, Quaternion.identity);
                if (character == null) {
                    character = Resources.Load("prefab/character") as GameObject;
                }
                character.transform.position = char_pos;
                break;
            case "Finish":
                Debug.Log("win");
                break;
            default:
                break;
        }
    }
}
