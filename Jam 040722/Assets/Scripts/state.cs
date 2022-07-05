using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class state : MonoBehaviour
{
    public GameObject death_menu;
    public bool isActive;
    public Toggle is_retry_auto;

    void Start()
    {
        isActive = false;
    }

    void OnTriggerEnter(Collider other) {
        switch (other.gameObject.tag) {
            case "death":
                isActive = true;
                if (!is_retry_auto.isOn && isActive == true)
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
