using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int SpawnAmount;
    public int CurrentWave;
    public float SpawnInterval;
    public float WaveInterval;

    private Dictionary<string, GameObject> Enemies = new Dictionary<string, GameObject>();

    private float WaveCountdown;


    // Start is called before the first frame update
    void Start()
    {
        SpawnAmount = 5;
        CurrentWave = 0;
        SpawnInterval = 0.5f;
        WaveInterval = 5.0f;

        var test = Resources.LoadAll<GameObject>("Prefabs/Enemies");

        // Load all enemy prefabs and store them
        foreach (GameObject G in test)
        {
            Enemies.Add(G.name, G);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (WaveCountdown <= 0.0f)
        {
            StartCoroutine(SpawnWave());
            WaveCountdown = WaveInterval;
        }

        WaveCountdown -= Time.deltaTime;
    }

    // Spawn the next wave
    IEnumerator SpawnWave()
    {
        Debug.Log("Spawn");
        CurrentWave++;

        for (int i = 0; i < SpawnAmount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(SpawnInterval);
        }
    }

    // Spawns a given enemy
    private void SpawnEnemy()
    {
        Instantiate(Enemies["Enemy1"], transform.position, transform.rotation);
    }
}
