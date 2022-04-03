using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int fase = 2;
    public GameObject parametrosFase1, creadorEnemigosFase1;
    public GameObject parametrosFase2, creadorEnemigosFase2;

    bool isFase1, isFase2;
    bool isFase1Ended;
    bool gameOver;

    VidaNave nave;
    float tiempoTranscurrido, nextCheck;



    private void Start()
    {
        nave = FindObjectOfType<VidaNave>();
    }

    void Update()
    {
        if (fase == 1)
        {
            if (!isFase1)
            {
                isFase1 = true;
                Instantiate(parametrosFase1, Vector2.zero, Quaternion.identity);
                Instantiate(creadorEnemigosFase1, Vector2.zero, Quaternion.identity);
            }
            CheckEnemies();
            if (isFase1Ended) fase = 2;
        }
        if (fase == 2 && isFase2 == false)
        {
            isFase2 = true;
            Instantiate(parametrosFase2, Vector2.zero, Quaternion.identity);
            Instantiate(creadorEnemigosFase2, Vector2.zero, Quaternion.identity);
        }

        if (nave.vidas == 0) Reiniciar();
        tiempoTranscurrido += Time.deltaTime;
        //Debug.Log(tiempoTranscurrido);
        CambiarFase();
    }



    void CheckEnemies()
    {
        if (tiempoTranscurrido > nextCheck)
        {
            GameObject[] listaDeEnemigos = GameObject.FindGameObjectsWithTag("enemigo");
            if (listaDeEnemigos.Length == 0)
            {
                isFase1Ended = true;
            }
            nextCheck += 3;
        }
    }


    public void Reiniciar()
    {
        if (!gameOver)
        {
            gameOver = true;
            Invoke("restart", 2f);
        }
    }

    void CambiarFase()
    {
        if (Input.GetKey("k")) fase = 1;
        if (Input.GetKey("l")) fase = 2;
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        //Debug.Log("fase " + fase);
    }

    void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
