using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketMovement : MonoBehaviour
{
    //Velocidad de la raqueta 
    public float racketSpeed = 25;
    //El eje que quiero usar
    public string axis = "Horizontal";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Ponemos FixedUpdate para que la longitud de cada frame en segundos mida lo mismo, y así el movimiento sea suavizado
    void FixedUpdate()
    {
        //Obtenemos el valor del eje asignado
        float h = Input.GetAxis(axis);
        //Debug.Log(v);
        //Accedemos al componente del RigidBody del objeto donde está metido el script y le aplicamos una velocidad en Y
        GetComponent<Rigidbody2D>().velocity = new Vector2(h, 0) * racketSpeed;//Multiplicamos por la velocidad de movimiento => 1*25 ó -1*25
    }
}
