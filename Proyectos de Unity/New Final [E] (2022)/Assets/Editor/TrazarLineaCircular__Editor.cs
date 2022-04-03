using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TrazarLineaCircular))]
public class TrazarLineaCircular__Editor : Editor
{
    //Este script solo modifica el inspector de la script
    //El codigo es sacado de: https://www.youtube.com/watch?v=RInUu1_8aGw

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TrazarLineaCircular trazarLineaCircular = (TrazarLineaCircular)target;

        if (GUILayout.Button("Dibujar Linea"))
        {
            Transform centro = trazarLineaCircular.centro;
            Transform satelite = trazarLineaCircular.satelite;
            trazarLineaCircular.radio = MetodosDeExtension.CalcularRadio(centro, satelite);
            //trazarLineaCircular.rotacionInicial = trazarLineaCircular.GetRotacionInicial();

            trazarLineaCircular.numerosDeVertices =  trazarLineaCircular.CalcularNumeroDeVertices();
            trazarLineaCircular.DibujarCirculo(trazarLineaCircular.numerosDeVertices);
        }
    }
}
