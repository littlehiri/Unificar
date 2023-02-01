using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Es una clase de tipo "Helper", un script que nos sirve de apoyo pero no depende de MonoBehaviour
public class GridHelperBuscaminas : MonoBehaviour
{
    //Variables para conocer el ancho y alto de la rejilla
    public static int w = 15; //ancho de la malla       static significa que solo habr� por ejemplo en este caso un ancho y un alto de rejilla en todo el juego
    public static int h = 15; //alto de la malla
    //Un array donde guardar todas las celdas de nuestro juego
    public static Cell[,] cells = new Cell[w, h]; //al ser static tambi�n me permite acceder a esto desde otro script

    //M�todo para destapar todas las minas
    public static void UncoverAllTheMines()
    {
        //Bucle para recorrer el array de celdas y que vaya destapando las minas que haya en esta rejilla
        foreach (Cell c in cells)
        {
            //Si esa celda tiene una mina
            if (c.hasMine)
            {
                //Llamar al m�todo que carga la textura de la mina
                c.LoadTexture(0);
            }
        }
    }

    //M�todo para saber si hay una mina en una posici�n dada
    public static bool HasMineAt(int x, int y)//La posici�n de la celda
    {
        //Si estas condiciones se cumplen estaremos dentro de la rejilla
        if (x >= 0 && y >= 0 && x < w && y < h)
        {
            //Vemos que celda hemos seleccionado y guardamos su posici�n en una variable de tipo celda
            Cell cell = cells[x, y];
            //De esa celda nos devolver� su booleano, si es true habr� mina, si es false no
            return cell.hasMine;
        }
        //Si las condiciones de arriba no se cumplen estamos fuera de la rejilla
        else
        {
            //No hay posibilidad de que haya una mina
            return false;
        }
    }

    //M�todo para contar las minas adyacentes a una celda (minas alrededor de una celda dada)
    public static int CountAdjacentMines(int x, int y) //La posici�n de la celda
    {
        //Contador de minas
        int count = 0;

        //8 casos adyacentes en los que contar�amos una mina si la hubiese
        //Usaremos el m�todo para saber si hay una mina en una posici�n dada(celda dada)
        if (HasMineAt(x + 1, y)) count++; //medio-derecha
        if (HasMineAt(x - 1, y)) count++; //medio-izquierda
        if (HasMineAt(x, y + 1)) count++; //medio-arriba
        if (HasMineAt(x, y - 1)) count++; //medio-abajo
        if (HasMineAt(x + 1, y + 1)) count++; //arriba-derecha
        if (HasMineAt(x - 1, y + 1)) count++; //arriba-izquierda
        if (HasMineAt(x + 1, y - 1)) count++; //abajo-derecha
        if (HasMineAt(x - 1, y - 1)) count++; //abajo-izquierda

        //Una vez hechas las comprobaciones devolvemos el n�mero de minas
        return count;
    }

    //Este m�todo levanta una posici�n en X e Y
    public static void FloodFillUncover(int x, int y, bool[,] visited) //Le pasamos una posici�n X e Y, y tenemos un array de booleanos para saber si una celda ya ha sido visitada antes en una posici�n X e Y
    {
        //Solo debemos proceder si el punto (x, y)  es v�lido (est� dentro de la rejilla)
        if (x >= 0 && y >= 0 && x < w && y < h)
        {
            //Si ya he pasado por esta celda, el algoritmo de FFU no debe hacer nada
            if (visited[x, y])
            {
                //Salimos del m�todo si se cumple la condici�n
                return;
            }
            //Si estoy aqu� es que la celda no hab�a sido visitada
            //Y entonces cuento el n�mero de minas adyacentes a mi posici�n (x, y)
            int adjacentMines = CountAdjacentMines(x, y);
            //Muestro en la celda, el n�mero de minas adyacentes(desde 0 hasta 8 m�ximo)
            cells[x, y].LoadTexture(adjacentMines);
            //Si tengo minas adyacentes, no puedo seguir destapando desde esa celda
            if (adjacentMines > 0)
            {
                //Salimos del m�todo si se cumple la condici�n
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
            //Si queremos implementar Conectividad8 faltar�an por ver las esquinas
            FloodFillUncover(x - 1, y - 1, visited); //Visitamos abajo-izquierda
            FloodFillUncover(x - 1, y + 1, visited); //Visitamos arriba-izquierda
            FloodFillUncover(x + 1, y - 1, visited); //Visitamos abajo-derecha
            FloodFillUncover(x + 1, y + 1, visited); //Visitamos arriba-derecha
        }    
    }

    //M�todo para detectar si el juego ha terminado devolvi�ndome verdadero o falso
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
    } //Este m�todo cuando devuelva un valor, no seguir� ejecut�ndose, saldr� del m�todo

}
