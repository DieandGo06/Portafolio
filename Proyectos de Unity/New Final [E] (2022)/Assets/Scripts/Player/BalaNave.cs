using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaNave : MonoBehaviour
{
    //Publicas
    public float speed;
    public float tiempoActiva;
    public bool canMove = false;

    //Privadas
    float tiempoTranscurrido;
    Vector3 posicionIncial;



    private void Awake()
    {
        canMove = false;
    }

    void FixedUpdate()
    {
        if (canMove == true)
        {
            Move();
            Cronometro();

            if (tiempoTranscurrido > tiempoActiva)
            {
                Desactivar();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Barrera"))
        {
            transform.tag = "BalaBloqueada";
        }

        if (collision.CompareTag("ColisionDeBarrera") && transform.CompareTag("BalaBloqueada"))
        {
            Desactivar();
            transform.tag = "Bala";
        }

        if (collision.CompareTag("Boss"))
        {
            Desactivar();
            transform.tag = "Bala";
        }
    }




    void Move()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void Cronometro()
    {
        tiempoTranscurrido += Time.fixedDeltaTime;
    }

    void Desactivar()
    {
        canMove = false;
        tiempoTranscurrido = 0;
        transform.position = posicionIncial;
        transform.rotation = Quaternion.identity;

    }


    public void GetPosicionInicial(Vector3 _posicion)
    {
        //Se ejecuta en el spawnBalas cuando la bala es creada
        posicionIncial = _posicion;
    }

}