using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonMenu : MonoBehaviour
{
    [SerializeField] int thisIndex;
    [SerializeField] Animator animator;
    [SerializeField] MenuManager menuManager;

    private void Update()
    {
        if (menuManager.index == thisIndex)
        {
            animator.SetBool("isSelected", true);

            if (Input.GetAxis("Submit") == 1)
            {
                animator.SetBool("isPressed", true);
            }
        }
        else animator.SetBool("isSelected", false);
    }

    public void PlayCinmatica()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
