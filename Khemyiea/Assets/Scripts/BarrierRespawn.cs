using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushRespawn : MonoBehaviour
{
    public GameObject bushPrefabPath;
    public float maxBush;
    public float spawnRadius;
    public float spawnCheckTime;

    private float lastSpawnCheckTime;
    private List<GameObject> curBush = new List<GameObject>();

    void Update()
    {
        if (Time.time - lastSpawnCheckTime > spawnCheckTime)
        {
            lastSpawnCheckTime = Time.time;
            TrySpawn();
        }
    }

    void TrySpawn()
    {
        // remove any dead bushes from the curBush list
        for (int x = 0; x < curBush.Count; ++x)
        {
            if (!curBush[x])
                curBush.RemoveAt(x);
        }
        // if we have maxed out our bushes, return
        if (curBush.Count >= maxBush)
            return;
        // otherwise, spawn an bush
        Vector3 randomInCircle = Random.insideUnitCircle * spawnRadius;
        GameObject bush = Instantiate(bushPrefabPath, transform.position + randomInCircle, Quaternion.identity);
        curBush.Add(bush);
    }
}
