using UnityEngine;

public class CrearEnemigosFase1 : MonoBehaviour
{
    [SerializeField] GameObject prefabEnemigo;
    ParametrosFase1 parametrosFase1;

    int contadorEnemigos;
    int candtidadEnemigos = 32;



    private void Start()
    {
        parametrosFase1 = ParametrosFase1.instance;
        CrearEnemigos();
        CambiarNombres();
        Destroy(gameObject);
    }

    void CrearEnemigos()
    {
        GameObject grupoDeEnemigos = new GameObject("Grupo de Enemigos");
        for (int k = 0; k < 4; k++)
        {
            for (int i = -4; i < 4; i++)
            {
                GameObject _enemigo = Instantiate(prefabEnemigo, CalcularPosicion(k,i), Quaternion.identity);
                parametrosFase1.enemigo[contadorEnemigos] = _enemigo;
                _enemigo.transform.parent = grupoDeEnemigos.transform;
                contadorEnemigos++;
            }
        }
    }

    Vector2 CalcularPosicion(int _fila, int _columna)
    {
        float margenX = 0.9f;
        float margenY = 4;

        float distanciaXEnemigos = 1.8f;
        float distanciaYEnemigos = 1.25f;

        float posicionX = (distanciaXEnemigos * _columna) + margenX;
        float posicionY = (distanciaYEnemigos * _fila) + margenY;

        Vector2 posicionEnemigo = new Vector2(posicionX, posicionY);
        return posicionEnemigo;
    }


    void CambiarNombres()
    {
        for (int i = 0; i < candtidadEnemigos; i++)
        {
            parametrosFase1.enemigo[i].GetComponent<Transform>().name = "enemigo " + i;
        }
    }
}
