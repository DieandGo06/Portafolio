using System.Collections;
using UnityEngine;

public static class MetodosDeExtension
{
    //La idea de metodos de extension viene de:
    //https://www.youtube.com/watch?v=E7b3ZNmhbnU&t=12s 


    static public float CalcularRadio(Transform centro, Transform satelite)
    {
        //Realmente no es un metodo de extension, sino solo una funcion estatica
        float distanciaX = satelite.position.x - centro.position.x;
        float distanciaY = satelite.position.y - centro.position.y;
        float _radio = new Vector2(distanciaX, distanciaY).magnitude;
        return _radio;
    }

    //Codigo sacado de: https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
    public static float Map(float value, float minEscala1, float maxEscala1, float minEscala2, float maxEscala2)
    {
        return (value - minEscala1) / (maxEscala1 - minEscala1) * (maxEscala2 - minEscala2) + minEscala2;
    }


    #region Movimiento Circular
    //Codigo sacado de: https://www.youtube.com/watch?v=BGe5HDsyhkY
    static public Vector2 PolaresToRectangulares(float radio, float angulo, Transform centro)
    {
        #region Formulas
        /* Cordenadas Polares a rectangulares:
         * x = radio * cos(angulo)
         * y = radio * sin(angulo)
         * Se suma la posicion del gameObject para que si se mueve el centro, tambien lo haga la recta
         * 
         * Si se quiere un movimiento eliptico, debe haber un radioX, y un radioY que tomen en cuenta el ancho y largo de la elipisis
         */
        #endregion
        #region Explicacion: angulos
        /* Como el "sin" y "cos" son operaciones ciclicas, no importa el valor
         * que tome la variable de rotacion. Tambien, al multiplicarse con la 
         * velocidad, si no se mueve, el valor es cero y suma a la rotacion. */
        #endregion
        float posX = Mathf.Cos(angulo) * radio + centro.position.x;
        float posY = Mathf.Sin(angulo) * radio + centro.position.y;
        return new Vector2(posX, posY);
    }

    static public float MoverAngulo(float angulo, float velocidad, int direccion)
    {
        return angulo + (Time.fixedDeltaTime * (velocidad * direccion));
    }
    #endregion
}
