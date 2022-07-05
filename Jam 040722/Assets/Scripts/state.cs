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
                GameObject new_cl = Instantiate(prefab, character.transform.position, Quaternion.identity);
                
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
