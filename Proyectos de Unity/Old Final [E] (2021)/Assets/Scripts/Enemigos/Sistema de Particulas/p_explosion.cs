using UnityEngine;

public class p_explosion : MonoBehaviour
{
    public GameObject objetoCreador;

    float speedX;
    float speedY;
    Vector2 moveSpeed;

    float speed;
    float randomScale;
    float destructionTime;



    void Start()
    {
        Random_Scale();
        Random_Move();

        RandomDestruction();
    }


    

    void Random_Move()
    {
        { /* Primero busca si quien ejecuta "SetObjetoCreador()" es uno de los enemigos,
           * de no ser asi, busca el ejecutor es la bala. */
        } //Comentario
        if (objetoCreador != null)
        {
            var find_ScriptEnemigo = objetoCreador.GetComponent<EnemyLifes>();
            if (find_ScriptEnemigo == true)
            {
                speed = 500;
            }
            else
            {
                var find_ScriptBala = objetoCreador.GetComponent<Bala>();
                if (find_ScriptBala == true)
                {
                    speed = 100;
                }
            }
        }

        speedX = Random.Range(-speed, speed);
        speedY = Random.Range(-speed/1.5f, speed/1.5f);

        moveSpeed = new Vector2(speedX, speedY);
        gameObject.GetComponent<Rigidbody2D>().AddForce(moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
    }


    void Random_Scale()
    {
        if (objetoCreador != null)
        {
            var find_ScriptEnemigo = objetoCreador.GetComponent<Fusion>();
            if (find_ScriptEnemigo == true)
            {
                randomScale = Random.Range(0.3f, 1f);
            }
            else
            {
                var find_ScriptBala = objetoCreador.GetComponent<Bala>();
                if (find_ScriptBala == true)
                {
                    randomScale = Random.Range(0.3f, 0.75f);
                }
            }
        }
        transform.localScale = new Vector3(randomScale, randomScale, 1);
    }


    void RandomDestruction()
    {
        destructionTime = Random.Range(0.3f, 1.2f);
        Destroy(gameObject, destructionTime);
    }


    public void SetObjetoCreador(GameObject objeto)
    {
        objetoCreador = objeto;
    }
}
