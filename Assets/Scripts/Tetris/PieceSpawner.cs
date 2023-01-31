using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    //Array donde guardamos los objetos que ser�n spawneados
    public GameObject[] levelPieces;

    //Variables para saber la pieza que tenemos y la siguiente que nos dar� el juego
    public GameObject currentPiece, nextPiece;

    // Start is called before the first frame update
    void Start()
    {
        //Sacamos la siguiente pieza para tenerla visible
        nextPiece = Instantiate(levelPieces[0], this.transform.position, Quaternion.identity);
        //Sacar� una pieza al azar al empezar el juego
        SpawnNextPiece();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //M�todo para spawnear piezas
    public void SpawnNextPiece()
    {
        //Al ir a spawnear la siguiente la que ten�amos guardada pasa a ser la actual, por lo que activamos su script para poder moverla
        //Iniciamos la pieza actual a la siguiente
        currentPiece = nextPiece;
        //Activo esa pieza (su script para que esta funcione)
        currentPiece.GetComponent<Piece>().enabled = true;

        //Para cada bloque dentro de esa pieza
        foreach (SpriteRenderer child in this.gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            //Cogemos el color actual de ese bloque
            Color currentColor = child.color;
            //Hacemos algo transparente a ese bloque
            currentColor.a = 1.0f; //La transparencia va entre 0 y 1
            child.color = currentColor;
        }

        //Lamamos a la corutina que prepara la siguiente pieza
        StartCoroutine("PrepareNextPiece");
    }

    //Corrutina para preparar la siguiente pieza
    IEnumerator PrepareNextPiece()
    {
        //Esperamos antes de nada 0.1 segundos
        yield return new WaitForSecondsRealtime(0.1f);

        //Tomamos un valor aleatorio comprendido en la longitud de todo el array
        int i = Random.Range(0, levelPieces.Length); //Random.Range(valor m�s bajo, valor m�s alto)

        //Instanciamos la pieza seleccionada en la posici�n del objeto spawneador
        nextPiece = Instantiate(levelPieces[i], this.transform.position, Quaternion.identity); //Quaternion.identity es la rotaci�n que tuviera en ese momento el objeto spawneador
        //Instantiate(objeto a instanciar, posici�n en la que se instancia, rotaci�n con la que se instancia)
        //Desactivamos para que la siguiente pieza no se mueva su script
        nextPiece.GetComponent<Piece>().enabled = false;
        //Para cada bloque dentro de esa pieza
        foreach (SpriteRenderer child in this.gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            //Cogemos el color actual de ese bloque
            Color currentColor = child.color;
            //Hacemos algo transparente a ese bloque
            currentColor.a = 0.3f; //La transparencia va entre 0 y 1
            child.color = currentColor;
        }

    }
}
