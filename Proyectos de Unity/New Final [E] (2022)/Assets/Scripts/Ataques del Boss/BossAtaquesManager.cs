using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossAtaquesManager : MonoBehaviour
{
    public List<Barrera> barreasOrbita1 = new List<Barrera>();
    public List<Barrera> barreasOrbita2 = new List<Barrera>();
    public List<Barrera> barreasOrbita3 = new List<Barrera>();


    public float duracionDelAtaque;
    [Range(0.1f, 3f)] public float duracionPreview;
    [Range(0.1f, 1f)] public float duracionFades;
    public float cooldown;

    public bool inicioAtaque, atacando;
    public int grupoActual;



    private void Awake()
    {
        //Busca todos los gamesObject barreras
        GameObject[] barrerasObject = GameObject.FindGameObjectsWithTag("Barrera");

        //Los organiza por orbita 
        for (int i = 0; i < barrerasObject.Length; i++) {
            if (barrerasObject[i].GetComponent<MovimientoCircular>().orbita == 1) {
                barreasOrbita1.Add(barrerasObject[i].GetComponent<Barrera>());
            }
            else if (barrerasObject[i].GetComponent<MovimientoCircular>().orbita == 2) {
                barreasOrbita2.Add(barrerasObject[i].GetComponent<Barrera>());
            }
            else if (barrerasObject[i].GetComponent<MovimientoCircular>().orbita == 3) {
                barreasOrbita3.Add(barrerasObject[i].GetComponent<Barrera>());
            }
        }
    }

    private void Start()
    {
        ApagarGrupo(0);
        ApagarGrupo(1);
        ReiniciarAtaque();
    }


    private void Update()
    {
        if (!inicioAtaque) {
            ActivarGrupo(grupoActual);
            inicioAtaque = true;
            atacando = true;
        }

        if (atacando) {
            if (!estaActivoGrupo(grupoActual)) {
                Tareas.Nueva(cooldown, CambiarGrupo);
                Tareas.Nueva(cooldown, ReiniciarAtaque);
                atacando = false;
            }
        }

    }



    void ActivarGrupo(int index)
    {
        ReactivarGrupo(index);
        SetDuracionGrupo(index, duracionDelAtaque);
        SetFadeDurationGrupo(index, duracionFades);
        SetPreviewDurationGrupo(index, duracionPreview);
    }

    void CambiarGrupo()
    {
        grupoActual++;
        if (grupoActual >= 2) {//Maximo de ataques
            grupoActual = 0;
        }
    }

    void ReiniciarAtaque()
    {
        inicioAtaque = false;
        atacando = false;
    }

    void ReactivarGrupo(int index)
    {
        barreasOrbita1[index].ReActivar();
        barreasOrbita2[index].ReActivar();
        barreasOrbita3[index].ReActivar();
    }









    void SetDuracionGrupo(int index, float duracion)
    {
        barreasOrbita1[index].duracionActivo = duracion;
        barreasOrbita2[index].duracionActivo = duracion;
        barreasOrbita3[index].duracionActivo = duracion;
    }

    void SetPreviewDurationGrupo(int index, float duracion)
    {
        barreasOrbita1[index].duracionPreview = duracion;
        barreasOrbita2[index].duracionPreview = duracion;
        barreasOrbita3[index].duracionPreview = duracion;
    }

    void SetFadeDurationGrupo(int index, float duracion)
    {
        barreasOrbita1[index].duracionFades = duracion;
        barreasOrbita2[index].duracionFades = duracion;
        barreasOrbita3[index].duracionFades = duracion;
    }


    void ApagarGrupo(int index)
    {
        barreasOrbita1[index].gameObject.SetActive(false);
        barreasOrbita2[index].gameObject.SetActive(false);
        barreasOrbita3[index].gameObject.SetActive(false);
    }

    bool estaActivoGrupo(int index)
    {
        if (barreasOrbita1[index].gameObject.activeSelf &&
            barreasOrbita2[index].gameObject.activeSelf &&
            barreasOrbita3[index].gameObject.activeSelf) {
            return true;
        }
        else return false;
    }







}
