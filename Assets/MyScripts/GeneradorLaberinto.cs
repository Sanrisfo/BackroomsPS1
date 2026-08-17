using Unity.VisualScripting;
using UnityEngine;

public class GeneradorLaberinto : MonoBehaviour
{
    [Header("Punto de Inicio")]
    [Tooltip("Si está activo, usará las coordenadas de abajo. Si está desactivado, usará la posición de este GameObject en la escena.")]
    public bool usarCoordenadasPersonalizadas = false;
    public Vector3 puntoInicio = Vector3.zero;

    [Header("Dimensiones de la Cuadrícula")]
    public int anchoX = 10; //cantidad de cuartos
    public int largoZ = 10;
    public float tamanoModulo = 4.0f; //tamaño del cuarto

    [Header("Catálogo de modulos")]
    public GameObject[] prefabsHabitaciones;

    [Header("Opciones")]
    public bool rotacionAleatoria = true;
    public Transform contenedorPadre;

    void Start()
    {
        GenerarMapa();
    }

    //Esto es para probarlo sin darle al play
    [ContextMenu("Generar mapa ahora")]
    public void GenerarMapa()
    {
        LimpiarMapa();

        if (prefabsHabitaciones == null || prefabsHabitaciones.Length == 0){
            Debug.LogWarning("No se asignaron prefabs");
            return;
        }
        if (contenedorPadre == null){
            GameObject nuevoContenedor = new GameObject("LaberintoGen");
            contenedorPadre = nuevoContenedor.transform;
        }

        // Determinar el origen desde donde comienza el laberinto
        Vector3 origen = usarCoordenadasPersonalizadas ? puntoInicio : transform.position;

        for (int x=0; x<anchoX; x++){
            for (int z=0; z<largoZ; z++){
                
                //Definir la posicion donde se instanciará el prefab en el entorno 3D
                Vector3 posicion = new Vector3(
                    origen.x + (x * tamanoModulo),
                    origen.y,
                    origen.z + (z * tamanoModulo)
                );

                //Elegir un prefab aleatorio
                int indiceRandom = Random.Range(0, prefabsHabitaciones.Length);
                GameObject prefabElegido = prefabsHabitaciones[indiceRandom];

                //Calcular rotación cuadrantal (si la hay)
                Quaternion rotacion = Quaternion.identity;
                if (rotacionAleatoria)
                {
                    int anguloY = Random.Range(0, 4) * 90;
                    rotacion = Quaternion.Euler(0f, anguloY, 0f);
                }

                //Instanciar en la escena
                Instantiate(prefabElegido, posicion, rotacion, contenedorPadre);
            }
        }
    }

    [ContextMenu("Limpiar mapa")]
    public void LimpiarMapa()
    {
        if (contenedorPadre != null)
        {
            while(contenedorPadre.childCount > 0)
            {
                DestroyImmediate(contenedorPadre.GetChild(0).gameObject);
            }
        }
    }

}
