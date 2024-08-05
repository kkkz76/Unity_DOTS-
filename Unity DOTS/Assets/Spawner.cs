using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private int spawnAmount;
    [SerializeField] private float rotateSpeed = 1.0f; // Added this variable to control rotation speed

    private List<GameObject> spawnedCubes = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Spawn the cubes initially
        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject newCube = Instantiate(cube, transform.position, transform.rotation);
            spawnedCubes.Add(newCube);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate all spawned cubes
        foreach (GameObject spawnedCube in spawnedCubes)
        {
            spawnedCube.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }
}
