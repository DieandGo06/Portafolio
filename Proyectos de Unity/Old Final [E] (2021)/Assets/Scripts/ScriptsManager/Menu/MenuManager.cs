using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int maxIndex;
    public int index;
    bool keyDown;



    private void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            if (!keyDown)
            {
                if (Input.GetAxisRaw("Vertical") == -1)
                {
                    index++;
                    if (index > maxIndex) index = 0;
                }
                if (Input.GetAxisRaw("Vertical") == 1)
                {
                    index--;
                    if (index < 0) index = maxIndex;
                }
                keyDown = true;
            }
        }
        else keyDown = false;
    }




    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
