using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBalas : MonoBehaviour
{
    //Publicas
    public GameObject balaPrefab;
    public Transform almacenDeBalas;
    [Range(0.1f, 0.8f)]public float cooldown;

    [Space(20)]
    public GameObject[] balas;


    //Priavdas
    int ultimaBalaUsada;
    int cantidadBalas = 50;
    float nextShoot;


    private void Awake()
    {
        balas = new GameObject[cantidadBalas];
        nextShoot = cooldown;
        CrearBalas();
    }

    void LateUpdate()
    {
       DisparoAutomatico();
    }



    void DisparoAutomatico()
    {
        if (Time.time > nextShoot)
        {
            balas[ultimaBalaUsada].transform.position = this.transform.position;
            balas[ultimaBalaUsada].transform.rotation = this.transform.rotation;
            balas[ultimaBalaUsada].GetComponent<BalaNave>().canMove = true;
            ultimaBalaUsada++;

            nextShoot = Time.time + cooldown;
            if (ultimaBalaUsada == balas.Length) ultimaBalaUsada = 0;
        }
    }

    void CrearBalas()
    {
        int fila, columna;
        float espaciadoX = 0.4f;
        float espaciadoY = -0.8f;
        Vector2 origenPosition = new Vector2(11, 4.8f);
        
        for (int i=0; i < balas.Length; i++)
        {
            if (i <= 9) fila = 0;
            else if (i > 9 && i <= 19) fila = 1;
            else if (i > 19 && i <= 29) fila = 2;
            else if (i > 29 && i <= 39) fila = 3;
            else fila = 4;
            columna = i - (fila * 10);

            float posXInicial = origenPosition.x + (columna * espaciadoX);
            float posYInicial = origenPosition.y + fila * espaciadoY;

            Vector3 posicionInicial = new Vector3(posXInicial, posYInicial, 0);
            GameObject _bala = Instantiate(balaPrefab, posicionInicial, Quaternion.identity, almacenDeBalas);
            _bala.GetComponent<BalaNave>().GetPosicionInicial(posicionInicial);
            balas[i] = _bala;
            
            //_bala.SetActive(false);
        }
    }

    void PlaySonidoDisparo()
    {
        /*
        if (AudioManager.instance != null)
        {
            AudioManager.PlaySound(AudioManager.instance.disparoBala);
        }
        */
    }
}
