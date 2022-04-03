using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemyFase2 : MonoBehaviour
{
    EnemyLevel enemyLevel;

    Vector2 moveSpeed;
    [SerializeField] Vector2 limiteX;
    [SerializeField] Vector2 limiteY;

    float cooldown;
    float nextMove;
    float tiempoTranscurrido;



    private void Start()
    {
        nextMove = 1f;
        enemyLevel = GetComponent<EnemyLevel>();
        if (enemyLevel.nivel == 1) cooldown = 1f;
        if (enemyLevel.nivel == 2) cooldown = 2f;
        if (enemyLevel.nivel == 3) cooldown = 4f;
        StartMove();
    }

    private void Update()
    {
        limites();
        tiempoTranscurrido += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (enemyLevel.nivel == 1 || enemyLevel.nivel == 2 || enemyLevel.nivel == 3)
        {
            MoveEnemy();
        }
    }



    void StartMove()
    {
        if (transform.position.y >= 4.75)
        {
            moveSpeed = new Vector2(Random.Range(-200, 200), Random.Range(-50, -200));
            GetComponent<Rigidbody2D>().AddForce(moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    void MoveEnemy()
    {
        if (tiempoTranscurrido > nextMove)
        {
            SetDirection(new Vector2(300, 200));
            GetComponent<Rigidbody2D>().AddForce(moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
            nextMove = tiempoTranscurrido + cooldown;
        }
    }

    void SetDirection(Vector2 randomSpeed)
    {
        float pre_limiteX = 6.5f;
        var enemyPosition = transform.position;
        

        //Cambio de direccion en X
        if (enemyPosition.x <= pre_limiteX && enemyPosition.x >= -pre_limiteX)
        {
            moveSpeed.x = Random.Range(-randomSpeed.x, randomSpeed.x);
        }
        else if (enemyPosition.x > pre_limiteX)
        {
            moveSpeed.x = Random.Range(-100, -randomSpeed.x);
        }
        else if (enemyPosition.x < pre_limiteX)
        {
            moveSpeed.x = Random.Range(100, randomSpeed.x);
        }

        //Cambio de direccion en Y
        if (enemyPosition.y <= 3.5f && enemyPosition.y >= 0.5f)
        {
            moveSpeed.y = Random.Range(-randomSpeed.y, randomSpeed.y);
        }
        else if (enemyPosition.y > 3.5f)
        {
            moveSpeed.y = Random.Range(-100, -randomSpeed.x);
        }
        else if (enemyPosition.y < 0.5f)
        {
            moveSpeed.y = Random.Range(0, randomSpeed.y);
        }
    }

    void limites()
    {
        //Se crea un vector que limita la posicion donde puede estar la nave
        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x, limiteX.x, limiteX.y),
            Mathf.Clamp(transform.position.y, limiteY.x, limiteY.y)
        );
    }
}
