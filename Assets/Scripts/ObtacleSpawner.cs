using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtacleSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _obtaclePrefabs = new();

    [SerializeField]
    private List<Transform> _spawnPositions = new();

    private bool SpawnAllow = true;

    private readonly float _spawnTime = 0.8f;

    private GameController _gameController;

    private void Awake()
    {
        _gameController = FindObjectOfType<GameController>();
    }

    private void Start()
    {
        _gameController.IsGameOver.AddListener(GameOver);
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (SpawnAllow)
        {
            int spawnIndex = Random.Range(0, _spawnPositions.Count);
            int obtacleIndex = Random.Range(0, _obtaclePrefabs.Count);

            Instantiate(_obtaclePrefabs[obtacleIndex], _spawnPositions[spawnIndex].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(_spawnTime);
        }
    }

    private void GameOver()
    {
        SpawnAllow = false;

        gameObject.SetActive(false);
    }
}
