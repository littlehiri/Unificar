using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PacDots : MonoBehaviour
{
   
    public GameManagerPacMan referencia;
    //Método para conocer cuando un objeto se ha metido en la zona de trigger de los PacDots

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si el objeto que ha entrado en el trigger está etiquetado como Player
        if (collision.tag == "Player")
        {
            

            //Podría sumar puntos
            GameManagerPacMan.sharedInstance.Puntos += 100;

            GetComponent<AudioSource>().Play();

            GameManagerPacMan.sharedInstance.Score.text = GameManagerPacMan.sharedInstance.Puntos.ToString();
            //Recogemos el objeto PacDot concreto, en nuestro caso lo eliminamos
            Destroy(this.gameObject);
        }
    }


}
