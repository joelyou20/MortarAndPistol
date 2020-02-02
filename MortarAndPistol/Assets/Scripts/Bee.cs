using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bee : MonoBehaviour
{
    private int _hash;
    private Spawner _spawner;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            _hash = gameObject.GetHashCode();
            List<GameObject> activeSpawnerList = GetActiveSpawners();

            Debug.Log(activeSpawnerList
                .Select(x => x.GetComponent<Spawner>().GetSpawned()
                .Where(y => y.GetHashCode() == _hash)).ToList().Count);

            List<Spawner> prunedSpawnerList = activeSpawnerList
                .Select(x => (Spawner)x.GetComponent<Spawner>().GetSpawned()
                .Where(y => y.GetHashCode() == _hash)).ToList();


            if (prunedSpawnerList.Count > 1)
            {
                throw new Exception();
            }
            else
            {
                _spawner = prunedSpawnerList[0];
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Spawner hash not found. ERROR=" + e);
        }

        // List<int> spawnerHashList = spawnerList.Select(x => x.GetHashCode()).ToList();

    }

    // Update is called once per frame
    void Update()
    {
        if(_spawner == null)
        {
            Destroy(gameObject);
        }
    }

    private List<GameObject> GetActiveSpawners()
    {
        return GameObject.FindGameObjectsWithTag("Enemy")
            .Where(x => x.GetComponent<Spawner>() != null).ToList();
    }
}
