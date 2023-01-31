using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerPacMan : MonoBehaviour
{
    
    //Variable para controlar el tiempo en el que PacMan es invencible
    public float invincibleTime = 0.0f;
    //Creamos el Singleton del GameManager
    public static GameManagerPacMan sharedInstance;

    public AudioSource Comer;

    public TextMeshProUGUI Score;
    public int Puntos;  

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }
    private void Update()
    {
        //hacemos que el contador de tiempo vaya decreciendo hasta que se vacie 
        //Si el contador a�n no est� vac�o
        if (invincibleTime > 0)
        {
            //Usando el Time.deltaTime le restamos 1 cada segundo al contador, porque le restamos parte 
            invincibleTime -= Time.deltaTime;
        }
    }

    //Es un metodo para inicializar el contador de tiempo de invencibilidad. Al llamarlo le pasamos ese tiempo para par�metros
    public void MakeInvincibleFor(float numberOfSeconds)
    {
        //Inicializamos el contador de tiempo de invencibilidad
        invincibleTime += numberOfSeconds;
    }
}
