using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collidechecker : MonoBehaviour
{
    public bool is_triger = false;
    public Collider[] collide;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "TERRAIN")
            is_triger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        is_triger = false;
    }
    public void remove_triger_effect()
    {
        collide = gameObject.GetComponents<Collider>();
        foreach (Collider coll in collide) {
            coll.isTrigger = false;
        }
    }
}
