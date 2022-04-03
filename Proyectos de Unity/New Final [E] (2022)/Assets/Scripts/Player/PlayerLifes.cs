using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifes : MonoBehaviour
{
    public int vidas;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrera"))
        {
            PerderVida();
            //Debug.Log(vidas);
        }
    }

    void PerderVida()
    {
        vidas--;
    }
}
