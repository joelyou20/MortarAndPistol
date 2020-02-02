using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> enemyTypes;
    public Transform spawnPoint;
    public float timeBetweenSpawns = 5f;
    public int maxEnemies = 3;

    private float _timer;
    private List<EnemyAI> _spawnedEnemies;
    private int _id;

    // Start is called before the first frame update
    void Start()
    {
        _id = gameObject.GetHashCode();
        _spawnedEnemies = new List<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= timeBetweenSpawns)
        {
            if(_spawnedEnemies.Count < maxEnemies)
            {
                _spawnedEnemies.Add(Instantiate(enemyTypes[0], spawnPoint.position, spawnPoint.rotation)
                    .GetComponent<EnemyAI>());
                _timer = 0f;
            }
        }
        pollSpawnedEnemies();
    }

    public List<EnemyAI> GetSpawned() { return _spawnedEnemies; }

    private void pollSpawnedEnemies()
    {
        for(int i = 0; i < _spawnedEnemies.Count; i++)
        {
            if(_spawnedEnemies[i] == null)
            {
                _spawnedEnemies.RemoveAt(i);
            }
        }
    }
}
