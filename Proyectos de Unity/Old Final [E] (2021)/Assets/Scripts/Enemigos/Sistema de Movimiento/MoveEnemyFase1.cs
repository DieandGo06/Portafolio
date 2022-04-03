using UnityEngine;

public class MoveEnemyFase1 : MonoBehaviour
{
    ParametrosFase1 parametrosFase1;
    Vector2 limiteX = new Vector2(-8f, 8f);
    Vector2 limiteY = new Vector2(-9f, 9f);
    new Rigidbody2D rigidbody2D;

    float speed;

    //Movimiento Inicial
    static bool isEnemigosPosicionados;
    Vector2 posicionInicial;


    
    private void Start()
    {
        parametrosFase1 = ParametrosFase1.instance;
        rigidbody2D = GetComponent<Rigidbody2D>();
        posicionInicial = transform.position;
        SetVelocidad();
    }

    private void FixedUpdate()
    {
        MovimientoInicial();
        if (parametrosFase1.isMoverEnemigos)
        {
            MovimientoHorizontal();
        }
        if (parametrosFase1.isGenerarFusiones)
        {
            MoveToFusion();
        }
        Debug.Log(isEnemigosPosicionados);
        Limites();
    }



    void SetVelocidad()
    {
        var enemyLevel = GetComponent<EnemyLevel>();
        if (enemyLevel.nivel == 1) speed = 1f;
        if (enemyLevel.nivel == 2) speed = 1f;
    }

    void MovimientoInicial()
    {
        float speed = 1f;
        float descenso = 4;
        Vector2 posicionFinal = new Vector2(posicionInicial.x, posicionInicial.y - descenso);
        Vector2 move = Vector2.Lerp(transform.position, posicionFinal, speed * Time.deltaTime);

        if (isEnemigosPosicionados == false)
        {
            rigidbody2D.MovePosition(move);
        }
        if (transform.position.y <= 0.2)
        {
            isEnemigosPosicionados = true;
        }
    }


    void MovimientoHorizontal()
    {
        for (int i = 0; i < parametrosFase1.enemigo.Length; i++)
        {
            if (parametrosFase1.enemigo[i] != null)
            {
                if (transform.position.x == limiteX.x)
                    ParametrosFase1.moveEnemyDirection = 1;
                if (transform.position.x == limiteX.y)
                    ParametrosFase1.moveEnemyDirection = -1;
            }
        }
        Vector3 movimiento = new Vector3(speed * ParametrosFase1.moveEnemyDirection, 0, 0);
        transform.Translate(movimiento * Time.deltaTime);
    }



    void MoveToFusion()
    {
        for (int i = 0; i < parametrosFase1.enemigo.Length; i++)
        {
            var _enemigo = parametrosFase1.enemigo[i];
            if (gameObject == _enemigo)
            {
                //Se dividen entre pares e impares. Unos se mueven mientras el otro es estatico. Esto reduce un poco la desaliceacion.
                if (i >= 0 && i < 8 && i % 2 == 0)
                    transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
                if (i >= 0 && i < 8 && i % 2 != 0)
                    rigidbody2D.bodyType = RigidbodyType2D.Static;

                if (i >= 8 && i < 16 && i % 2 == 0)
                    rigidbody2D.bodyType = RigidbodyType2D.Static;
                if (i >= 8 && i < 16 && i % 2 != 0)
                    transform.Translate(new Vector3(-speed, 0, 0) * Time.deltaTime);

                if (i >= 16 && i < 24 && i % 2 == 0)
                    transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
                if (i >= 16 && i < 24 && i % 2 != 0)
                    rigidbody2D.bodyType = RigidbodyType2D.Static;

                if (i >= 24 && i < 32 && i % 2 == 0)
                    rigidbody2D.bodyType = RigidbodyType2D.Static;
                if (i >= 24 && i < 32 && i % 2 != 0)
                    transform.Translate(new Vector3(-speed, 0, 0) * Time.deltaTime);
            }
        }
    }

    void Limites()
    {
        GetComponent<Transform>().position = new Vector2(
            Mathf.Clamp(transform.position.x, limiteX.x, limiteX.y),
            Mathf.Clamp(transform.position.y, limiteY.x, limiteY.y)
        );
    }
}

/*void MoveToFusion()
    {
        for (int i = 0; i < parametrosFase1.enemigo.Length; i++)
        {
            var _enemigo = parametrosFase1.enemigo[i];
            if (gameObject == _enemigo)
            {
                //Se dividen entre pares e impares. Unos se mueven mientras el otro se vuelve estatico
                if (i >= 0 && i < 8 && i % 2 == 0)
                    transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);

                if (i >= 8 && i < 16 && i % 2 != 0)
                    transform.Translate(new Vector3(-speed, 0, 0) * Time.deltaTime);

                if (i >= 16 && i < 24 && i % 2 == 0)
                    transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);

                if (i >= 24 && i < 32 && i % 2 != 0)
                    transform.Translate(new Vector3(-speed, 0, 0) * Time.deltaTime);
            }
        }
    }
*/