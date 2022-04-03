using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifes : MonoBehaviour
{
    EnemyLevel enemyLevel;

    GameObject particulas;
    [SerializeField] GameObject prefabParticulas;

    [HideInInspector] public float vidas;



    private void Awake()
    {
        enemyLevel = GetComponent<EnemyLevel>();
    }

    private void Start()
    {
        if (enemyLevel.nivel == 1) vidas = 5;
        if (enemyLevel.nivel == 2) vidas = 10;
        if (enemyLevel.nivel == 3) vidas = 12;
        if (enemyLevel.nivel == 4) vidas = 15;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        int fusionLayer = 16;
        if (collision.transform.tag == "naveBala"  &&  gameObject.layer != fusionLayer)
        {
            vidas--;
            //retrocedePorImpacto();
            if (vidas <= 0) destruirEnemigo();
        }

        if (collision.transform.tag == "naveBala"  &&  gameObject.layer == fusionLayer)
        {
            vidas--;
            if (vidas <= 0)
            {
                ExplotarFusion();
                GetComponent<Fusion>().DestruirFusion();
            }
        }
    }




    void destruirEnemigo()
    {
        explosion();
        Destroy(gameObject);
    }

    void retrocedePorImpacto()
    {
        Vector2 fuerzaDeEmpuje = Vector2.up * 50 * Time.deltaTime;
        GetComponent<Rigidbody2D>().AddForce(fuerzaDeEmpuje, ForceMode2D.Impulse);
    }

    void explosion()
    {
        for (int i = 0; i <= 50; i++)
        {
            particulas = Instantiate(prefabParticulas, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            referenciaParaParticulas();
        }
    }

    public void ExplotarFusion()
    {
        for (int i = 0; i <= 150; i++)
        {
            particulas = Instantiate(prefabParticulas, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            referenciaParaParticulas();
        }
    }

    void referenciaParaParticulas()
    {
        p_explosion scriptParticulas = particulas.GetComponent<p_explosion>();
        scriptParticulas.SetObjetoCreador(gameObject);
    }
}
