using UnityEngine;

public class VidaNave : MonoBehaviour
{
    public float vidas;
    [SerializeField] GameObject explosion;

    public Sprite[] n_vidas = new Sprite[3];
    Sprite sprite_activo;
    float canHitTimer;
    float tiempoInvulnerable;



    private void Start()
    {
        vidas = 3;
        tiempoInvulnerable = 0.5f;
    }

    private void Update()
    {
        LifeSystem();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject  &&  Time.time > canHitTimer)
        {
            vidas--;

            GameObject _explosion = Instantiate(explosion, transform.position, Quaternion.identity);
            _explosion.transform.parent = gameObject.transform;
            Destroy(_explosion, 1);

            AudioManager.PlayExplosion(AudioManager.instance.hitNave);
            canHitTimer = Time.time + tiempoInvulnerable;
        }
        Destroy(collision.gameObject);
    }


    void LifeSystem()
    {
        var animator = GetComponent<Animator>();
        if (vidas == 3)
        {
            sprite_activo = n_vidas[0];
        }
        if (vidas == 2)
        {
            animator.SetBool("isLife2", true);
            sprite_activo = n_vidas[1];
        }
        if (vidas == 1)
        {
            animator.SetBool("isLife1", true);
            sprite_activo = n_vidas[2];
        }
        if (vidas == 0)
        {
            GetComponent<MoveNave>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponentInChildren<SpawnBala>().enabled = false;
            Destroy(gameObject, 1f);
        }
        GetComponent<SpriteRenderer>().sprite = sprite_activo;
    }
}
