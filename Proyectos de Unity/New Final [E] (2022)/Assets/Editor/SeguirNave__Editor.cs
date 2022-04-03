using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SeguirNave))]
public class SeguirNave__Editor : Editor
{
    //Este script solo modifica el inspector de la script
    //El codigo es sacado de: https://www.youtube.com/watch?v=RInUu1_8aGw

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SeguirNave _seguirNave = (SeguirNave)target;

        if (GUILayout.Button("Ubicar en Posicion Inicial"))
        {
            Transform _centro = _seguirNave.centro;
            Transform _satelite = _seguirNave.satelite;
            _seguirNave.radio = MetodosDeExtension.CalcularRadio(_centro, _satelite);

            #region Explicacion: Referencias en editor scripts y movimiento
            /* Las funciones ejecutadas en el inspector no son in-game, por lo que muchas referencias 
             * que son buscadas en el "awake" son "null".
             * Por ello, hay que buscar todas las referencias necesarias en el script "editor"
             *
             * Por otro lado, creo que el script "editor" solo puede mover objetos con "transform"
             * y no con rigidbodies. Por ello tuve que duplicar la funcion que genera el movimeinto circular
             * pero con "transform.position" enves de "rigidbody.MovePostion"
             */
            #endregion
            PlayerMove naveScript = FindObjectOfType<PlayerMove>();
            _seguirNave.anguloActual = naveScript.rotacionInicial * Mathf.Deg2Rad;
            _seguirNave.MoverAPosicionInicial();
        }
    }

}