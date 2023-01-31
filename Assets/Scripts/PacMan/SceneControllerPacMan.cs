using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //Para cambiar entre escenas

public class SceneControllerPacMan : MonoBehaviour
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
    public void PacManScene()
    {
        //Cargamos la escena que se llama así
        SceneManager.LoadScene("PacMan");
    }
}

