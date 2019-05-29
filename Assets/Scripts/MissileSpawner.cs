using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    public GameObject missiles;

    private void Start()
    {
        StartCoroutine(SpawnMissiles());
    }

    IEnumerator SpawnMissiles()
    {
        yield return null;
    }
}
