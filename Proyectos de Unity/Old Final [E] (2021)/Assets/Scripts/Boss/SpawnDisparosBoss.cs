using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDisparosBoss : MonoBehaviour
{
    public GameObject naranja, rojo;
    Disparo[] scriptDisparo = new Disparo[2];

    float speed = 2;
    float nextDisparo;
    float cadenciaDisparo;
    float contadorDisparo;



    private void Start()
    {
        scriptDisparo[0] = naranja.GetComponent<Disparo>();
        scriptDisparo[1] = rojo.GetComponent<Disparo>();
    }

    private void Update()
    {
        DispararFase2();
    }



    void CrearDisparo(GameObject color)
    {
        Instantiate(color, transform.position, Quaternion.identity);
    }


    void DispararFase2()
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

            //Esquina superior derecha
            if (contadorDisparo < disparosPorRotacion)
            {
               scriptDisparo[1].directionX = speed - (speedRotation * contadorDisparo);
               scriptDisparo[1].directionY = speed;
            }
            if (contadorDisparo >= disparosPorRotacion)
            {
               scriptDisparo[1].directionX = -speed;
               scriptDisparo[1].directionY = speed - (speedRotation * (contadorDisparo - disparosPorRotacion));
            }
            CrearDisparo(rojo);

            //Esquina inferior derecha
            if (contadorDisparo < disparosPorRotacion)
            {
               scriptDisparo[0].directionX = speed;
               scriptDisparo[0].directionY = -speed + (speedRotation * contadorDisparo);
            }
            if (contadorDisparo >= disparosPorRotacion)
            {
               scriptDisparo[0].directionX = speed - (speedRotation * (contadorDisparo - disparosPorRotacion));
               scriptDisparo[0].directionY = speed;
            }
            CrearDisparo(naranja);

            //Esquina inferior izquierda
            if (contadorDisparo < disparosPorRotacion)
            {
               scriptDisparo[1].directionX = -speed + (speedRotation * contadorDisparo);
               scriptDisparo[1].directionY = -speed;
            }
            if (contadorDisparo >= disparosPorRotacion)
            {
               scriptDisparo[1].directionX = speed;
               scriptDisparo[1].directionY = -speed + (speedRotation * (contadorDisparo - disparosPorRotacion));
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

