using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovimientoCircular : MonoBehaviour
{
    [Header("Propiedades")]
    [Range(1, 3)] public int orbita;
    [Range(-1, 1)] public int direccion;
    [Range(0, 360)] public int rotacionInicial;
    public float speed;
    public float radio;

    [Header("Ejecucion en Inspector")]
    [Space(20)]
    public Transform centro;
    public Transform satelite;

    public Action Mover;
    public Action CambiarPosicion;

    //Privadas y ocultas
    Rigidbody2D rBody;
    Barrera barreraScript;
    public float anguloActual;



    private void Awake()
    {
        if (GameMaster.instance != null)
        {
            centro = GameMaster.instance.centro;
            satelite = GameMaster.instance.satelites[orbita - 1];
        }
        rBody = GetComponent<Rigidbody2D>();
        radio = MetodosDeExtension.CalcularRadio(centro, satelite);

        //Comprueba si tiene el componente para controlar el lineRederer, para copiarle la orbita y evitar bugs
        if (GetComponent<TrazarLineaCircular>()) orbita = GetComponent<TrazarLineaCircular>().orbita;

        //Comprueba si el gameObject es una barrera. De serlo, se ejecutaran unas funciones unicas de la barrera
        if (GetComponent<Barrera>()) barreraScript = GetComponent<Barrera>();

        //Si es una barrera, la rotacion inicial debe ser 0 para evitar bugs con los colliders
        if (barreraScript != null) rotacionInicial = 0;
    }

    private void Start()
    {
        anguloActual = MetodosDeExtension.Map(rotacionInicial, 0, 360, 0, Mathf.PI * 2);
        MoverAPosicionInicial();
        Mover += MoverRigidBody;
    }


    private void FixedUpdate()
    {
        Mover();
    }



    //Codigo sacado de: https://www.youtube.com/watch?v=BGe5HDsyhkY
    public void MoverRigidBody()
    {
        #region Explicacion
        /* Al tener el radio y angulo, las coordenadas se trabajan como polares.
         * Se suma o resto al angulo y se pasan esta nuevas coordenadas polares a rectangulares
         * para hallar la nueva posicion.
         */
        #endregion
        anguloActual = MetodosDeExtension.MoverAngulo(anguloActual, speed, direccion);
        Vector2 newPosition = MetodosDeExtension.PolaresToRectangulares(radio, anguloActual, centro);
        rBody.MovePosition(newPosition);
    }

    //Esta funcion es usada en el script editor
    public void MoverAPosicionInicial()
    {
        float posX = Mathf.Cos(anguloActual) * radio + centro.position.x;
        float posY = Mathf.Sin(anguloActual) * radio + centro.position.y;
        transform.position = new Vector3(posX, posY, -1);
    }
}
