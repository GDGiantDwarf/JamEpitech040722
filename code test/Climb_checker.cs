using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb_checker : MonoBehaviour
{
    public bool is_climbing = false;
    // private character player;
    public float Climb_force = 6f;

    private void Start()
    {
        // player = GameObject.Find("character").GetComponent<character>();
        Checkforclimb();
    }

    public void Checkforclimb()
    {
        Vector3 origin = transform.position;
        origin.y += 1.4f;
        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.forward, out hit, 1)) {
            initforclimb(hit);
        }
    }

    private void initforclimb(RaycastHit hit)
    {
        is_climbing = true;
    }
}