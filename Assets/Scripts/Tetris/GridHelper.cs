using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHelper : MonoBehaviour
{
   

    /* Matriz filas y columnas
      |  0  |  1  |  2  |  3  |
   -----------------------------
    0 | null   x     x    null
    1 |  x     x    null  null
    2 | null  null  null  null
    3 | null  null  null  null
     
     */

    //Ancho y alto de la rejilla
    //Son estáticas para poder instanciarlas sin tener que consultar el objeto
    public static int w = 10, h = 18 + 4;
    //Creamos el array doble rejilla, de altura y anchura dada
    public static Transform[,] grid = new Transform[w, h]; //La [,] indica dos dimensiones

    //Método que dado un Vector2 cogerá ese Vector, y redondeará sus coordenadas de X e Y. Tras esto el método nos devuelve el vector redondeado
    public static Vector2 RoundVector(Vector2 v)
    {
        //Devuelve un nuevo Vector2 ya redondeado en X e Y
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y)); //Mathf.Round -> redondea al número entero más próximo
    }

    //Método que dada una posición comprobamos si esta pieza está dentro de los bordes del juego. Nos devolverá si es cierto o no
    public static bool IsInsideBorders(Vector2 pos)
    {
        //Si ambas coordenadas son positivas y no se pasan por la derecha
        if (pos.x >= 0 && pos.y >= 0 && pos.x < w)
        {
            //La pieza está dentro de la zona de juego
            return true;
        }
        //Si lo de arriba no se cumple
        else
        {
            //La pieza está fuera de la zona de juego
            return false;
        }
    }

    //Método al que le pasamos una fila y si hemos comprobado que está completa, la elimina
    public static void DeleteRow(int y)
    {
        //Para poder borrar la fila, vemos cada una de las columnas de la fila actual
        for (int x = 0; x < w; x++)
        {
            //Destruyo el cuadrado que hay en esa posición, el objeto que vemos en la pantalla
            Destroy(grid[x, y].gameObject);
            //Después de destruirlo, el espacio que había reservado en la rejilla virtual, lo vacío.
            //Cambiaríamos las X del dibujo de arriba por una posición vacía (null)
            grid[x, y] = null;

            GameManagerTetris.referencia.Puntos += 100;
            GameManagerTetris.referencia.Score.text = GameManagerTetris.referencia.Puntos.ToString();
            GameManagerTetris.referencia.GetComponent<AudioSource>().Play();

            GameManagerTetris.referencia.Puntos2 += 0.1f;
            GameManagerTetris.referencia.Lineas.text = GameManagerTetris.referencia.Puntos2.ToString();




        }
    }

    //Método que baja una fila a partir de una fila concreta
    public static void DecreaseRow(int y)
    {
        //Para poder bajar la fila, vemos cada una de las columnas de la fila actual
        for (int x = 0; x < w; x++)
        {
            //Si la posición que quiero bajar no está vacía
            if (grid[x, y] != null)
            {
                //Muevo la ficha -1 en la Y, a la posición en la que me encontraba
                grid[x, y - 1] = grid[x, y];
                //Como hemos bajado el bloque en la posición anterior, hacemos null la posición que ahora ha quedado vacía
                grid[x, y] = null;

                //Ahora repintamos en pantalla
                //Repintamos en pantalla el bloque una posición más abajo en la pantalla por cada bloque
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    //Método que baja las filas de arriba a partir de una dada
    public static void DecreaseAbove(int y)
    {
        //Para todas las filas desde la dada
        for (int i = y; i < h; i++)
        {
            //Llamamos al método que baja una fila, pero en este caso irán bajando de una en una, hasta que no queden más
            DecreaseRow(i);
        }
    }

    //Método para saber si una fila está completa, pasándole una fila
    public static bool IsRowFull(int y)
    {
        //Pasamos primero por todas las columnas de esa fila
        for (int x = 0; x < w; x++)
        {
            //Si encuentro algún hueco en esa fila, es que no está llena
            if (grid[x, y] == null)
            {
                //Hay un hueco en la fila, no está completa
                return false;
            }
        }
        //La fila si no se cumple lo de arriba, será que está llena
        return true;
    }

    //Método para borrar varias o todas las filas de golpe
    public static void DeleteAllFullRows()
    {
        //Comprobamos para todas las filas, desde la de más abajo, hasta la de más arriba
        for (int y = 0; y < h; y++)
        {
            //Si la fila que estamos comprobando está llena
            if (IsRowFull(y))
            {
                //Borramos la fila actual
                DeleteRow(y);
                //Al borrar la fila actual, bajamos las que estén por encima
                DecreaseAbove(y + 1);
                //Volveríamos a la fila anterior, es decir, si ya hemos borrado una fila todas bajarán
                //Pero no pasaremos a la siguiente, primero volvemos a comprobar la fila en la que estamos
                y--;
            }
        }

        //Hacemos un borrado de piezas que se hayan quedado vacías
        CleanPieces();
    }

    //Método para limpiar piezas cuando ya no tienen bloques
    private static void CleanPieces()
    {
        //Hacemos una pasada por todos los objetos de tipo pieza que encontramos
        foreach (GameObject piece in GameObject.FindGameObjectsWithTag("Piece"))
        {
            //Si esa pieza ya no tiene bloques
            if (piece.transform.childCount == 0)
            {
                //Destruimos el objeto pieza
                Destroy(piece);
            }
        }
    }
}
