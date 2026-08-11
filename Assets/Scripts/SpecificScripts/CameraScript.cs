using Unity.Mathematics;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public bool canMove = true;

    [SerializeField] private float xSencibility;
    [SerializeField] private float ySencibility;

    public Transform player;

    private float xRotation = 0;
    private float yRotation = 0;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if(!canMove) return;

        float xValue = Input.GetAxis("Mouse X") * xSencibility * Time.deltaTime;
        float yValue = Input.GetAxis("Mouse Y") * ySencibility * Time.deltaTime;

        xRotation += xValue;
        yRotation -= yValue;

        yRotation = Mathf.Clamp(yRotation, -80, 80);

        transform.localRotation = Quaternion.Euler(yRotation, 0, 0);

        player.Rotate(0, xSencibility * xValue, 0);
    }
}
