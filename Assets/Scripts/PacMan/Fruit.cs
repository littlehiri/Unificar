using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    //Tiempo especifico de fruta
    public float fruitTime;

    //Método para que la fruta sea recogida
    private void OnTriggerEnter2D(Collider2D collision)
    {
       //Si el objeto que se ha metido donde está la fruta es el jugador
       if (collision.CompareTag("Player"))
        {
            //LLamamos al método del GameManager que inicializa el contador de tiempo de invencibilidad
            GameManagerPacMan.sharedInstance.MakeInvincibleFor(15.0f);
            //Eliminamos la fruta
            Destroy(gameObject);
            GameManagerPacMan.sharedInstance.GetComponent<AudioSource>().Play();
        }
    }
}
