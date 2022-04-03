using UnityEngine;

public class ParametrosFase1 : MonoBehaviour
{
    public static ParametrosFase1 instance;
    public static int moveEnemyDirection;

    public bool isMoverEnemigos;
    public bool isGenerarFusiones;
    public GameObject[] enemigo = new GameObject[32];

    int contarEnemigo;
    float tiempoTranscurrido;


    private void Awake()
    {
        instance = this;
        moveEnemyDirection = 1;
    }

    void Update()
    {
        ActivarEnemigos();
        GenerarFusion();
        
        tiempoTranscurrido += Time.deltaTime;
        //Debug.Log(tiempoTranscurrido);
    }



    void ActivarEnemigos()
    {
        if (tiempoTranscurrido > 3)
        {
            isMoverEnemigos = true;
        }
    }

    void GenerarFusion()
    {
        float activador1 = 30;
        float activador2 = activador1 + 1.25f;
        float activador3 = activador2 + 1.75f;
        if (tiempoTranscurrido > activador1)
        {
            isMoverEnemigos = false;
            isGenerarFusiones = true;
        }
        if (tiempoTranscurrido > activador2)
        {
            isGenerarFusiones = false;
        }
        if (tiempoTranscurrido > activador3)
        {
            isMoverEnemigos = true;
        }
    }

    public void SetNuevoEnemigo(GameObject nuevoEnemigo)
    {
        var newEnemyLevel = nuevoEnemigo.GetComponent<EnemyLevel>();
        if (newEnemyLevel.nivel == 2)
        {
            enemigo[contarEnemigo] = nuevoEnemigo;
            nuevoEnemigo.transform.name = "enemigo " + contarEnemigo;
            contarEnemigo++;
        }
    }
    


}





