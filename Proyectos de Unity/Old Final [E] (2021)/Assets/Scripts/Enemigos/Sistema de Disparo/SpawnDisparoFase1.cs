using UnityEngine;

public class SpawnDisparoFase1 : MonoBehaviour
{
    public GameObject naranja, rojo;
    Disparo[] scriptDisparo = new Disparo[2];

    float speed = 2;
    float cooldown;
    float nextShoot;
    float tiempoTranscurrido;
    


    private void Awake()
    {
        if (GameManager.fase == 1) GetComponent<SpawnDisparoFase1>().enabled = true;
        if (GameManager.fase != 1) GetComponent<SpawnDisparoFase1>().enabled = false;
    }

    private void Start()
    {
        scriptDisparo[0] = naranja.GetComponent<Disparo>();
        scriptDisparo[1] = rojo.GetComponent<Disparo>();
        nextShoot = Random.Range(2f, 10f);
    }

    private void Update()
    {
        if (GetComponentInParent<EnemyLevel>().nivel == 1) 
            DispararLv1();

        if (GetComponentInParent<EnemyLevel>().nivel == 2)
            DispararLv2();

        tiempoTranscurrido += Time.deltaTime;
    }



    void CrearDisparo(GameObject color)
    {
        Instantiate(color, transform.position, Quaternion.identity);
    }

    void DispararLv1()
    {
        cooldown = 6;
        if (tiempoTranscurrido > nextShoot)
        {
            scriptDisparo[0].directionX = Random.Range(-speed, speed);
            scriptDisparo[0].directionY = -speed;
            CrearDisparo(naranja);

            nextShoot = tiempoTranscurrido + cooldown;
        }
    }

    void DispararLv2()
    {
        cooldown = 4;
        float randomDirectionX = Random.Range(-speed, speed);

        if (tiempoTranscurrido > nextShoot)
        {
            scriptDisparo[0].directionX = randomDirectionX;
            scriptDisparo[0].directionY = -speed;
            CrearDisparo(naranja);

            scriptDisparo[1].directionX = -randomDirectionX;
            scriptDisparo[1].directionY = -speed;
            CrearDisparo(rojo);

            nextShoot = tiempoTranscurrido + cooldown;
        }
    }
}
