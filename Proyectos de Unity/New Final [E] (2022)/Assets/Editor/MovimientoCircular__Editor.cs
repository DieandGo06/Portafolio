using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MovimientoCircular))]
public class MovimientoCircular__Editor : Editor
{
    //Este script solo modifica el inspector de la script
    //El codigo es sacado de: https://www.youtube.com/watch?v=RInUu1_8aGw

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MovimientoCircular _movimientoCircular = (MovimientoCircular)target;

        if (GUILayout.Button("Ubicar en Posicion Inicial"))
        {
            Transform _centro = _movimientoCircular.centro;
            Transform _satelite = _movimientoCircular.satelite;
            _movimientoCircular.radio = MetodosDeExtension.CalcularRadio(_centro, _satelite);
            _movimientoCircular.anguloActual = MetodosDeExtension.Map(_movimientoCircular.rotacionInicial, 0, 360, 0, Mathf.PI * 2);
            _movimientoCircular.MoverAPosicionInicial();
        }
    }
}
