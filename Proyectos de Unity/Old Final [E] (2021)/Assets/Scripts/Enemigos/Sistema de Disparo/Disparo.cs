using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparo : MonoBehaviour
{
    Transform m_transform;

    [HideInInspector] public float directionX;
    [HideInInspector] public float directionY;
    public Animator animator;
    Vector2 move;


    
    void Start()
    {
        m_transform = gameObject.GetComponent<Transform>();
    }

    private void Update()
    {
        Destroy(gameObject, 10);
    }

    private void FixedUpdate()
    {
        move = new Vector2(directionX, directionY);
        m_transform.Translate(move * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "naveBala")
        {
            directionX = 0;
            directionY = 0;
            animator.SetBool("exploto", true);

            Destroy(gameObject, 0.3f);
        }
        int naveLayer = 12;
        if (collision.gameObject.layer == naveLayer) Destroy(gameObject);
    }
}
