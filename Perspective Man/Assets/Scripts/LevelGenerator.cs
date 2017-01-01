using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public int numberOfBlocks;
    public float screenDistanceOffset;
    
    public Transform objPrefab;
    public Vector3 startingPosition;

    private Vector3 nextPosition;
    private Queue<Transform> objectQueue;

    // Use this for initialization
    void Start () {
        objectQueue = new Queue<Transform>(numberOfBlocks);
        for(int i = 0; i <= numberOfBlocks; i++)
        {
            objectQueue.Enqueue((Transform)Instantiate(objPrefab));
        }

        nextPosition = startingPosition;
        for(int i = 0; i <= numberOfBlocks; i++)
        {
            SpawnBlock();
        }
    }
    
    // Update is called once per frame
    void Update () {
        if (objectQueue.Peek().localPosition.x + screenDistanceOffset < PlayerController.distanceTraveled)
        {
            SpawnBlock();
        }

    }

    void SpawnBlock()
    {
        Transform o = objectQueue.Dequeue();
        o.localPosition = nextPosition;
        nextPosition.x += o.localScale.x;
        objectQueue.Enqueue(o);
    }
}
