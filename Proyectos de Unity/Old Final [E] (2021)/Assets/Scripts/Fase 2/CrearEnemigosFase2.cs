using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearEnemigosFase2 : MonoBehaviour
{
    public GameObject boss;
    public GameObject enemigoLv1;
    public GameObject enemigoLv2;

    float tiempoTranscurrido;
    float nextSpawn;
    bool isFase2;
    int cooldown = 2;



    private void Start()
    {
        nextSpawn = 1;
        //CrearBoss();
    }

    private void Update()
    {
        CrearEnemigos();
        tiempoTranscurrido += Time.deltaTime;
    }



    void CrearBoss()
    {
        Vector2 posicionInicial = new Vector2(0, 6);
        Vector2 movimientoIncial = new Vector2(0, -500);

        GameObject _boss = Instantiate(boss, posicionInicial, Quaternion.identity);
        _boss.GetComponent<Rigidbody2D>().AddForce(movimientoIncial * Time.deltaTime, ForceMode2D.Impulse);
    }

    void CrearEnemigos()
    {
        if (tiempoTranscurrido > nextSpawn)
        {
            Vector2 posicionInicial = new Vector2(Random.Range(-6, 6), 6);
            if (ParametrosFase2.dificultad_acumulada < ParametrosFase2.instance.enemigo.Length - 1)
            {
                Instantiate(enemigoLv1, posicionInicial, Quaternion.identity);
            }
            
            nextSpawn = tiempoTranscurrido + cooldown;
        }
    }    
}
