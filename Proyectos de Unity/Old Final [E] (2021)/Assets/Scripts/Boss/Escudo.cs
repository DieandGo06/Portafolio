using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escudo : MonoBehaviour
{
    [SerializeField] Animator animator;
    int vidas;

    private void Start()
    {
        vidas = 100;
    }

    private void Update()
    {
        Destruir();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "naveBala")
        {
            vidas--;
        }
    }



    void ChangeToIdle()
    {
        animator.SetBool("isEscudoIdle", true);
    }

    void Destruir()
    {
        if (vidas <= 0)
        {
            Destroy(gameObject);
            Debug.Log(Time.time);
        }
    }

}
