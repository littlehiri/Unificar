using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Block : MonoBehaviour
{

    //Marcador
    public GameManager referencia;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*Método para saber cuando algo choca contra el bloque, en nuestro caso
    //el único objeto que se mueve por la pantalla es la bola, luego solamente 
    puede ser ella la que choque contra los bloques*/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Detectamos que sea la pelota el objeto contra el que hemos colisionado
        if(collision.gameObject.name == "Ball")
        {
            //Destruimos el objeto bloque concreto contra el que ha chocado la pelota
            Destroy(this.gameObject);

            //suma
            referencia.Puntos+=5;

            referencia.Score.text = referencia.Puntos.ToString();
        }
    }
}
