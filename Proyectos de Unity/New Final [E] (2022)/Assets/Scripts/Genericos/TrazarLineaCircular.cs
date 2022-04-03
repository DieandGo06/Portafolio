using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrazarLineaCircular : MonoBehaviour
{
    [Header("Propiedades")]
    [Space(10)]
    [Range(1, 3)] public int orbita;
    [Range(0, 100)] public int porcentajeDelCirculo;
    [Range(0, 360)] public int rotacionInicial;
    public int numerosDeVertices;
    public float radio;

    [Header("Propiedades Extras")]
    [Space(10)]
    [Range(1f, 3f)] public float altoExtra;
    [Range(1f, 3f)] public float anchoExtra;

    [Header("Plano de dibujo")]
    [Space(10)]
    public int posicionZVertices;

    [Header("Ejecucion en Inspector")]
    [Space(20)]
    public Transform centro;
    public Transform satelite;
    public LineRenderer lineRender;



    private void Awake()
    {
        #region NOTA IMPORTANTE:
        /* Todos los metodos de este script deberian ejecutarse en el "Awake" ya que de estos pueden depender otros scripts 
         * como por ejemplo, "barrera".
         * Asi mismo, todos los script que dependan de este, deben referenciar este script minimo en el "Start".
         * 
         * Si esto no es suficiente y saltan errores en el recorrido de los arreglos de los vertices
         * entra en ProjectSetting y establece el orden de ejecucion de los scripts.
         */
        #endregion
        if (GameMaster.instance != null)
        {
            centro = GameMaster.instance.centro;
            satelite = GameMaster.instance.satelites[orbita - 1];
        }
        lineRender = GetComponent<LineRenderer>();
        radio = MetodosDeExtension.CalcularRadio(centro, satelite);

        //El numero de vertices varia segun el porcentaje del circulo y la orbita
        numerosDeVertices = CalcularNumeroDeVertices();

        //Si es una barrera, la rotacion inicial debe ser 0 para evitar bugs con los colliders
        if (GetComponent<Barrera>()) rotacionInicial = 0;

        lineRender.positionCount = numerosDeVertices;
        DibujarCirculo(numerosDeVertices);
    }


    //Codigo sacado de: https://www.youtube.com/watch?v=DdAfwHYNFOE
    public void DibujarCirculo(int vertices)
    {
        #region Explicacion
        /* El circulo se genera con una linea con muchas pequeños vertices que van conformando la circunferencia
         * La circunferencia puede ser completa o incompleta y dependera de los angulos en radianos por los que 
         * se multiplique.
         * 
         * "float radianActual = progresoDelCirculo * porcentajeEnRadianes;"
         * Opte por usar un map para "porcentajeEnRadianes" porque es mucho mas facil entender los angulos
         * con relacion al porcentaje de completacion que en base a radianes.
         * 
         * El map va de (0, Mathf.PI * 2) porque 2PI es una circunferencia completa.
         */
        #endregion
        lineRender.positionCount = vertices;
        float porcentajeEnRadianes = MetodosDeExtension.Map(porcentajeDelCirculo, 0, 100, 0, Mathf.PI * 2);

        for (int verticeAcual = 0; verticeAcual < vertices; verticeAcual++)
        {
            float progresoDelCirculo = (float)verticeAcual / vertices;
            float radianActual = progresoDelCirculo * porcentajeEnRadianes + (rotacionInicial * Mathf.Deg2Rad);

            #region Formula
            /* Cordenadas Polares a rectangulares:
             * x = radio * cos(angulo)
             * y = radio * sin(angulo)
             * Se suma la posicion del gameObject centro para que si se mueve el centro, tambien lo haga la recta
             */
            #endregion
            float posX = (radio * anchoExtra) * Mathf.Cos(radianActual) + centro.position.x;
            float posY = (radio * altoExtra) * Mathf.Sin(radianActual) + centro.position.y;

            Vector3 verticePosicion = new Vector3(posX, posY, posicionZVertices);
            lineRender.SetPosition(verticeAcual, verticePosicion);
        }
    }

    public int CalcularNumeroDeVertices()
    {
        float factor = 1;

        if (GameMaster.instance != null)
        {
            float radio1 = MetodosDeExtension.CalcularRadio(centro, GameMaster.instance.satelites[0]);
            float radio2 = MetodosDeExtension.CalcularRadio(centro, GameMaster.instance.satelites[1]);
            float radio3 = MetodosDeExtension.CalcularRadio(centro, GameMaster.instance.satelites[2]);

            if (orbita == 1) factor = 1;
            else if (orbita == 2) factor = radio2/radio1;
            else if (orbita == 3) factor = radio3/radio1;
        }

        return (int)((porcentajeDelCirculo * 2) * factor);
    }
    
}
