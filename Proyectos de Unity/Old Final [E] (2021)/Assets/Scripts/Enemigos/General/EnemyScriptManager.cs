using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptManager : MonoBehaviour
{
    Fusion fusion;
    MoveEnemyFase1 moveEnemyFase1;
    MoveEnemyFase2 moveEnemyFase2;
    SpawnDisparoFase1 spawnDisparoFase1;
    SpawnDisparoFase2 spawnDisparoFase2;



    private void Awake()
    {
        fusion = GetComponent<Fusion>();
        moveEnemyFase1 = GetComponent<MoveEnemyFase1>();
        moveEnemyFase2 = GetComponent<MoveEnemyFase2>();
        spawnDisparoFase1 = GetComponentInChildren<SpawnDisparoFase1>();
        spawnDisparoFase2 = GetComponentInChildren<SpawnDisparoFase2>();
        fusion.enabled = true;
    }

    private void Start()
    {
        ActivarScriptsFase1();
        ActivarScriptsFase2();
    }


    void ActivarScriptsFase1()
    {
        if (GameManager.fase == 1)
        {
            moveEnemyFase1.enabled = true;
            spawnDisparoFase1.enabled = true;

            moveEnemyFase2.enabled = false;
            spawnDisparoFase2.enabled = false;
            ParametrosFase1.instance.SetNuevoEnemigo(gameObject);
        }
    }

    void ActivarScriptsFase2()
    {
        if (GameManager.fase == 2)
        {
            moveEnemyFase2.enabled = true;
            spawnDisparoFase2.enabled = true;

            if (GetComponent<EnemyLevel>().nivel == 1 || GetComponent<EnemyLevel>().nivel == 2)
            {
                moveEnemyFase1.enabled = false;
                spawnDisparoFase1.enabled = false;
            }
            //Genera un error cuando el enemigo lo agregas desde el inspector y no lo crea "ParametrosFase2"
            ParametrosFase2.instance.SetEnemyReference(gameObject);
        }
    }
}
