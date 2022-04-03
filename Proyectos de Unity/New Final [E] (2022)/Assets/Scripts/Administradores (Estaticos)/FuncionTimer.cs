using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* "FuncionTimer" y "AdministradorTareas" funciona muy similar.
 * 
 * FuncionTimer: crea un objeto al que se le paso una funcion que ejecutara pasado cierto tiempo y luego destruirse
 * AdministradorTareas: hace lo mismo pero guardando las acciones en una lista dentro de un solo objeto
 * 
 * A AdministradorTareas le agregue un metodo para que puediera ejecutar un funcion durante "x" tiempo
 * antes de sacarlo de la lista.
 */



//Este codigo lo saque de: https://www.youtube.com/watch?v=1hsppNzx7_0
//Tutorial complementario: https://www.youtube.com/watch?v=TdiN18PR4zk
public class FuncionTimer
{
    private Action accion;
    private float timer;
    private bool isDestroyed;
    private GameObject objeto;

    //Constructor de la clase
    private FuncionTimer(Action _accion, float _timer, GameObject _gameObject)
    {
        accion = _accion;
        timer = _timer;
        objeto = _gameObject;
        isDestroyed = false;
    }

    //No es un update de unity, sino un metodo cualquiera
    public void Update()
    {
        if (!isDestroyed)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                accion();
                DestuirTimer();
            }
        }
    }

    void DestuirTimer()
    {
        isDestroyed = true;
        UnityEngine.Object.Destroy(objeto);
    }


    //--------------------------------------------------------------------------------------
    //Se usa para poder ejecutar las funciones de Monobehaviour, como Update.
    public class EnlaceConMonoBehaviour : MonoBehaviour
    {
        public Action accionEnUpdate;
        private void Update()
        {
            if (accionEnUpdate != null) accionEnUpdate();
        }
    }


    //--------------------------------------------------------------------------------------
    public static FuncionTimer Crear(Action _accion, float _timer)
    {
        //Se crea un gameObject que ejecutara la funcion del timer
        GameObject _gameObject = new GameObject("funcionTimer", typeof(EnlaceConMonoBehaviour));
        //Se crea un construtuctor de "FuncionTimer" para poder usar su metodo "Update"
        FuncionTimer constructor = new FuncionTimer(_accion, _timer, _gameObject);
        //Y paso el metodo "Update" del contructor como la accion a la script con MonoHebaviour
        _gameObject.GetComponent<EnlaceConMonoBehaviour>().accionEnUpdate = constructor.Update;

        return constructor;
    }
}
