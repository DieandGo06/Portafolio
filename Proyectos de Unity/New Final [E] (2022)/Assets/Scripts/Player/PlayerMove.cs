using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region Variables publicas
    [Header("Orbita Data")]
    public Transform centro;
    public float radio;
    [Range(0, 2)] public int orbitaActual = 2;
    [Range(0, 360)] public int rotacionInicial;


    [Header("Acelerar")]
    public float aceleracion;
    public float desaceleracion;
    public float velocidadMaxima;
    public float velocidadActual;
    public float duracionFreno;
    public string estado;
    

    [Header("Cambio de Orbitas")]
    public Transform[] posicionOrbita = new Transform[3];
    #endregion

    #region Variables privadas
    //Movimento General
    int direccion;
    Rigidbody2D rBody;
    [HideInInspector] public float anguloActual;
    #endregion




    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        radio = MetodosDeExtension.CalcularRadio(centro, posicionOrbita[orbitaActual]);
        anguloActual = rotacionInicial * Mathf.Deg2Rad;
        estado = "acelerar";
        direccion = 1;
    }

    private void FixedUpdate()
    {
        //Maquina de estado 2
        if (estado == "acelerar")
        {
            AcelerarSinInputs();
            CambiarDeOrbita();
            Mirar(centro);
            duracionFreno = Mathf.Abs(velocidadActual) / desaceleracion;

            if (Input.GetKeyDown("space")) estado = "frenar";
        }

        if (estado == "frenar")
        {
            FrenarSinInputs();
            CambiarDeOrbita();
            Mirar(centro);

            if (DebeCambiarDireccion())
            {
                direccion = direccion * -1;
                estado = "acelerar";
            }
        }
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

    #region Movimiento sin Inputs
    void AcelerarSinInputs()
    {
        velocidadActual += (Time.fixedDeltaTime * aceleracion) * direccion;
        FijarVelocidadMaxima();
        Mover(velocidadActual);
    }

    void FrenarSinInputs()
    {
        velocidadActual += (Time.fixedDeltaTime * -desaceleracion) * direccion;
        Mover(velocidadActual);
    }

    bool DebeCambiarDireccion()
    {
        if (direccion == 1)
        {
            if (velocidadActual <= 0) return true;
        }
        if (direccion == -1)
        {
            if (velocidadActual >= 0) return true;
        }
        return false;
    }

    void FijarVelocidadMaxima()
    {
        if (direccion == 1) if (velocidadActual >= velocidadMaxima) velocidadActual = velocidadMaxima;
        if (direccion == -1) if (velocidadActual <= -velocidadMaxima) velocidadActual = -velocidadMaxima;
    }

    #endregion


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




    #region Funciones desechadas
    //Codigo sacado de: https://gamedev.stackexchange.com/questions/128141/how-to-orbit-an-object-around-another-object-in-an-oval-path-in-unity
    void OcilarOvalo()
    {
        #region Explicacion
        /* Como el "sin" y "cos" son operaciones ciclicas, no importa el valor
         * que tome la variable de rotacion. Tambien, al multiplicarse con la 
         * velocidad, si no se mueve, el valor es cero y suma a la rotacion. 
         * 
         * Formulas del movimiento ovalado:
         * x = centerX + (cos(angulo) * radioMaximoX);
         * y = centerY + (sin(angulo) * radioMaximoY);
         */
        #endregion
        /*
        float speedX = Input.GetAxisRaw("Horizontal") * speed;
        float anchoDeOrbita = radio * 2;
        float altoDeOrbita = radio;

        rotacionSegunTiempo += Time.fixedDeltaTime * speedX;
        float posX = centro.position.x + (Mathf.Cos(rotacionSegunTiempo) * anchoDeOrbita);
        float posY = centro.position.y + (Mathf.Sin(rotacionSegunTiempo) * altoDeOrbita);

        Vector3 newPosition = new Vector3(posX, posY, 0);
        rBody.MovePosition(newPosition);
        */
    }


    //Codigo sacado del segundo ejemplo de: https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html
    /*
     void DashConLerp()
    {
        journeyLength = Vector3.Distance(startMarker, endMarker);
        float distCovered = (Time.time - inicioDelDash) * speedDash;
        float fractionOfJourney = distCovered / journeyLength;

        Vector3 newPosition = Vector3.Lerp(startMarker, endMarker, fractionOfJourney);
        rBody.MovePosition(newPosition);

        if (transform.position == endMarker) isDashing = false;
    }
    */
    /*
    Vector3 CalcularEndDashPosition(float _direccion)
    {
        if (_direccion > 0)
        {
            rotacionSegunTiempo += dashDintanceAngle;
        }
        if (_direccion < 0)
        {
            rotacionSegunTiempo -= dashDintanceAngle;
        }
        float posX = Mathf.Cos(rotacionSegunTiempo) * radio + centro.position.x;
        float posY = Mathf.Sin(rotacionSegunTiempo) * radio + centro.position.y;
        return new Vector3(posX, posY, 0);
    }
    */
    #endregion

}
