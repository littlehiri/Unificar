using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Para poder usar elementos de la interfaz
using TMPro; //Para usar el TextMeshPro
using UnityEngine.SceneManagement; //Para cambiar entre escenas

public class GameManager : MonoBehaviour
{
    //Imágenes de las vidas
    public Image live1, live2, live3;
    //public Image live1;
    //public Image live2;
    //public Image live3;
    //Textos de GameOver y GameWin
    public TextMeshProUGUI gameOver, gameWin;

    public TextMeshProUGUI Score;
    public int Puntos;

    //Empezamos el juego teniendo 3 vidas
    public int lives = 3;

    //Hacemos un Singleton del script GameManager, para poder usar sus propiedades desde cualquier script
    public static GameManager sharedInstance = null;

    private void Awake()
    {
        //Si la instancia del Singleton está vacía 
        if (sharedInstance == null)
        {
            //La rellena con todo el código del GameManager
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Si pulsamos la tecla escape salimos del juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //Controlamos las imágenes de las vidas, dependiendo de cuantas nos quedan
        //Si nos quedan menos de 3 vidas
        //if (lives < 3)
        //{
        //    //Desactivamos la imagen de la vida 3
        //    live3.enabled = false;
        //}
        ////Si nos quedan menos de 2 vidas
        //if (lives < 2)
        //{
        //    //Desactivamos la imagen de la vida 2
        //    live2.enabled = false;
        //}
        ////Si nos quedan menos de 1 vida
        //if (lives < 1)
        //{
        //    //Desactivamos la imagen de la vida 1
        //    live1.enabled = false;
        //}
        //Nos damos cuenta de que al ver el valor de una sola variable, podemos sustituir lo de arriba por un switch
        switch (lives)
        {
            //En el caso en el que las vidas sean 3
            case 3:
                //Activamos la imagen de la vida 3
                live3.enabled = true;
                //Activamos la imagen de la vida 2
                live2.enabled = true;
                //Activamos la imagen de la vida 1
                live1.enabled = true;
                break;
            //En el caso en el que las vidas sean 2
            case 2:
                //Desactivamos la imagen de la vida 3
                live3.enabled = false;
                break;
            case 1:
                //Desactivamos la imagen de la vida 2
                live2.enabled = false;
                break;
            case 0:
                //Desactivamos la imagen de la vida 1
                live1.enabled = false;
                //Activamos la imagen de GameOver
                gameOver.enabled = true;
                break;
            default:
                //Desactivamos la imagen de la vida 3
                live3.enabled = false;
                //Desactivamos la imagen de la vida 2
                live2.enabled = false;
                //Desactivamos la imagen de la vida 1
                live1.enabled = false;
                break;
        }

        //Vamos a contar cuantos bloques hay en esta partida
        //Creamos un array donde meter todos los bloques que tenemos en esta partida
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        //Si el tamaño del array es 0 (osea se ha quedado vacío, no quedan bloques)
        if (blocks.Length == 0)
        {
            //Aparece el letrero de GameWin
            gameWin.enabled = true;
            //Invocamos al método para hacer el cambio de escena tras 2 segundos
            Invoke("GoToNextScene", 2.0f);
        }
    }

    //Método para cambiar entre escenas
    private void GoToNextScene()
    {
        //Cambiamos de escena yendo a la siguiente que tengamos preparada en la Build
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
