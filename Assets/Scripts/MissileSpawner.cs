using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    public GameObject missiles;
    public GameObject rocketMenu;
    public float minTime;
    public float maxTime;
    public int minSpawns;
    public int maxSpawns;
    public int numberOfBarrage;

    private float randomTime;
    private LightBeam beam;

    private bool rockets = true;
    public bool stop;

    private float timer;

    private void Start()
    {
        beam = FindObjectOfType<LightBeam>();
        StartCoroutine(WaitTimer(SpawnMissiles()));
    }

    private void Update()
    {
        if (beam.gameOver && !stop)
        {
            stop = true;
            StopAllCoroutines();
        }

        if(beam.gameWon && !stop)
        {
            stop = true;
            StopAllCoroutines();
            TheMissiles[] missiles = FindObjectsOfType<TheMissiles>();
            if(missiles != null)
            {
                foreach(TheMissiles missile in missiles)
                {
                    Destroy(missile.gameObject);
                }
            }
        }

        if(rockets && Input.GetKeyDown(KeyCode.R))
        {
            rockets = false;
            Rockets();
        }
    }

    void Rockets()
    {
        rocketMenu.SetActive(true);
        StopAllCoroutines();
    }

    IEnumerator WaitTimer(IEnumerator ie)
    {
        yield return new WaitForSecondsRealtime(10);
        StartCoroutine(ie);
    }

    IEnumerator SpawnMissiles()
    {
        int numberOfSpawns = Random.Range(minSpawns, maxSpawns);
        int i = 0;
        while (i < numberOfSpawns)
        {
            randomTime = Random.Range(minTime, maxTime);
            i++;
            float chance = Random.value;
            if(chance > .5f)
            {
                GameObject missileObj = Instantiate(missiles, new Vector2(transform.position.x, transform.position.y + 2), Quaternion.identity);
                Destroy(missileObj, 5f);
            }
            else
            {
                GameObject missileObj = Instantiate(missiles, new Vector2(transform.position.x, transform.position.y - 2), Quaternion.identity);
                Destroy(missileObj, 5f);
            }

            yield return new WaitForSecondsRealtime(randomTime);
        }

        StartCoroutine(WaitTimer(SpawnBarrage()));
    }

    IEnumerator SpawnBarrage()
    {
        int i = numberOfBarrage;
        while(i > 0)
        {
            i--;
            float chance = Random.value;
            if (chance > .5f)
            {
                Instantiate(missiles, new Vector2(transform.position.x, transform.position.y + 2), Quaternion.identity);
            }
            else
            {
                Instantiate(missiles, new Vector2(transform.position.x, transform.position.y - 2), Quaternion.identity);
            }

            yield return new WaitForSecondsRealtime(1f);
        }

        StartCoroutine(WaitTimer(SpawnMissiles()));
    }
}
