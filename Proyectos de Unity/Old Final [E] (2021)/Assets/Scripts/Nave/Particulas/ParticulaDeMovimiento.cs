using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticulaDeMovimiento : MonoBehaviour
{
    float transparencia = 1;


    private void Start()
    {
        MoveParticula();
        ChangeScale();
    }

    void Update()
    {
        FadeOut();
        Destroy(gameObject, 1f);
    }

    void MoveParticula()
    {
        float moveSpeed = 0.5f;
        float moveX = Random.Range(-moveSpeed, moveSpeed);
        float moveY = Random.Range(-moveSpeed, moveSpeed);

        Vector2 move = new Vector2(moveX, moveY);
        GetComponent<Rigidbody2D>().AddForce(move, ForceMode2D.Impulse);
    }

    void FadeOut()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, transparencia-= Time.deltaTime);
    }

    void ChangeScale()
    {
        float randomScale = Random.Range(1f, 1.8f);
        Vector3 scale = new Vector3(randomScale, randomScale, 1);
        GetComponent<Transform>().localScale = scale;
    }
}
