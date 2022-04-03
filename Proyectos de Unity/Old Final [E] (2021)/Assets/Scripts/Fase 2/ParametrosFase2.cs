using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametrosFase2 : MonoBehaviour
{
    public static ParametrosFase2 instance;
    public static int dificultad_acumulada;
    int[] dificultad_enemigo;

    public GameObject[] enemigo;
    int enemigoNumero;



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        enemigo = new GameObject[60];
        dificultad_enemigo = new int[enemigo.Length];
        Debug.Log(enemigo.Length);
    }

    private void Update()
    {
        LimitarNumeroDeEnemigos();
        //Debug.Log("dificultad " + dificultad_acumulada);
    }



    void LimitarNumeroDeEnemigos()
    {
        for (int i = 0; i < enemigo.Length; i++)
        {
            if (enemigo[i] != null)
            {
                var enemyLevel = enemigo[i].GetComponent<EnemyLevel>();
                if (enemyLevel.nivel == 1) dificultad_enemigo[i] = 1;
                if (enemyLevel.nivel == 2) dificultad_enemigo[i] = 2;
                if (enemyLevel.nivel == 3) dificultad_enemigo[i] = 3;
                if (enemyLevel.nivel == 4) dificultad_enemigo[i] = 4;
            }
            else dificultad_enemigo[i] = 0;
        }
        dificultad_acumulada =
            dificultad_enemigo[0] + dificultad_enemigo[1] + dificultad_enemigo[2] + dificultad_enemigo[3] +
            dificultad_enemigo[4] + dificultad_enemigo[5] + dificultad_enemigo[6] + dificultad_enemigo[7];
    }

    public void SetEnemyReference(GameObject _enemigo)
    {
        for (int i = 0; i < enemigo.Length; i++)
        {
            if (enemigoNumero == 16) enemigoNumero = 0;
            if (enemigo[enemigoNumero] != null)
            {
                enemigoNumero++;
            }
            if (enemigo[enemigoNumero] == null)
            {
                enemigo[enemigoNumero] = _enemigo;
                i = enemigo.Length + 1;
            }
        }
    }
}
