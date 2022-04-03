using UnityEngine;

public class SpawnParticulaDeMovimiento : MonoBehaviour
{
    MoveNave moveNave;
    public GameObject prefab_1;
    public GameObject prefab_2;

    float nextSpawn;
    float contadorDeParticulas;

    Vector2 spawnParticulasPosition;



    private void Start()
    {
        moveNave = FindObjectOfType<MoveNave>();
    }



    public void CrearParticulas()
    {
        if (Time.time > nextSpawn && moveNave.isMoving)
        {
            Instantiate(prefab_1, transform.position, Quaternion.identity);
            contadorDeParticulas++;

            if (contadorDeParticulas > 3)
            {
                Instantiate(prefab_2, transform.position, Quaternion.identity);
                contadorDeParticulas = 0;
            }

            float cooldownSpawn = 0.3f;
            nextSpawn = Time.time + cooldownSpawn;
        }
    }

    public void FollowNave()
    {
        float naveRotation = (moveNave.angle + 270) * Mathf.Deg2Rad;
        Vector2 _navePosition = moveNave.transform.position;

        spawnParticulasPosition = new Vector2(
            _navePosition.x + (0.5f * Mathf.Sin(naveRotation)),
            _navePosition.y - (0.5f * Mathf.Cos(naveRotation))
            );

        transform.position = spawnParticulasPosition;
    }
}