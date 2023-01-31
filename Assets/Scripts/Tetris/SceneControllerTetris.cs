using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //Para cambiar entre escenas

public class SceneControllerTetris : MonoBehaviour
{
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

    }

    //Método cuando hagamos click en el botón
    public void TetrisScene()
    {
        //Cargamos la escena que se llama así
        SceneManager.LoadScene("Tetris");
    }
}

