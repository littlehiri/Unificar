using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //Velocidad de la bola
    public float speed = 10;

    //Referencia a la posici�n inicial
    public Vector2 ballInit;

    // Start is called before the first frame update
    void Start()
    {
        //Recogemos la posici�n inicial de la pelota
        ballInit = transform.position;
        //La bola se mueve hacia arriba
        //GetComponent<Rigidbody2D>().velocity = Vector2.up * speed; //Vector2.up = new Vector2(0,1)
        RestartBallMovement();
        //Llamamos a la corutina
        StartCoroutine(UpgradeDifficulty());
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
     * El objeto collision del par�ntesis contiene la informaci�n del choque
     * En particular, nos interesa saber cuando choca con una pala.
     * -collision.gameObject: tiene informaci�n del objeto contra el cual he colisionado (raqueta)
     * -collision.transform.position: tiene informaci�n de la posici�n (raqueta)
     * -collision.collider: tiene informaci�n del collider (raqueta)
     */
    /*Es un m�todo de Unity que detecta colisi�n entre dos GO. 
     * Al chocar el objeto contra el que choca le pasa su Colisi�n por par�metro */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Cada vez que choca la pelota contra algo reproduce su sonido
        GetComponent<AudioSource>().Play();
        //Si la pelota ha colisionado con la pala izquierda
        if (collision.gameObject.name == "Racket")
        {
            //Obtenemos el factor de golpeo, pas�ndole la posici�n de la pelota, la posici�n de la pala, y lo que mide de ancho el collider de la pala (es decir, lo que mide la pala)
            float xF = HitFactor(transform.position, collision.transform.position, collision.collider.bounds.size.x);
            /*Le damos una nueva direcci�n a la pala
             * En este caso con una Y hacia arriba
             * Y nuestro factor de golpeo calculado
             * Normalizado todo el vector a 1, para que la bola no acelere*/
            Vector2 direction = new Vector2(xF, 1).normalized;
            //Le decimos a la bola que salga con esa velocidad previamente calculada
            GetComponent<Rigidbody2D>().velocity = direction * speed;
        }
    }

    /*
     * 1 - La bola choca contra la parte m�s a la derecha de la raqueta
     * 0 - La bola choca contra el centro de la raqueta
     * -1 - La bola choca contra la parte m�s a la izquierda de la raqueta
    */
    /*Es un m�todo de tipo 3. En este caso le pasamos 3 par�metros:
     * - posici�n actual de la pelota
     * - posici�n actual de la pala
     * - el ancho de la pala
     * Y el m�todo tal y como le indicamos nos devuelve un variable de tipo float */
    private float HitFactor(Vector2 ballPosition, Vector2 racketPosition, float racketWidth)
    {
        return (ballPosition.x - racketPosition.x) / racketWidth;
    }

    //M�todo para resetear la pelota
    public void ResetBall()
    {
        //Reseteamos la bola a la velocidad inicial que ten�a
        speed = 10f;
        //Paramos la velocidad de la pelota
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //Ponemos a la pelota en su posici�n original
        transform.position = ballInit;
        //Si a�n nos quedan vidas reseteamos el movimiento de la bola y sino no
        if (GameManager.sharedInstance.lives > 0)
        {
            //Esperamos unos segundos y volvemos a decirle a la bola que se mueva
            Invoke("RestartBallMovement", 2.0f);
        }
    }

    //M�todo para relanzar la bola
    private void RestartBallMovement()
    {
        //Le volvemos a dar una velocidad hacia arriba a la bola
        GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
    }

    //Corutina para aumentar la dificultad del juego acelerando la bola
    //La corutina se establece fuera del tiempo del bucle de juego, se usa en acciones que deben hacerse en un momento puntual, independientes de lo dem�s
    private IEnumerator UpgradeDifficulty()
    {
        //Hacemos un bucle siempre verdadero, para que cada segundo aumente un poco la velocidad de la pelota
        while (true)//Mientras la condici�n del bucle se cumpla lo hace. As� le indicamos que la condici�n siempre ser� verdad
        {
            //Hace que la corutina se espere un segundo
            yield return new WaitForSeconds(1.0f);
            //Aumento la velocidad de la bola
            //speed += 0.5f;
            speed *= 1.005f; //Un 0,5% cada vez
        }
    }
}
