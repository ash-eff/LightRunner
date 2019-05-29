using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    public GameObject missiles;
    public float minTime;
    public float maxTime;
    public int minSpawns;
    public int maxSpawns;
    public int numberOfBarrage;

    private float randomTime;

    private void Start()
    {
        StartCoroutine(WaitTimer(SpawnMissiles()));
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
                Instantiate(missiles, new Vector2(transform.position.x, transform.position.y + 2), Quaternion.identity);
            }
            else
            {
                Instantiate(missiles, new Vector2(transform.position.x, transform.position.y - 2), Quaternion.identity);
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
