using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMathSpawner : MonoBehaviour
{
    public GameObject spikePrefab;
    public ScoreScript score;
    public GameOverScript dead;
    public float spawnRate = 2f;
    public float minY = -2f;
    public float maxY = 2f;
    public int poolSize = 5;
    public float spikeSpeed = 2f;

    private List<GameObject> pipePool;
    private float lastSpawnTime = 0f;
    private int currentPipeIndex = 0;

    public Transform planeTransform; 
    public float spikeWidth = 1f; 

    void Start()
    {
        InitializePipePool();
        score = GameObject.FindGameObjectWithTag("Logic").GetComponent<ScoreScript>();
        dead = GameObject.FindGameObjectWithTag("Logic").GetComponent<GameOverScript>();
    }

    void InitializePipePool()
    {
        pipePool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject spike = Instantiate(spikePrefab, new Vector3(10f, 0f, 0f), Quaternion.identity);
            spike.SetActive(false);
            pipePool.Add(spike);
        }
    }

    void Update()
    {
        if (Time.time - lastSpawnTime > 1 / spawnRate)
        {
            SpawnSpike();
            lastSpawnTime = Time.time;
        }

        MoveSpikes();
        CheckCollision();
    }

    void SpawnSpike()
    {
        float randomY = Mathf.Clamp(Random.Range(minY, maxY), minY, maxY);
        GameObject spike = GetNextPipeFromPool();

        // Set the position of the spike and activate it
        spike.transform.position = new Vector3(10f, randomY, 0);
        spike.SetActive(true);
    }

    void MoveSpikes()
    {
        foreach (var spike in pipePool)
        {
            if (spike.activeSelf)
            {
                // Move the spike to the left
                spike.transform.position += new Vector3(-spikeSpeed * Time.deltaTime, 0, 0);
            }
        }
    }

    void CheckCollision()
    {
        foreach (var spike in pipePool)
        {
            if (spike.activeSelf)
            {
                // Separate bounding boxes for top and bottom spikes
                if (IsPlaneCollidingTop(spike) || IsPlaneCollidingBottom(spike))
                {
                    Debug.Log("Plane collided with a spike!");
                    dead.gameOver();
                }

                // Middle box for scoring
                if (IsPlaneCollidingMiddle(spike))
                {
                    Debug.Log("Plane scored!");
                    score.addScore(1);
                }
            }
        }
    }

    bool IsPlaneCollidingTop(GameObject pipe)
    {
        Vector3 planePosition = planeTransform.position;
        Vector3 pipePosition = pipe.transform.position;

        float planeRadius = 1f; //Plane Radius
        float spikeHeight = 2f; // Vertical rectangular border for spike.

        if (Mathf.Abs(planePosition.x - pipePosition.x) < (planeRadius + spikeWidth) * 0.5f &&
            planePosition.y + planeRadius > pipePosition.y + (spikeHeight * 0.5f))
        {
            return true;
        }

        return false;
    }

    bool IsPlaneCollidingBottom(GameObject pipe)
    {
        Vector3 planePosition = planeTransform.position;
        Vector3 spikePosition = pipe.transform.position;

        float planeRadius = 1f; // BirdRadius
        float spikeHeight = 2f; 

        if (Mathf.Abs(planePosition.x - spikePosition.x) < (planeRadius + spikeWidth) * 0.5f &&
            planePosition.y - planeRadius < spikePosition.y - (spikeHeight * 0.5f))
        {
            return true;
        }

        return false;
    }

    bool IsPlaneCollidingMiddle(GameObject pipe)
    {
        Vector3 birdPosition = planeTransform.position;
        Vector3 pipePosition = pipe.transform.position;

        float planeRadius = 1f; 
        float spikeHeight = 2f; 

        if (Mathf.Abs(birdPosition.x - pipePosition.x) < (planeRadius + spikeWidth) * 0.5f &&
            Mathf.Abs(birdPosition.y - pipePosition.y) < (spikeHeight * 0.5f))
        {
            return true;
        }

        return false;
    }

    GameObject GetNextPipeFromPool()
    {
        GameObject nextSpike = pipePool[currentPipeIndex];
        currentPipeIndex = (currentPipeIndex + 1) % poolSize;
        return nextSpike;
    }
}
