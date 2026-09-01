using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    Coroutine ProgressLevelCoroutine;
    bool PlayerEntered = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        { 
            PlayerEntered = true;

            if (ProgressLevelCoroutine == null)
            {
                ProgressLevelCoroutine = StartCoroutine(CheckforPlayer());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer ==3)
        {
            PlayerEntered=false;

            if (ProgressLevelCoroutine != null)
            {
                StopCoroutine(ProgressLevelCoroutine);
                ProgressLevelCoroutine = null;
            }
        }
    }

    IEnumerator CheckforPlayer()
    {
        yield return new WaitForSeconds(5);
        if (PlayerEntered == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        ProgressLevelCoroutine = null;
    }
}
