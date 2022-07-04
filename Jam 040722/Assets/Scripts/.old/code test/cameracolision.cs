using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameracolision : MonoBehaviour
{
    public float mindist = 1.0f;
    public float maxdist = 4.0f;
    public float smooth = 10.0f;
    public float distance;
    private Vector3 dollydir;
    public Vector3 dollydiradjust;
    void Awake()
    {
        dollydir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 desiredCamerapos = transform.parent.TransformPoint(dollydir * maxdist);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, desiredCamerapos, out hit)) {
            distance = Mathf.Clamp((hit.distance * 0.45f), mindist, maxdist);
        } else {
            distance = maxdist;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollydir * distance, Time.deltaTime * smooth);
    }
}
