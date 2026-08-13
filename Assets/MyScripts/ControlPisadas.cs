using UnityEngine;

public class ControlPisadas : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] sonidosPisadas;

    public float intervaloPisadas = 0.5f; // Ritmo continuo entre pasos
    public float retrasoPrimerPaso = 0.2f; // Tiempo para sonar la primera pisada al presionar tecla

    private float temporizadorPasos = 0f;
    private bool estabaMoviendose = false; // Nos ayuda a detectar el momento preciso en que empieza a caminar

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        // 1. Detectar si estás presionando teclas de movimiento
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");
        bool seEstaMoviendo = (Mathf.Abs(inputX) > 0.1f || Mathf.Abs(inputZ) > 0.1f);

        if (seEstaMoviendo)
        {
            // 2. Si acaba de empezar a moverse este frame (primer toque de tecla)
            if (!estabaMoviendose)
            {
                // Configuramos el temporizador para que le falten exactamente 0.2s para sonar
                temporizadorPasos = Mathf.Max(0f, intervaloPisadas - retrasoPrimerPaso);
                estabaMoviendose = true;
            }

            // 3. Acumulamos tiempo
            temporizadorPasos += Time.deltaTime;

            // 4. Cuando alcanza el intervalo, suena la pisada
            if (temporizadorPasos >= intervaloPisadas)
            {
                ReproducirPisada();
                temporizadorPasos = 0f; // Se reinicia a 0 para mantener el ritmo continuo
            }
        }
        else
        {
            // Si el jugador se detiene, reseteamos las variables de control
            estabaMoviendose = false;
            temporizadorPasos = 0f;
        }
    }

    void ReproducirPisada()
    {
        if (sonidosPisadas == null || sonidosPisadas.Length == 0) return;

        int indiceAleatorio = Random.Range(0, sonidosPisadas.Length);

        // Variación sutil en el tono
        audioSource.pitch = Random.Range(0.9f, 1.1f);

        audioSource.PlayOneShot(sonidosPisadas[indiceAleatorio]);
    }
}