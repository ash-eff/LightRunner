using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviour
{
    public GameObject[] planets;
    public GameObject startText;
    public Camera cam;

    private bool started;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S) && !started)
        {
            started = true;
            StartCoroutine(StartGame());
        }
    }

    IEnumerator StartGame()
    {
        startText.SetActive(false);
        cam.GetComponent<Animator>().enabled = true;
        float timer = 2f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            foreach(GameObject planet in planets)
            {
                planet.transform.Translate(Vector2.right * 100 * Time.deltaTime);
            }

            yield return null;
        }

        Debug.Log("Load Game");
        SceneManager.LoadScene(1);
    }

}
