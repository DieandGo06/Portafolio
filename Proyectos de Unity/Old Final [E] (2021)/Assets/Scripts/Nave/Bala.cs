using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{

    public float speed;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject particulas;


    void FixedUpdate()
    {
        moveBala();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "enemigo" || collision.transform.tag == "boss")
        {
            ParticulasDeImpacto();
            CrearExplosion();
            AudioManager.PlaySound(AudioManager.instance.impactoEnemigo);

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "disparo_naranja")
        {
            Destroy(gameObject);
        }
    }
    
    


    void moveBala()
    {
        GetComponent<Transform>().Translate(Vector2.up * speed * Time.deltaTime);
    }

    void ParticulasDeImpacto()
    {
        for (int i = 0; i <= 15; i++)
        {
            Instantiate(particulas, transform.position, Quaternion.identity);
            referenciaParaParticulas();
        }
    }

    void CrearExplosion()
    {
        GameObject animacionExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(animacionExplosion, 0.25f);
    }

    void referenciaParaParticulas()
    {
        p_explosion scriptParticulas = particulas.GetComponent<p_explosion>();
        scriptParticulas.SetObjetoCreador(gameObject);
    }
}
