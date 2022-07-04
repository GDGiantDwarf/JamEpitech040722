using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class state : MonoBehaviour
{
    public GameObject death_menu;
    public Toggle is_retry_auto;
    void OnTriggerEnter(Collider other) {
        switch (other.gameObject.tag) {
            case "death":
                if (!is_retry_auto.isOn)
                    death_menu.SetActive(true);
                else
                    SceneManager.LoadScene(sceneName:SceneManager.GetActiveScene().name);
                break;
            case "Finish":
                Debug.Log("win");
                break;
            default:
                break;
        }
    }
}
