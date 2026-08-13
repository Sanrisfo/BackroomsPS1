using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public CharacterController controller;
    public float velocidad = 4.0f;
    public float sensibilidadMouse = 2.0f;
    public float gravedad = -9.81f; // Fuerza para mantener al personaje pegado al suelo

    private float rotacionX = 0f;
    private float velocidadY = 0f;

    void Start()
    {
        // Al inicio obtenemos el CharacterController si no fue asignado
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }

        // Al inicio ocultamos el cursor del mouse y lo bloqueamos
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // --- 1. TOGGLE DEL CURSOR (Tecla L para modo edición) ---
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None; // Libera el ratón
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked; // Vuelve a bloquear la cámara
                Cursor.visible = false;
            }
        }

        // Si el cursor está libre, detemos la rotación y el movimiento
        if (Cursor.lockState != CursorLockMode.Locked) return;


        // --- 2. LÓGICA DE CÁMARA Y MOUSE ---
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);

        Camera.main.transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);


        // --- 3. LÓGICA DE MOVIMIENTO WASD (NORMALIZADO) ---
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Creamos la dirección local
        Vector3 direccionEntrada = new Vector3(x, 0f, z);

        // NORMALIZACIÓN: Si la longitud es mayor a 1 (ej. en diagonal), la ajusta a 1 exacto
        if (direccionEntrada.magnitude > 1f)
        {
            direccionEntrada.Normalize();
        }

        // Convertimos la dirección local a espacio del mundo
        Vector3 mover = transform.right * direccionEntrada.x + transform.forward * direccionEntrada.z;


        // --- 4. GRAVEDAD (Asegura que controller.isGrounded funcione bien) ---
        if (controller.isGrounded && velocidadY < 0)
        {
            velocidadY = -2f; // Mantener una leve presión hacia el suelo
        }

        velocidadY += gravedad * Time.deltaTime;
        mover.y = velocidadY;


        // --- 5. APLICAR EL MOVIMIENTO ---
        controller.Move(mover * velocidad * Time.deltaTime);
    }
}