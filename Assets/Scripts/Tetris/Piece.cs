using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    //Última caída(bajada) de la pieza hace 0 segundos
    float lastFall = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Movimiento horizontal de las piezas
        //Movimiento de la ficha a la izquierda
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Llamamos al método de movimiento horizontal y le pasamos la dirección izquierda
            MovePieceHorizontally(-1);
        }
        //Movimiento de la ficha a la derecha
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //Llamamos al método de movimiento horizontal y le pasamos la dirección derecha
            MovePieceHorizontally(1);
        }
        //Rotación de la pieza
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Roto la pieza hacia la derecha
            this.transform.Rotate(0, 0, -90);

            //Si la posición es válida
            if (IsValidPiecePosition())
            {
                //Actualizamos la rejilla, guardando la nueva posición en el GridHelper
                UpdateGrid();
            }
            //Si la posición no fuese válida
            else
            {
                //Revierto la rotación hacia el lado contrario(izquierdo)
                this.transform.Rotate(0, 0, 90);
            }
        }
        //Mover la pieza hacia abajo al pulsar la tecla o cuando haya pasado más de un segundo desde la última vez que se movió
        else if (Input.GetKeyDown(KeyCode.DownArrow) || (Time.time - lastFall) > 1.0f)
        {
            //Muevo la pieza hacia abajo una posición
            this.transform.position += new Vector3(0, -1, 0);

            // Si la posición es válida
            if (IsValidPiecePosition())
            {
                //Actualizamos la rejilla, guardando la nueva posición en el GridHelper
                UpdateGrid();
            }
            //Si la posición no fuese válida
            else
            {
                //Revierto el movimiento hacia abajo sumando uno hacia arriba
                this.transform.position += new Vector3(0, 1, 0);
                //Si ya no pudiese bajar más, habría que detectar si es momento de borrar una fila
                GridHelper.DeleteAllFullRows();
                //Hacemos que aparezca una pieza nueva, llamando al PieceSpawner a su método
                FindObjectOfType<PieceSpawner>().SpawnNextPiece();//Busca un objeto de ese tipo para poder usar sus métodos y variables
                //Deshabilitamos este script para que esta pieza no vuelva a moverse
                this.enabled = false;
            }
            //Reiniciamos el contador de tiempo
            lastFall = Time.time;
        }
        //Cuento en milisegundos cuanto ha pasado desde la última caída
        lastFall += Time.deltaTime;
    }

    //Método para el movimiento horizontal
    void MovePieceHorizontally(int direction) //con direction, le pasamos un número para saber si el movimiento es a izquierda o a derecha
    {
        //Muevo la pieza en la dirección dada
        this.transform.position += new Vector3(direction, 0, 0);
        //Comprobamos si la nueva posición es válida
        if (IsValidPiecePosition())
        {
            //Actualizamos la rejilla, guardando la nueva posición en el GridHelper
            UpdateGrid();
        }
        //Si la posición no es válida
        else
        {
            //Revertimos el movimiento a la posición en la que estaba antes
            this.transform.position += new Vector3(-direction, 0, 0);
        }
    }

    //Método que comprueba si la posición en la que se encuentra ahora mismo la pieza, es o no válida
    private bool IsValidPiecePosition()
    {
        //Hacemos una pasada por todas las posiciones de los hijos de la pieza(los bloques)
        foreach (Transform block in this.transform)
        {
            //Recuperamos su posición (la de los bloques, hijos de la pieza) y la redondeamos para que no tenga decimales
            Vector2 pos = GridHelper.RoundVector(block.position);

            //Si no está dentro de los bordes, la posición no es válida. Es decir, alguno de los bloques de la pieza se sale de los bordes o está encima de ellos
            if (!GridHelper.IsInsideBorders(pos))
            {
                //Si algún bloque de la pieza no está en una posición válida
                return false;
            }

            //Si ya hay otro bloque en esa misma posición, la posición tampoco es válida. 
            //Como la posición podría ser un float(tener decimales), la transformamos en número entero
            Transform possibleObject = GridHelper.grid[(int)pos.x, (int)pos.y];
            //Si ya hay otro objeto y este no es hijo del mismo objeto (osea el bloque que hay es de otra pieza)
            if (possibleObject != null && possibleObject.parent != this.transform)
            {
                //La posición no será valida
                return false;
            }
        }
        //Si ninguna de las cosas anteriormente mencionadas se cumple, será que este bloque o pieza está en una posición válida
        return true;
    }

    //Método que actualiza la rejilla virtual tras mover las piezas o bloques a su nueva posición
    //Lo haremos primero haciendo un borrado de bloques, poniendo primero todo a null, y luego poniendo las posiciones nuevas de esos bloques
    private void UpdateGrid()
    {
        //Comparamos si el padre del objeto coincide con el del bloque estamos mirando
        for (int y = 0; y < GridHelper.h; y++)
        {
            //Después por columnas de cada fila
            for (int x = 0; x < GridHelper.w; x++)
            {
                //Comprobamos si en esa posición no hay un bloque
                if (GridHelper.grid[x, y] != null)
                {
                    //Comprobamos si el padre del bloque es la pieza donde está este script metido
                    if (GridHelper.grid[x, y].parent == this.transform)
                    {
                        //Se carga los bloques que quedan de esa pieza y pone esas posiciones a null
                        GridHelper.grid[x, y] = null;
                    }
                }
            }
            //Insertamos los bloques en las posiciones que deben estar
            //Hacemos una pasada por cada uno de los bloques de la pieza actual
            foreach (Transform block in this.transform)
            {
                //Cojo la posición donde esté cada uno de los hijos y la redondeo
                Vector2 pos = GridHelper.RoundVector(block.position);
                //Metemos esa posición en la posición de la rejilla virtual que le toque
                GridHelper.grid[(int)pos.x, (int)pos.y] = block;
            }
        }
        
    }
}
