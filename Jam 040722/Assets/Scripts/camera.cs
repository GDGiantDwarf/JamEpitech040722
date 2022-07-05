using UnityEngine;

public class camera : MonoBehaviour
{
    private GameObject player;
    private GameObject bar;
    public GameObject camerafollow;
    public float rotation_speed = 120.0f;
    public float sensitivity = 150.0f;
    [Range(0.1f, 1f)]
    public float smoothCamera;
    public float Clampangle = 80.0f;
    [Range(1f, 2f)]
    public float maxdistance = 2f;
    [Range(0.2f, 1f)]
    public float mindistance = 0.2f;
    static float tampondist = 1f;
    public Vector2 mousepos;
    private Vector2 rot;
    private Vector3 init;
    public string[] inputs = {"z", "q", "s", "d"};
    public bool followplayer = true;

    void Start()
    {
        Vector3 rotv = transform.localRotation.eulerAngles;
        rot.y = rotv.y;
        rot.x = rotv.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player = GameObject.Find("character");
    }

    bool enable_cursor()
    {
        if (!followplayer)
            return (true);
        /* if (Input.GetKey(KeyCode.LeftAlt) && !Cursor.visible) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt) && Cursor.visible) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }*/
        if (Cursor.visible) {
            return (true);
        } else {
            return (false);
        }
    }

    void Rotate_handle()
    {
        mousepos.x = Input.GetAxis("Mouse X");
        mousepos.y = Input.GetAxis("Mouse Y");
        rot.y += mousepos.x * sensitivity * Time.deltaTime;
        rot.x += mousepos.y * sensitivity * Time.deltaTime;

        rot.x = Mathf.Clamp(rot.x, -Clampangle, Clampangle);
        Quaternion localRotation = Quaternion.Euler(rot.x, rot.y, 0.0f);
        transform.rotation = localRotation;
    }

    void Zoom_handle()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel") + 1;
        Camera camera = GameObject.Find("Camera").GetComponent<Camera>();

        tampondist *= wheel;
        if (tampondist > maxdistance) {
            tampondist = maxdistance;
            wheel = 1f;
        } else if (tampondist < mindistance) {
            tampondist = mindistance;
            wheel = 1f;
        }
        camera.fieldOfView = camera.fieldOfView * wheel;
    }

    void Update()
    {
        if (!enable_cursor()) {
            Rotate_handle();
            if (Input.GetAxis("Mouse ScrollWheel") != 0) {
                Zoom_handle();
            }
        }
    }

    void CameraUpdater()
    {
        Transform target = camerafollow.transform;

        float step = rotation_speed * Time.deltaTime * smoothCamera;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

    void freeCamera()
    {
        float speed = player.GetComponent<character>().runsp;
        if (Input.GetKey(inputs[0])) { // front
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
        if (Input.GetKey(inputs[1])) { // rot left
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(inputs[2])) { // back
            transform.Translate(0, 0, -speed * Time.deltaTime);
        }
        if (Input.GetKey(inputs[3])) { // rot right
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetMouseButton(1)) {
            Rotate_handle();
        }
    }

    void LateUpdate() {
        if (followplayer) {
            CameraUpdater();
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void FixedUpdate() {
        if (!followplayer) {
            freeCamera();
        }
    }
}