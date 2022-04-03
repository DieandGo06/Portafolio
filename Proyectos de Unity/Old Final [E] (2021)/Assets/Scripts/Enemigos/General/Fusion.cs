using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fusion : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;
    [SerializeField] Animator animator;
    [SerializeField] GameObject ondaLevelUp;

    GameObject enemigoCreador;
    GameObject enemigoFusionado;
    public GameObject newEnemy;

    int fusionLayer = 16;
    bool seFusionan;


    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (seFusionan)
        {
            if (enemigoCreador == null) Destroy(enemigoFusionado);
            if (enemigoFusionado == null) Destroy(enemigoCreador);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "enemigo")
        {
            //Compara el ID de los objetos, organizados por orden de creacion.
            if (gameObject.GetInstanceID() > collision.GetInstanceID())
            {
                transform.name = "Creador";
                enemigoCreador = gameObject;
                enemigoFusionado = collision.gameObject;

                PlayCreadorAnimation();
                desactivarEnemigo();
            }
            else
            {
                transform.name = "Fusionado";
                enemigoFusionado = gameObject;
                enemigoCreador = collision.gameObject;

                PlayFusionadoAnimation();
                desactivarEnemigo();
            }
            gameObject.layer = fusionLayer;
            seFusionan = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "enemigo")
        {
            if (gameObject.GetInstanceID() > collision.GetInstanceID())
            {
                MoveToEnemigoFusionado(collision);
            }
            else
            {
                MoveToEnemigoCreador(collision);
            }
        }
    }



    void MoveToEnemigoCreador(Collider2D other)
    {
        float moveSpeed = 1f;
        Vector2 enemigoCreadorPos = other.transform.position;
        Vector2 position = Vector2.Lerp(transform.position, enemigoCreadorPos, moveSpeed * Time.deltaTime);

        //El condicional reduce la desalinacion tras la fusion de la fase 1
        if (rigidbody2D.bodyType != RigidbodyType2D.Static)
            rigidbody2D.MovePosition(position);
    }

    void MoveToEnemigoFusionado(Collider2D other)
    {
        float moveSpeed = 1f;
        Vector2 enemigoFusionadoPos = other.transform.position;
        Vector2 position = Vector2.Lerp(transform.position, enemigoFusionadoPos, moveSpeed * Time.deltaTime);

        //El condicional reduce la desalinacion tras la fusion de la fase 1
        if (rigidbody2D.bodyType != RigidbodyType2D.Static)
            rigidbody2D.MovePosition(position);
    }



    void desactivarEnemigo()
    {
        if (rigidbody2D.velocity != Vector2.zero)
        {
            rigidbody2D.AddForce(-rigidbody2D.velocity, ForceMode2D.Impulse);
        }

        GetComponent<MoveEnemyFase2>().enabled = false;
        GetComponentInChildren<SpawnDisparoFase2>().enabled = false;
    }


    void PlayCreadorAnimation()
    {
        animator.SetBool("PlayEnemigoCreador", true);
    }

    void PlayFusionadoAnimation()
    {
        animator.SetBool("PlayEnemigoFusionado", true);
    }

    void PlayOnda()
    {
        GameObject onda = Instantiate(ondaLevelUp, transform.position, Quaternion.identity);
        Destroy(onda, 0.5f);
    }


    void DestruirFusionado()
    {
        Destroy(enemigoFusionado);
    }

    public void DestruirFusion()
    {
        if (enemigoFusionado.GetComponent<EnemyLifes>().vidas <= 0)
        {
            Destroy(enemigoCreador);
            Destroy(enemigoFusionado);
        }
        if (enemigoCreador.GetComponent<EnemyLifes>().vidas <= 0)
        {
            if (enemigoFusionado != null) Destroy(enemigoFusionado);
            Destroy(enemigoCreador);
        }
    }

    void EndFusion()
    {
        Instantiate(newEnemy, transform.position, transform.rotation);
        Destroy(enemigoFusionado);
        Destroy(enemigoCreador);
    }
}
