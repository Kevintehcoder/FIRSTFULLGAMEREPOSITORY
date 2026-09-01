using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    [SerializeField] GameObject[] doors;
    List<GameObject> enemies = new();

    bool enteredRoom= false;

    Coroutine closeDoorsCoroutine;

    void Start()
    {
        InvokeRepeating(nameof(CheckEnemies), 1f, 0.05f);

        
    }

    private void Update()
    {
        Debug.Log(enemies.Count);
    }

    [ContextMenu("Close Doors")]
    public void CloseDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(true);
        }

       
    }

    [ContextMenu("Open Doors")]
    public void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == 6 || collision.gameObject.layer == 10) && enemies.Contains(collision.gameObject) == false)
        {
            enemies.Add(collision.gameObject);
            Debug.Log(collision.gameObject.transform.parent.name + " entered the room.");
        }

        if (collision.gameObject.layer == 3)
        {
            enteredRoom = true;
            closeDoorsCoroutine = StartCoroutine(closeDoors());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            enteredRoom = false;
            if(closeDoorsCoroutine != null)
            {
                StopCoroutine(closeDoorsCoroutine);
                closeDoorsCoroutine = null;
            }
        }
    }

    void CheckEnemies()
    {
        if (enemies.Count == 0 || enteredRoom == false)
        {
            OpenDoors();
        }

    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    IEnumerator closeDoors()
    {
        yield return new WaitForSeconds(0.8f);

        if(enteredRoom)
        {
            CloseDoors();
        }

        closeDoorsCoroutine = null;
    }

}
