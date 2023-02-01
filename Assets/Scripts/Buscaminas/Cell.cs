using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;   

public class Cell : MonoBehaviour
{
    public GameManagerBuscaminas referencia;
    //Variable que nos permite conocer si un panel tiene una mina o no
    public bool hasMine;
    //Array en el que guardamos todas las imágenes que corresponden a que no hay una mina en esa celda al pulsar sobre ella
    public Sprite[] emptyTextures;
    //Necesitamos la imagen de que hay una mina
    public Sprite mineTexture;

    //Creamos unas variables donde guardar la posición de la celda concreta
    int x, y;

    // Start is called before the first frame update
    void Start()
    {
        //Le decimos que hay un 15% de posibilidades de que haya una mina en esa celda
        hasMine = (Random.value < 0.15);//Random.value nos da un valor entre 0 y 1. Si se cumple en este caso que ese valor sea menor que 0.15, hasMine será verdadero, sino falso
        //Variables para recoger la posición inicial de la celda
        x = (int)this.transform.position.x; //La posición en X de esa celda concreta (la columna)       (int) lo usamos para transformar ese float que nos da el transform.position a número entero
        y = (int)this.transform.parent.position.y; //La posición en Y de esa celda concreta (la fila)
        //Metemos esta celda concreta(this) con esa X e Y que hemos obtenido en la posición X e Y correspondiente de ese array de celdas
        GridHelperBuscaminas.cells[x, y] = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Método de ayuda para saber si la celda ha sido destapada o no devolviéndonos verdadero o falso
    //Comprobamos entonces si la imagen es la de 'panel' o bien otra
    public bool IsCovered()
    {
        //Coge del SpriteRenderer de esa celda la textura que está ahi puesta para saber si es un panel
        //o no. En caso de que sea un panel, será verdadero, y sino falso
        return GetComponent<SpriteRenderer>().sprite.texture.name == "panel";
    }

    //Método para cargar las texturas en las celdas
    public void LoadTexture(int adjacentCount)//para pasarle el conteo de minas adyacentes de una celda
    {
        //Si hay una mina en esa celda
        if (hasMine)
        {
            //Accedemos al SpriteRenderer de esa celda para cambiar su imagen a una de mina
            GetComponent<SpriteRenderer>().sprite = mineTexture;
        }
        //Si no hay mina en esa celda
        else
        {
            //Accedemos al SpriteRenderer de esa celda para cambiar su imagen a una de las que están dentro del array EmptyTextures
            GetComponent<SpriteRenderer>().sprite = emptyTextures[adjacentCount];
        }
    }

    //Método para cuando pulsamos en una celda y soltamos el click del ratón
    private void OnMouseUpAsButton()
    {
        //Si esa celda tiene una mina
        if (hasMine)
        {
            //TO DO:
            //Llamamos al método que descubre todas las minas del juego
            GridHelperBuscaminas.UncoverAllTheMines();
            //mostrar mensaje de Game Over
            GetComponent<AudioSource>().Play();
            referencia.gameFail.SetActive(true);
            Debug.Log("Pringaoooo");
            
        }
        //Si no hay mina en esa celda
        else
        {
            //TO DO:
            //cambiar la textura de la celda
            //Al método de abajo le pasamos la posición de esta celda concreta
            //Cargamos la textura de minas adyacentes adecuada
            LoadTexture(GridHelperBuscaminas.CountAdjacentMines(x, y)); //Usamos el método que cuenta cuantas minas hay alrededor de la celda
            //descubrir toda el área sin minas alrededor de la celda destapada
            //Le pasamos la posición en X e Y de esa celda concreta y vemos en el array de celdas si esta había sido visitada o no
            GridHelperBuscaminas.FloodFillUncover(x, y, new bool[GridHelper.w, GridHelper.h]);
            //comprobar si el juego ha acabado o no
            if (GridHelperBuscaminas.HasTheGameEnded())
            {
                Debug.Log("¡Has ganado! Fin de la partida.");
                referencia.gameWin.SetActive(true);
            }
        }
    }
}
