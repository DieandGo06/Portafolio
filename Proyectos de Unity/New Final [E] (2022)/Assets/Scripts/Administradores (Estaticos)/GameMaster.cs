using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    [Header("Referencias")]
    public Transform centro;
    public Transform[] satelites = new Transform[3];



    private void Awake()
    {
        instance = this;

        BuscarSatelites();
        centro = GameObject.FindGameObjectWithTag("Centro").transform;
    }

    void BuscarSatelites()
    {
        GameObject satelitesParent = GameObject.FindGameObjectWithTag("Satelites");
        if (satelitesParent != null)
        {
            for (int i = 0; i < satelites.Length; i++)
            {
                satelites[i] = satelitesParent.transform.GetChild(i);
            }
        }
    }
}
