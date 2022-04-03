using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirNave : MonoBehaviour
{
    [Header("Posicionamiento Inicial")]
    public Transform centro;
    public Transform satelite;
    public float radio;
    public bool ocultar;

    //Son publicas porque se usan en el script editor
    [HideInInspector] public float anguloActual;
    [HideInInspector] public PlayerMove player;


    [Header("Distancia al frenar nave")]
    public float distancia;
    public float distanciaX;
    public float distanciaY;
    public float tiempo;



    private void Awake()
    {
        radio = MetodosDeExtension.CalcularRadio(centro, satelite);
        player = FindObjectOfType<PlayerMove>();
    }

    private void Start()
    {
        anguloActual = player.anguloActual;
        if (ocultar) GetComponent<SpriteRenderer>().enabled = false;
    }

    private void FixedUpdate()
    {
        Seguir();
    }



    //Codigo sacado de: https://www.youtube.com/watch?v=BGe5HDsyhkY
    void Seguir()
    {
        Vector2 newPosition = MetodosDeExtension.PolaresToRectangulares(radio, player.anguloActual, centro);
        transform.position = new Vector3(newPosition.x, newPosition.y, 0);
    }


    //Esta funcion es usada en el script editor
    public void MoverAPosicionInicial()
    {
        float posX = Mathf.Cos(anguloActual) * radio + centro.position.x;
        float posY = Mathf.Sin(anguloActual) * radio + centro.position.y;
        transform.position = new Vector3(posX, posY, -1);
    }

    #region Metodos descartados
    //La idea era calcular la posicion en donde la nave frenaba por completo: Posiblemente se borrara esta funcion
    void CalcularDistanciaAlFrenar()
    {
        float delayFrenado = Mathf.Abs(player.velocidadActual) / player.desaceleracion;
        tiempo = delayFrenado;
        //Debo tener el frame rate que tiene el juego en todo momento para predecir cuantos frames tardara
        //en hacer el frenado. "delatFrenado" me esta dando el tiempo pero en segundos.

        // distancia = VelInicial * tiempo + (aceleracion *  tiempo)/2
        float distanciaRadianes = player.velocidadActual * delayFrenado + ((player.desaceleracion * delayFrenado) / 2);

        float distX = Mathf.Cos(distanciaRadianes) * radio + centro.position.x;
        float distY = Mathf.Sin(distanciaRadianes) * radio + centro.position.y;

        distancia = distanciaRadianes;
        distanciaX = distX;
        distanciaY = distY;

        anguloActual = player.anguloActual;
        float posX = Mathf.Cos(anguloActual + distanciaRadianes) * radio + centro.position.x;
        float posY = Mathf.Sin(anguloActual + distanciaRadianes) * radio + centro.position.y;

        Vector3 newPosition = new Vector3(posX, posY , 0);
        transform.position = newPosition;
    }
    #endregion
}
