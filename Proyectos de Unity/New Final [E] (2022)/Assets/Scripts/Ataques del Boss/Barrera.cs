using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(TrazarLineaCircular))]
public class Barrera : MonoBehaviour
{
    #region Variables
    [Header("Tipo de Collider")]
    public bool tieneColliderTriangular;
    public bool tieneColliderCircular;

    [Header("Estados")]
    public float duracionActivo;
    [Range(0.1f, 3f)] public float duracionPreview;
    [Range(0.1f, 1f)] public float duracionFades;
    public string estado;


    //Logica de maquina de estados
    public float opacidad;
    bool fuePrevisualizado;
    bool fueActivado;
    bool fueApagado;


    Rigidbody2D rBody;
    LineRenderer lineRenderer;
    TrazarLineaCircular lineaScript;
    MovimientoCircular movimientoCircular;

    Transform centro;
    List<float> anguloVerticesList = new List<float>();
    List<CircleCollider2D> collidersCirculares = new List<CircleCollider2D>();
    #endregion



    void Awake()
    {
        #region NOTA IMPORTANTE:
        /* Este script depende mucho del script que modifica la cantidad de vertices ("TrazarLineaCircular")
         * Asi que todos los metodos que recorran los vertices del lineRenderer deben estar como minimo en "Start"
         * dejando que aquel otro script ejecute todos sus metodos en "Awake".
         * 
         * Si esto no es suficiente y saltan errores en el recorrido de los arreglos de los vertices
         * entra en ProjectSetting y establece el orden de ejecucion de los scripts.
         */
        #endregion
        rBody = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        lineaScript = GetComponent<TrazarLineaCircular>();
        movimientoCircular = GetComponent<MovimientoCircular>();
    }

    private void Start()
    {
        centro = lineaScript.centro;

        //Se guardan para usarlos para mover los vertices del LineRenderer
        GuardarAnguloDeVertices();
        //Si la barrera tiene mas de 90 grados usara collider circulares
        SelecionarTipoDeCollider();

        estado = null;
        opacidad = 0;
        SetOpacidad(opacidad);
    }

    private void OnEnable()
    {
        movimientoCircular.Mover += MirarAlCentro;
        movimientoCircular.Mover += MoverLineRenderer;
    }

    private void OnDisable()
    {
        movimientoCircular.Mover -= MirarAlCentro;
        movimientoCircular.Mover -= MoverLineRenderer;
    }


    private void Update()
    {

        if (estado == "iniciando") {
            if (!fuePrevisualizado) {
                //gameObject.SetActive(true);
                CambiarPosicion();
                Previsualizar();
                fueApagado = false;
                fuePrevisualizado = true;
            }
            Tareas.Nueva(duracionPreview, FadeIn);
            if (opacidad >= 1) CambiarEstado("activo");
        }


        if (estado == "activo") {
            if (!fueActivado) {
                CollidersEnabled(true);
                MovimientoEnabled(true);
                Tareas.Nueva(duracionActivo, CambiarEstado, "apagado");
                fueActivado = true;
            }
        }


        if (estado == "apagado") {
            if (!fueApagado) {
                CollidersEnabled(false);
                MovimientoEnabled(false);
                fueApagado = true;
            }
            if (opacidad > 0) {
                FadeOut();
            }
            if (opacidad <= 0) {
                estado = null;
                ReiniciarBooleanas();
                gameObject.SetActive(false);
            }
        }

        //Evita que la opacidad sea menor que "0" o mayor que "1"
        SetOpacidad(opacidad);
        RegularOpacidad();
    }




    #region Logica y metodos de la maquina de estados
    void Previsualizar()
    {
        gameObject.SetActive(true);
        movimientoCircular.enabled = false;
        CollidersEnabled(false);
        opacidad = 0.3f;
    }

    void CambiarEstado(string nuevoEstado)
    {
        estado = nuevoEstado;
    }

    void FadeIn()
    {
        opacidad += Time.deltaTime / duracionFades;
    }
    void FadeOut()
    {
        opacidad -= Time.deltaTime / duracionFades;
    }

    void MovimientoEnabled(bool estado)
    {
        movimientoCircular.enabled = estado;
    }

    void CollidersEnabled(bool estado)
    {
        if (tieneColliderTriangular) GetComponent<PolygonCollider2D>().enabled = estado;
        if (tieneColliderCircular) {
            for (int i = 0; i < collidersCirculares.Count; i++) {
                collidersCirculares[i].enabled = estado;
            }
        }
    }

    void ReiniciarBooleanas()
    {
        fuePrevisualizado = false;
        fueActivado = false;
        fueApagado = false;
    }

    void SetOpacidad(float _opacidad)
    {
        Color _color = lineRenderer.material.color;
        lineRenderer.material.color = new Color(_color.r, _color.g, _color.b, _opacidad);
    }

    void RegularOpacidad()
    {
        if (opacidad < 0) opacidad = 0;
        else if (opacidad > 1) opacidad = 1;
    }

    //Se usa en el script mananger de los ataques
    public void ReActivar()
    {
        gameObject.SetActive(true);
        estado = "iniciando";
    }
    #endregion

    
    #region Mover, girar y guardar los angulos de todos los vertices
    void GuardarAnguloDeVertices()
    {
        int cantidadVertices = lineRenderer.positionCount;
        for (int i = 0; i < cantidadVertices; i++) {
            anguloVerticesList.Add(i);
            Vector3 vertice = lineRenderer.GetPosition(i);
            anguloVerticesList[i] = Mathf.Atan2(vertice.y - centro.position.y, vertice.x - centro.position.x);
        }
    }

    public void MirarAlCentro()
    {
        float distanciaX = transform.position.x - centro.position.x;
        float distanciaY = transform.position.y - centro.position.y;
        float angulo = Mathf.Atan2(distanciaY, distanciaX) * Mathf.Rad2Deg;
        rBody.MoveRotation(angulo);
    }

    public void MoverLineRenderer()
    {
        float velocidad = movimientoCircular.speed;
        int direccion = movimientoCircular.direccion;

        int cantidadVertices = lineRenderer.positionCount;
        for (int i = 0; i < cantidadVertices; i++) {
            anguloVerticesList[i] = MetodosDeExtension.MoverAngulo(anguloVerticesList[i], velocidad, direccion);
            Vector2 nuevaPosicion = MetodosDeExtension.PolaresToRectangulares(lineaScript.radio, anguloVerticesList[i], centro);
            lineRenderer.SetPosition(i, new Vector3(nuevaPosicion.x, nuevaPosicion.y, lineaScript.posicionZVertices));
        }
    }
    #endregion


    #region Crear un collider triangular
    void CrearColliderTriangular()
    {
        #region Explicacion de la matematica
        /* 1. La posicion de los vertices del LineRederer toma como punto(0, 0) el punto cero de la escena
         *    pero el collider usa la posicion del transform.
         *    Asi que se convierten las posiciones absolutas en relativas.
         *    
         * 2. La linea tiene cierto grosor y los vertices del collider deben ir en el extremo superior de la linea.
         *    Sin embargo, para sumar los valores se debe pasar el grosor, que es una distancia de coordenadas polares, a coordenadas rectangulares
         *    para ello se debe hallar el angulo entre el centro de la circunferencia y los vertices absolutos (que en realidad son relativos a centro)
         *    
         * 3. Tras ello, se pasan de polares a rectangulares y se desplazan los vertices, que es solo al suma entre el grosor y los vertices.
         */
        #endregion
        Vector3[] verticesAbsolutos = new Vector3[]
        {
            //Se agarra el tercer y antepenultimo vertices para que el collider no sea pixel perfect
            lineRenderer.GetPosition(3),
            lineRenderer.GetPosition((lineRenderer.positionCount-1) / 2),
            lineRenderer.GetPosition(lineRenderer.positionCount-4)
        };

        Vector3[] verticesRelativosAlTransform = new Vector3[]
        {
            verticesAbsolutos[0] - transform.position,
            verticesAbsolutos[1] - transform.position,
            verticesAbsolutos[2] - transform.position
        };

        Vector3 centroDeOrbita = lineaScript.centro.position;
        float[] anguloCentroVertice = new float[]
        {
            CalcularAnguloEnRadianes(centroDeOrbita, verticesAbsolutos[0]),
            CalcularAnguloEnRadianes(centroDeOrbita, verticesAbsolutos[1]),
            CalcularAnguloEnRadianes(centroDeOrbita, verticesAbsolutos[2]),
        };

        Vector2 grosorLinea = Vector2.one * (lineRenderer.startWidth / 2);
        Vector2[] verticesCollider = new Vector2[]
        {
            DesplazarCoordenadas(verticesRelativosAlTransform[0], grosorLinea, anguloCentroVertice[0]),
            DesplazarCoordenadas(verticesRelativosAlTransform[1], grosorLinea + (Vector2.one * 0.6f), anguloCentroVertice[1]),
            DesplazarCoordenadas(verticesRelativosAlTransform[2], grosorLinea, anguloCentroVertice[2])
        };

        PolygonCollider2D collider = gameObject.AddComponent<PolygonCollider2D>();
        collider.SetPath(0, verticesCollider);
        collider.isTrigger = true;
    }

    float CalcularAnguloEnRadianes(Vector2 punto1, Vector2 punto2)
    {
        return Mathf.Atan2(punto2.y - punto1.y, punto2.x - punto1.x);
    }

    Vector2 DesplazarCoordenadas(Vector2 puntoInicial, Vector2 distancia, float angulo)
    {
        Vector2 coordenadasRectangularesDeDistancia = new Vector2(distancia.x * Mathf.Cos(angulo), distancia.y * Mathf.Sin(angulo));
        return puntoInicial + coordenadasRectangularesDeDistancia;
    }
    #endregion


    #region Crear multiples colliders circulares
    void CrearTodosLosCollidersCirculares()
    {
        #region Explicacion
        /* El conjunto de colliders esta pensado para una linea con vertices de multiplos de 10.
         * 
         * Se crea un collider cada 10 vertices, y como las linea pueden tener cualquier cantidad de vertices
         * se desactiva el ultimo collider, y se crea uno nuevo en el decimo ultimo vertice.
         * 
         * De esta forma, queda con el mismo espacio de no "pixelPerfect" al inicio y final de la linea.
         */
        #endregion
        for (int i = 0; i < lineRenderer.positionCount; i++) {
            if (i % 10 == 0 && i != 0) {
                CrearUnColliderCircular(i);
            }
        }
        collidersCirculares[collidersCirculares.Count - 1].enabled = false;
        collidersCirculares.Remove(collidersCirculares[collidersCirculares.Count - 1]);

        CrearUnColliderCircular(lineRenderer.positionCount - 11);
    }

    void CrearUnColliderCircular(int verticeNumero)
    {
        float factorColliderSize = 1.5f;

        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
        Vector2 posicionRelativa = lineRenderer.GetPosition(verticeNumero) - lineRenderer.GetPosition(0);
        collider.offset = posicionRelativa;

        float radioCollider = collider.radius = lineRenderer.startWidth * factorColliderSize;
        collider.radius = radioCollider;
        collider.isTrigger = true;
        collidersCirculares.Add(collider);
    }
    #endregion


    #region Seleccionar que collider usara y Cambio de posicion aleatoria
    void SelecionarTipoDeCollider()
    {
        if (lineaScript.porcentajeDelCirculo >= 30) {
            tieneColliderTriangular = false;
            tieneColliderCircular = true;
        }
        if (tieneColliderTriangular) CrearColliderTriangular();
        if (tieneColliderCircular) CrearTodosLosCollidersCirculares();
    }


    void CambiarPosicion()
    {
        //Se usa UnityEngine porque se esta usando "System" para el Action y "System" tiene su propio Random
        float grados = UnityEngine.Random.Range(0f, 360f);
        movimientoCircular.anguloActual = grados * Mathf.Deg2Rad;

        float anguloPrimerVertice = anguloVerticesList[0] * Mathf.Rad2Deg;
        for (int i = 0; i < anguloVerticesList.Count; i++) {
            //Formula: nuevoAngulo = viejoAngulo - viejoAnguloPrimerVertice + nuevoAnguloPrimerVertice;
            float nuevoAngulo = (anguloVerticesList[i] * Mathf.Rad2Deg) - anguloPrimerVertice + grados;
            anguloVerticesList[i] = nuevoAngulo * Mathf.Deg2Rad;
        }
        //Para ejecutar el movimiento movimiento circular debe ejecutar accion de Mover
        movimientoCircular.enabled = true;
        movimientoCircular.Mover();
        movimientoCircular.enabled = false;
    }
    #endregion







    void CambiarOrbita()
    {
        lineaScript.radio = 2.7f;
        movimientoCircular.radio = 2.7f;

        lineaScript.orbita = 2;
        int nuevoNumeroDeVertices = lineaScript.CalcularNumeroDeVertices();
        lineRenderer.positionCount = nuevoNumeroDeVertices;
        lineaScript.DibujarCirculo(nuevoNumeroDeVertices);

        anguloVerticesList.Clear();
        GuardarAnguloDeVertices();
    }
}

