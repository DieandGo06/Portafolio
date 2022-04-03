using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDisparoFase2 : MonoBehaviour
{

    [SerializeField] GameObject naranja, rojo;

    //Variables derivadas
    public Disparo[] scriptDisparo = new Disparo[2];
    EnemyLevel enemyLevel;

    //cooldowns
    float cd_ataque;
    float cadenciaDisparo;

    //Varaibles base
    float nextDisparo;
    float speed = 1.5f;
    float contadorDisparo = -1;

    //Disparo circular (enemigo Lv3)
    public float num_de_disparoLv3;


    float tiempoTranscurrido;



    private void Awake()
    {
        enemyLevel = GetComponentInParent<EnemyLevel>();
        if (GameManager.fase == 2) GetComponent<SpawnDisparoFase2>().enabled = true;
        if (GameManager.fase != 2) GetComponent<SpawnDisparoFase2>().enabled = false;
    }

    void Start()
    {
        //cooldowns iniciales
        if (enemyLevel.nivel == 1) cd_ataque = 2f;
        if (enemyLevel.nivel == 2) cd_ataque = 2f;
        if (enemyLevel.nivel == 3) cd_ataque = 3f;
        if (enemyLevel.nivel == 4) cd_ataque = 4f;
        nextDisparo = cd_ataque;
    }


    void Update()
    {
        if (enemyLevel.nivel == 1) disparoLv1();
        if (enemyLevel.nivel == 2) disparoLv2();
        if (enemyLevel.nivel == 3) disparoLv3();
        if (enemyLevel.nivel == 4) disparoLv4();
        tiempoTranscurrido += Time.deltaTime;
    }


    void CrearDisparo(GameObject color)
    {
        Instantiate(color, transform.position, Quaternion.identity);
    }



    void disparoLv1()
    {
        cd_ataque = 2;
        scriptDisparo[0].directionY = -speed;
        if (tiempoTranscurrido > nextDisparo)
        {
            //Cambia la direccion en "x" del script del disparo
            scriptDisparo[0].directionX = Random.Range(-speed, speed);

            CrearDisparo(naranja);
            nextDisparo = tiempoTranscurrido + cd_ataque;
        }
    }

    void disparoLv2()
    {
        cd_ataque = 2;
        if (tiempoTranscurrido > nextDisparo)
        {
            //Esquina superior derecha
            scriptDisparo[1].directionX = speed;
            scriptDisparo[1].directionY = speed;
            CrearDisparo(rojo);

            //Esquina inferior derecha
            scriptDisparo[0].directionX = speed;
            scriptDisparo[0].directionY = -speed;
            CrearDisparo(naranja);

            //Esquina inferior izquierda
            scriptDisparo[1].directionX = -speed;
            scriptDisparo[1].directionY = -speed;
            CrearDisparo(rojo);

            //Esquina superior izquierda
            scriptDisparo[0].directionX = -speed;
            scriptDisparo[0].directionY = speed;
            CrearDisparo(naranja);

            nextDisparo = tiempoTranscurrido + cd_ataque;
        }
    }



    void disparoLv3()
    {
        cd_ataque = 2;
        Vector2 posicion_inicial = transform.position;

        if (tiempoTranscurrido > nextDisparo)
        {
            float angle_step = 360 / num_de_disparoLv3;
            float angle = 0f;

            for (int i = 0; i <= num_de_disparoLv3 - 1; i++)
            {
                //Primero se convierten las coordenadas cartesianas a polares
                float positionXEnPolares = transform.position.x + Mathf.Cos(angle * Mathf.Deg2Rad);
                float positionYEnPolares = transform.position.y + Mathf.Sin(angle * Mathf.Deg2Rad);
                Vector2 positionEnPolares = new Vector2(positionXEnPolares, positionYEnPolares);

                //"normalized" calcula la equivalencia entre los valores pero escalandolos entre 0 y 1
                Vector2 disparo_direction = (positionEnPolares - posicion_inicial).normalized * speed;

                if (i % 2 == 0)
                {
                    scriptDisparo[0].directionX = disparo_direction.x;
                    scriptDisparo[0].directionY = disparo_direction.y;
                    CrearDisparo(naranja);
                }
                else
                {
                    scriptDisparo[1].directionX = disparo_direction.x;
                    scriptDisparo[1].directionY = disparo_direction.y;
                    CrearDisparo(rojo);
                }

                angle += angle_step;
            }
            nextDisparo = tiempoTranscurrido + cd_ataque;
        }
    }



    void disparoLv4()
    {
        //Al final se encuentra el controlador de tiempo.
        cadenciaDisparo = 0.6f;
        int disparosMediaRotacion = 5;
        int disparosPorRotacion = 11;
        float speedRotation = speed / disparosMediaRotacion;

        if (Time.time > nextDisparo)
        {
            //Esquina superior izquierda
            if (contadorDisparo < disparosPorRotacion)
            {
                scriptDisparo[0].directionX = -speed;
                scriptDisparo[0].directionY = speed - (speedRotation * contadorDisparo);
            }
            if (contadorDisparo >= disparosPorRotacion)
            {
                scriptDisparo[0].directionX = -speed + (speedRotation * (contadorDisparo - disparosPorRotacion));
                scriptDisparo[0].directionY = -speed;
            }
            CrearDisparo(naranja);

            //Esquina inferior derecha
            if (contadorDisparo < disparosPorRotacion)
            {
                scriptDisparo[1].directionX = speed;
                scriptDisparo[1].directionY = -speed + (speedRotation * contadorDisparo);
            }
            if (contadorDisparo >= disparosPorRotacion)
            {
                scriptDisparo[1].directionX = speed - (speedRotation * (contadorDisparo - disparosPorRotacion));
                scriptDisparo[1].directionY = speed;
            }
            CrearDisparo(rojo);
        }


        if (Time.time > nextDisparo) //Controlador de tiempos de disparo
        {
            contadorDisparo += 1;
            nextDisparo = Time.time + cadenciaDisparo;

            if (contadorDisparo >= disparosPorRotacion * 2)
                contadorDisparo = 0;
        }
    }

}
