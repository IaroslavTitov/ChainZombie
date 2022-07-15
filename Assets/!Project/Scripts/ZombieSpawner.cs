using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject zombiePrefab;
    public GameObject chainlinkPrefab;

    [Header("Spawn Settings")]
    public List<Transform> spawnPoints;
    public float minimumDistance;
    public float spawnRate;
    public float spawnRateDecay;
    public float minimumSpawnRate;
    public float spawnSprayRadius;
    public float chainBreakForce;

    [Header("Zombie Numbers Settings")]
    public int minZombies;
    public int maxZombies;
    public int minChains;
    public int maxChains;

    private float timer;
    private Transform player;

    private void Start()
    {
        timer = spawnRate;
        player = GameObject.FindObjectOfType<PlayerController>().transform;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        spawnRate = Mathf.Max(spawnRate - spawnRateDecay * Time.deltaTime, minimumSpawnRate);

        if (timer < 0)
        {
            timer += spawnRate;

            // Decide what are we spawning
            int zombieNumber = Random.Range(minZombies, maxZombies+1);
            int chainNumber = Mathf.Min(zombieNumber-1, Random.Range(minChains, maxChains+1));
            List<Vector3> avaliablePositions = spawnPoints.Where(x => Vector2.Distance(x.position, player.position) > minimumDistance).Select(x => x.position).ToList();
            Vector2 basePosition = avaliablePositions[Random.Range(0, avaliablePositions.Count)];

            // Spawn all zombies
            List<Rigidbody2D> spawnedZombies = new List<Rigidbody2D>();
            for (int i = 0; i < zombieNumber; i++)
            {
                Vector2 spawnPosition = basePosition + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * spawnSprayRadius;
                spawnedZombies.Add(Instantiate(zombiePrefab, spawnPosition, Quaternion.identity).GetComponent<Rigidbody2D>());
            }

            // Spawn all chains
            List<Tuple<int, int>> chainDirections = new List<Tuple<int, int>>();
            for (int i = 0; i < chainNumber; i++)
            {
                int startIndex = -1, endIndex = -1;
                while (startIndex == endIndex
                    || chainDirections.Contains(new Tuple<int, int>(startIndex, endIndex))
                    || chainDirections.Contains(new Tuple<int, int>(endIndex, startIndex)))
                {
                    startIndex = Random.Range(0, spawnedZombies.Count);
                    endIndex = Random.Range(0, spawnedZombies.Count);
                }

                chainDirections.Add(new Tuple<int ,int>(startIndex, endIndex));

                ChainGenerator.GenerateChain(chainlinkPrefab, spawnedZombies[startIndex], spawnedZombies[endIndex], ChainGenerator.characterOffset, ChainGenerator.characterOffset, chainBreakForce);
            }
        }
    }
}
