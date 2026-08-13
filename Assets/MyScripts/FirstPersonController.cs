using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movimiento")]
    public float flySpeed = 5.0f;

    [Header("Cámara y Mouse")]
    public Transform playerCamera;
    public float mouseSensitivity = 200.0f;
    public float topClamp = -80.0f;
    public float bottomClamp = 80.0f;

    private CharacterController controller;
    private float xRotation = 0.0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Bloquear y ocultar el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // --- 1. ROTACIÓN CON EL MOUSE ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        // Rotar cámara arriba / abajo
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotar cuerpo izquierda / derecha
        transform.Rotate(Vector3.up * mouseX);

        // --- 2. MOVIMIENTO LIBRE (SIN GRAVEDAD) ---
        float moveX = Input.GetAxis("Horizontal"); // A / D
        float moveZ = Input.GetAxis("Vertical");   // W / S

        // Mover relativo a la orientación del jugador
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Aplicar el movimiento directo
        controller.Move(move * flySpeed * Time.deltaTime);
    }
}