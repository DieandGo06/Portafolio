using UnityEngine;

public class MoveNave : MonoBehaviour
{
    GameManager gameManager;
    SpawnParticulaDeMovimiento spawnParticulas;

    new Transform transform;
    new Rigidbody2D rigidbody2D;

    float moveSpeed = 800;
    public Vector2 limiteX;
    public Vector2 limiteY;

    [HideInInspector] public float angle;
    [HideInInspector] public bool isMoving;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        spawnParticulas = FindObjectOfType<SpawnParticulaDeMovimiento>();
    }

    private void FixedUpdate()
    {
        if (GameManager.fase == 1) MoveOneDimension();
        //if (GameManager.fase == 2) MoveTwoDimension();
        if (GameManager.fase == 2) MoveThreeDimension();
    }

    private void LateUpdate()
    {
        MoveLimits();
    }



    void MoveOneDimension()
    {
        float moveX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        rigidbody2D.AddForce(Vector2.right * moveX * Time.deltaTime);

        if (moveX != 0) isMoving = true;
        else isMoving = false;
    }


    void MoveTwoDimension()
    {
        float moveX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        float moveY = Input.GetAxisRaw("Vertical") * moveSpeed;

        if (moveX != 0) moveY = 0;
        if (moveY != 0) moveX = 0;

        Vector2 move = new Vector2(moveX, moveY);
        rigidbody2D.AddForce(move * Time.deltaTime);

        if (moveX != 0 || moveY != 0) isMoving = true;
        else isMoving = false;
    }


    void MoveThreeDimension()
    {
        float moveX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        float moveY = Input.GetAxisRaw("Vertical") * moveSpeed;

        Vector2 move = new Vector2(moveX, moveY);
        rigidbody2D.AddForce(move * Time.deltaTime);

        MouseRotation();
        spawnParticulas.FollowNave();
        spawnParticulas.CrearParticulas();

        if (moveX != 0 || moveY != 0) isMoving = true;
        else isMoving = false;
    }


    public void MouseRotation()
    {
        Vector3 nave_position = Camera.main.WorldToScreenPoint(transform.position);//Conversion a pixeles

        Vector3 mouse_position = Input.mousePosition;//Posicion en pixeles
        float distanciaX = mouse_position.x - nave_position.x;
        float distanciaY = mouse_position.y - nave_position.y;
        mouse_position.z = 0;

        angle = Mathf.Atan2(distanciaY, distanciaX) * Mathf.Rad2Deg;//Se calcula el angulo entre la nave y mouse

        //"transform.rotation" no es un Vector3 sino un Quaternion.
        //"Quaternion.Euler" hace que los valores de un Vector3(x,y,z) se apliquen igual en el Quaternion. "x" con "x", "y" con "y"...
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 270));

    }

    void MoveLimits()
    {
        //Se crea un vector de posición que limita la posicion donde puede estar la nave
        GetComponent<Transform>().position = new Vector2(
            Mathf.Clamp(transform.position.x, limiteX.x, limiteX.y),
            Mathf.Clamp(transform.position.y, limiteY.x, limiteY.y)
        );
    }
}
