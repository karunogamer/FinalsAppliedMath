using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMathSpawner : MonoBehaviour
{
    public GameObject pipePrefab;
    public ScoreScript score;
    public GameOverScript dead;
    public float spawnRate = 2f;
    public float minY = -2f;
    public float maxY = 2f;
    public int poolSize = 5;
    public float pipeSpeed = 2f;

    private List<GameObject> pipePool;
    private float lastSpawnTime = 0f;
    private int currentPipeIndex = 0;

    public Transform birdTransform; // Reference to the bird's transform
    public float pipeWidth = 1.0f; // Adjust this based on your pipe sprite's width

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
            GameObject pipe = Instantiate(pipePrefab, new Vector3(10f, 0f, 0f), Quaternion.identity);
            pipe.SetActive(false);
            pipePool.Add(pipe);
        }
    }

    void Update()
    {
        if (Time.time - lastSpawnTime > 1 / spawnRate)
        {
            SpawnPipe();
            lastSpawnTime = Time.time;
        }

        MovePipes();
        CheckCollision();
    }

    void SpawnPipe()
    {
        float randomY = Mathf.Clamp(Random.Range(minY, maxY), minY, maxY);
        GameObject pipe = GetNextPipeFromPool();

        // Set the position of the pipe and activate it
        pipe.transform.position = new Vector3(10f, randomY, 0);
        pipe.SetActive(true);
    }

    void MovePipes()
    {
        foreach (var pipe in pipePool)
        {
            if (pipe.activeSelf)
            {
                // Move the pipes to the left
                pipe.transform.position += new Vector3(-pipeSpeed * Time.deltaTime, 0, 0);
            }
        }
    }

    void CheckCollision()
    {
        foreach (var pipe in pipePool)
        {
            if (pipe.activeSelf)
            {
                // Separate bounding boxes for top and bottom pipes
                if (IsBirdCollidingTop(pipe) || IsBirdCollidingBottom(pipe))
                {
                    Debug.Log("Bird collided with a pipe!");
                    dead.gameOver();
                }

                // Middle box for scoring
                if (IsBirdCollidingMiddle(pipe))
                {
                    Debug.Log("Bird scored!");
                    score.addScore(1);
                }
            }
        }
    }

    bool IsBirdCollidingTop(GameObject pipe)
    {
        Vector3 birdPosition = birdTransform.position;
        Vector3 pipePosition = pipe.transform.position;

        float birdRadius = 0.5f; // Assuming a circular bird representation
        float pipeHeight = 3.0f; // Assuming a vertical rectangular pipe representation

        if (Mathf.Abs(birdPosition.x - pipePosition.x) < (birdRadius + pipeWidth) * 0.5f &&
            birdPosition.y + birdRadius > pipePosition.y + (pipeHeight * 0.5f))
        {
            return true;
        }

        return false;
    }

    bool IsBirdCollidingBottom(GameObject pipe)
    {
        Vector3 birdPosition = birdTransform.position;
        Vector3 pipePosition = pipe.transform.position;

        float birdRadius = 0.5f; // Assuming a circular bird representation
        float pipeHeight = 5.0f; // Assuming a vertical rectangular pipe representation

        if (Mathf.Abs(birdPosition.x - pipePosition.x) < (birdRadius + pipeWidth) * 0.5f &&
            birdPosition.y - birdRadius < pipePosition.y - (pipeHeight * 0.5f))
        {
            return true;
        }

        return false;
    }

    bool IsBirdCollidingMiddle(GameObject pipe)
    {
        Vector3 birdPosition = birdTransform.position;
        Vector3 pipePosition = pipe.transform.position;

        float birdRadius = 0.5f; // Assuming a circular bird representation
        float pipeHeight = 3.0f; // Assuming a vertical rectangular pipe representation

        if (Mathf.Abs(birdPosition.x - pipePosition.x) < (birdRadius + pipeWidth) * 0.5f &&
            Mathf.Abs(birdPosition.y - pipePosition.y) < (pipeHeight * 0.5f))
        {
            return true;
        }

        return false;
    }

    GameObject GetNextPipeFromPool()
    {
        GameObject nextPipe = pipePool[currentPipeIndex];
        currentPipeIndex = (currentPipeIndex + 1) % poolSize;
        return nextPipe;
    }
}
