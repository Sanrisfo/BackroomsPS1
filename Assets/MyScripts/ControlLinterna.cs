using System.Collections;
using UnityEngine;

public class ControlLinterna : MonoBehaviour
{
    //referencia al componente luz
    public Light luzLinterna;

    private bool estaEncendida = true;
    private bool estaParpadeando = false;
  
    void Update()
    {
        //Detectamos la tecla F
        if (Input.GetKeyDown(KeyCode.F))
            {
            //cambiamos el estado de encendido(true) y apagado(false)
            estaEncendida = !estaEncendida;
            luzLinterna.enabled = estaEncendida;
            }
        
        //Si está encendida hay una probabilidad random de empezar a parpadear
        if (estaEncendida && !estaParpadeando)
        {
            //probabilidad de 0.1% por frame
            if (Random.value < 0.001f)
            {
                StartCoroutine(EfectoParpadeo());
            }
        }

        IEnumerator EfectoParpadeo()
        {
            estaParpadeando = true;

            //Apagamos y prendemos en milisegundos randoms
            luzLinterna.enabled = false;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
            luzLinterna.enabled = true;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
            luzLinterna.enabled = true;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
            luzLinterna.enabled = false;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
            luzLinterna.enabled = true;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));

            //Devolvemos el estado original
            luzLinterna.enabled = estaEncendida;
            estaParpadeando = false;
        }
    }
}
