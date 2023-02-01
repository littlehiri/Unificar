using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Es una clase de tipo "Helper", un script que nos sirve de apoyo pero no depende de MonoBehaviour
public class GridHelperBuscaminas : MonoBehaviour
{
    //Variables para conocer el ancho y alto de la rejilla
    public static int w = 15; //ancho de la malla       static significa que solo habrá por ejemplo en este caso un ancho y un alto de rejilla en todo el juego
    public static int h = 15; //alto de la malla
    //Un array donde guardar todas las celdas de nuestro juego
    public static Cell[,] cells = new Cell[w, h]; //al ser static también me permite acceder a esto desde otro script

    //Método para destapar todas las minas
    public static void UncoverAllTheMines()
    {
        //Bucle para recorrer el array de celdas y que vaya destapando las minas que haya en esta rejilla
        foreach (Cell c in cells)
        {
            //Si esa celda tiene una mina
            if (c.hasMine)
            {
                //Llamar al método que carga la textura de la mina
                c.LoadTexture(0);
            }
        }
    }

    //Método para saber si hay una mina en una posición dada
    public static bool HasMineAt(int x, int y)//La posición de la celda
    {
        //Si estas condiciones se cumplen estaremos dentro de la rejilla
        if (x >= 0 && y >= 0 && x < w && y < h)
        {
            //Vemos que celda hemos seleccionado y guardamos su posición en una variable de tipo celda
            Cell cell = cells[x, y];
            //De esa celda nos devolverá su booleano, si es true habrá mina, si es false no
            return cell.hasMine;
        }
        //Si las condiciones de arriba no se cumplen estamos fuera de la rejilla
        else
        {
            //No hay posibilidad de que haya una mina
            return false;
        }
    }

    //Método para contar las minas adyacentes a una celda (minas alrededor de una celda dada)
    public static int CountAdjacentMines(int x, int y) //La posición de la celda
    {
        //Contador de minas
        int count = 0;

        //8 casos adyacentes en los que contaríamos una mina si la hubiese
        //Usaremos el método para saber si hay una mina en una posición dada(celda dada)
        if (HasMineAt(x + 1, y)) count++; //medio-derecha
        if (HasMineAt(x - 1, y)) count++; //medio-izquierda
        if (HasMineAt(x, y + 1)) count++; //medio-arriba
        if (HasMineAt(x, y - 1)) count++; //medio-abajo
        if (HasMineAt(x + 1, y + 1)) count++; //arriba-derecha
        if (HasMineAt(x - 1, y + 1)) count++; //arriba-izquierda
        if (HasMineAt(x + 1, y - 1)) count++; //abajo-derecha
        if (HasMineAt(x - 1, y - 1)) count++; //abajo-izquierda

        //Una vez hechas las comprobaciones devolvemos el número de minas
        return count;
    }

    //Este método levanta una posición en X e Y
    public static void FloodFillUncover(int x, int y, bool[,] visited) //Le pasamos una posición X e Y, y tenemos un array de booleanos para saber si una celda ya ha sido visitada antes en una posición X e Y
    {
        //Solo debemos proceder si el punto (x, y)  es válido (está dentro de la rejilla)
        if (x >= 0 && y >= 0 && x < w && y < h)
        {
            //Si ya he pasado por esta celda, el algoritmo de FFU no debe hacer nada
            if (visited[x, y])
            {
                //Salimos del método si se cumple la condición
                return;
            }
            //Si estoy aquí es que la celda no había sido visitada
            //Y entonces cuento el número de minas adyacentes a mi posición (x, y)
            int adjacentMines = CountAdjacentMines(x, y);
            //Muestro en la celda, el número de minas adyacentes(desde 0 hasta 8 máximo)
            cells[x, y].LoadTexture(adjacentMines);
            //Si tengo minas adyacentes, no puedo seguir destapando desde esa celda
            if (adjacentMines > 0)
            {
                //Salimos del método si se cumple la condición
                return;
            }

            //Si esta celda no ha sido visitada y tampoco tiene minas adyacentes
            //Marcamos la celda actual como visitada
            visited[x, y] = true;
            //Visito todas las celdas vecinas en Conectividad4 de la celda actual
            FloodFillUncover(x - 1, y, visited); //Visitamos izquierda
            FloodFillUncover(x + 1, y, visited); //Visitamos derecha
            FloodFillUncover(x, y - 1, visited); //Visitamos abajo
            FloodFillUncover(x, y + 1, visited); //Visitamos arriba
            //Si queremos implementar Conectividad8 faltarían por ver las esquinas
            FloodFillUncover(x - 1, y - 1, visited); //Visitamos abajo-izquierda
            FloodFillUncover(x - 1, y + 1, visited); //Visitamos arriba-izquierda
            FloodFillUncover(x + 1, y - 1, visited); //Visitamos abajo-derecha
            FloodFillUncover(x + 1, y + 1, visited); //Visitamos arriba-derecha
        }    
    }

    //Método para detectar si el juego ha terminado devolviéndome verdadero o falso
    public static bool HasTheGameEnded()
    {
        //Bucle para recorrer el array de celdas 
        foreach (Cell c in cells)
        {
            //Ver si no hay paneles sin descubrir salvo aquellos que contengan minas
            //Si hay celdas cubiertas y sin mina el juego no ha terminado
            if (c.IsCovered() && !c.hasMine)
            {
                //El juego no ha terminado, devolvemos false
                return false;
            }
        }
        //Si no quedan celdas por destapar, salvo las que contengan minas, entonces el juego ha acabado
        return true;
    } //Este método cuando devuelva un valor, no seguirá ejecutándose, saldrá del método

}
