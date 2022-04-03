using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveInputs : MonoBehaviour
{
    #region Variables
    [Header("Orbita Data")]
    public Transform centro;
    public float radio;
    [Range(0, 2)]
    public int orbitaActual = 2;
    [Range(-Mathf.PI, Mathf.PI)]
    public float rotacionInicial;


    [Header("Movimiento")]
    public float speed;
    public float speedDash;
    [Range(0, 0.5f)]
    public float duracionDash;
    public string estado;

    [Header("Cambio de Orbitas")]
    public Transform[] posicionOrbita = new Transform[3];

    
    //Movimento General
    float direccion;
    Rigidbody2D rBody;
    [HideInInspector] public float anguloActual;

    //Movimiento con inputs y dash
    float ultimaDireccion;
    float inicioDelDash;
    #endregion



    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
        radio = MetodosDeExtension.CalcularRadio(centro, posicionOrbita[orbitaActual]);
        anguloActual = rotacionInicial;
        //estado = "idle";
        estado = "idle";
        direccion = 1;
    }

    private void FixedUpdate()
    {
        #region Maquina de estados con Inputs move

        //Maquina de estado 1
        if (estado == "idle")
        {
            Mirar(centro);
            CambiarDeOrbita();
            if (Input.GetKeyDown("space")) estado = "dashing";
            if (Input.GetButton("Horizontal")) estado = "moving";
        }

        if (estado == "moving")
        {
            OscilarConInputs();
            Mirar(centro);
            CambiarDeOrbita();

            if (Input.GetKeyDown("space")) estado = "dashing";
            if (Input.GetButton("Horizontal") == false) estado = "idle";
        }

        if (estado == "dashing")
        {
            Mirar(centro);
            DashConAngulos();
        }

        #endregion
    }




    #region Base del movimiento
    //Codigo sacado de: https://www.youtube.com/watch?v=BGe5HDsyhkY
    void Mover(float _velocidad)
    {
        #region Explicacion
        /* Al tener el radio y angulo, las coordenadas se trabajan como polares.
         * Se suma o resto al angulo y se pasan esta nuevas coordenadas polares a rectangulares
         * para hallar la nueva posicion.
         */
        #endregion
        anguloActual = MetodosDeExtension.MoverAngulo(anguloActual, _velocidad, 1);//El "1" deberia ser "direccion" pero se genera un bug
        Vector2 newPosition = MetodosDeExtension.PolaresToRectangulares(radio, anguloActual, centro);
        rBody.MovePosition(newPosition);
    }
    #endregion

    void OscilarConInputs()
    {
        direccion = Input.GetAxisRaw("Horizontal");
        if (Input.GetButton("Horizontal")) ultimaDireccion = direccion;

        float speedX = direccion * speed;
        Mover(speedX);
    }

    void DashConAngulos()
    {
        if (inicioDelDash == 0)
        {
            inicioDelDash = Time.time;
        }

        if (Input.GetButton("Horizontal"))
        {
            direccion = Input.GetAxisRaw("Horizontal");
            ultimaDireccion = direccion;
        }
        else direccion = ultimaDireccion;

        float speedX = direccion * speedDash;
        anguloActual += Time.fixedDeltaTime * speedX;
        float posX = Mathf.Cos(anguloActual) * radio + centro.position.x;
        float posY = Mathf.Sin(anguloActual) * radio + centro.position.y;

        Vector3 newPosition = new Vector3(posX, posY, 0);
        rBody.MovePosition(newPosition);

        if (Time.time > inicioDelDash + duracionDash)
        {
            estado = "idle";
            inicioDelDash = 0;
        }
    }


    void Mirar(Transform target)
    {
        float distanciaX = transform.position.x - target.position.x;
        float distanciaY = transform.position.y - target.position.y;
        float angulo = Mathf.Atan2(distanciaY, distanciaX) * Mathf.Rad2Deg;
        rBody.MoveRotation(angulo - 270);
    }

    void CambiarDeOrbita()
    {
        if (Input.GetKeyDown("up") && orbitaActual > 0)
        {
            orbitaActual--;
            Transform nuevaOrbita = posicionOrbita[orbitaActual];
            radio = MetodosDeExtension.CalcularRadio(centro, nuevaOrbita);
            rBody.MovePosition(nuevaOrbita.position);
        }
        if (Input.GetKeyDown("down") && orbitaActual < 2)
        {
            orbitaActual++;
            Transform nuevaOrbita = posicionOrbita[orbitaActual];
            radio = MetodosDeExtension.CalcularRadio(centro, nuevaOrbita);
            rBody.MovePosition(nuevaOrbita.position);
        }
    }
}
