using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManMovement : MonoBehaviour
{
    public GameManagerPacMan referencia;

    //Velocidad de PacMan
    public float speed = 0.0f;
    //Destino al que ir, que al inicio será el (0, 0)
    Vector2 destination = Vector2.zero;

    public GameObject objeto;

    public GameObject maze;

    // Start is called before the first frame update
    void Start()
    {
        //Asignamos a ese destination la posición inicial donde se encuentra PacMan al empezar el juego
        destination = this.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //MoveTowards es moverse hacia. Y le decimos desde dónde, hacia dónde, y a que velocidad
        Vector2 newPos = Vector2.MoveTowards(this.transform.position, destination, speed);
        //Usamos el Rigidbody para mandar a PacMan a dicha posición
        GetComponent<Rigidbody2D>().MovePosition(newPos);

        //Metemos en la variable DistanciaHaciaElDestino la distancia entre la que estamos y hacia la que vamos
        float distanceToDestination = Vector2.Distance((Vector2)this.transform.position, destination);

        //Si PacMan se ha movido hacia el destino
        //if((Vector2)this.transform.position == destination)//Con Vector2 transformo un Vector3 en un Vector2
        //Si esta distancia, ya que nos movemos de 1 en 1 con PacMan es < 1 podemos movernos
        if(distanceToDestination < 1)
        { 
            //Si pulso la tecla arriba y puedo moverme hacia arriba
            if(Input.GetKey(KeyCode.UpArrow) && CanMoveTo(Vector2.up))//Vector2.up <> (0, 1)
            {
                destination = (Vector2)this.transform.position + Vector2.up;
            }
            //Si pulso la tecla derecha y puedo moverme hacia la derecha
            if (Input.GetKey(KeyCode.RightArrow) && CanMoveTo(Vector2.right)) 
            {
                destination = (Vector2)this.transform.position + Vector2.right;
            }
            //Si pulso la tecla abajo y puedo moverme hacia abajo
            if (Input.GetKey(KeyCode.DownArrow) && CanMoveTo(Vector2.down))
            {
                destination = (Vector2)this.transform.position + Vector2.down;
            }
            //Si pulso la tecla izquierda y puedo moverme hacia la izquierda
            if (Input.GetKey(KeyCode.LeftArrow) && CanMoveTo(Vector2.left))
            {
                destination = (Vector2)this.transform.position + Vector2.left;
            }
        }
        //Con esto sabremos hacia donde está apuntando PacMan, sacaremos una X y una Y
        Vector2 dir = destination - (Vector2)this.transform.position;
        //Dependiendo del valor de dir.x, hacemos que PacMan se vea hacia la izquierda o hacia la derecha
        GetComponent<Animator>().SetFloat("DirX", dir.x); //Accedemos al Animator de PacMan y aplicando un cambio en su parámetro DirX, conseguimos el cambio de animación
        //Dependiendo del valor de dir.y. hacemos que PacMan se vea hacia arriba o hacia abajo
        GetComponent<Animator>().SetFloat("DirY", dir.y); //Accedemos al Animator de PacMan y aplicando un cambio en su parámetro DirY, conseguimos el cambio de animación
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si el objeto contra el que ha chocado está etiquetado como Player(PacMan)
        if (collision.tag == "PhantomB")
        {
            GetComponent<AudioSource>().Play();
            if (referencia.invincibleTime > 0)
            {
                Destroy(objeto);
                maze.GetComponent<AudioSource>().Play();
            }
        }
    }

    /* Método que dada una posible dirección de movimiento
     * devuelve true si podemos ir en dicha direccion
     * y false si algo nos impide avanzar*/
    private bool CanMoveTo(Vector2 dir)
    {
        //Metemos la posición de PacMan en un Vector2(x, y)
        Vector2 pacmanPos = this.transform.position;
        //Trazamos un Raycast desde la posición a la que queremos ir hacia PacMan
        RaycastHit2D hit = Physics2D.Linecast(pacmanPos + dir, pacmanPos);

        //El collider de PacMan
        Collider2D pacmanCollider = GetComponent<Collider2D>();
        //El collider contra el que he chocado
        Collider2D hitCollider = hit.collider;

        //Si esta comparación es true es que el collider contra el que he chocado es el de PacMan
        if(hitCollider == pacmanCollider)
        {
            //No tengo nada más en medio -> puedo moverme a esa posición
            return true;
        }
        else
        {
            //Tengo un collider delante que no es el de PacMan, no puedo moverme allí
            return false;
        }
    }
    public void Pacmandead()
    {
        StartCoroutine(Pacmandeadco());
    }
    public IEnumerator Pacmandeadco()
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
        
    }
}
