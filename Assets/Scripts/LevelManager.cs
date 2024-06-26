using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public GameObject rangedEnemy;
    public GameObject meleeEnemy;
    public TMP_Text levelText;
    public TMP_Text startingLevelText;
    public TMP_Text enemyCounter;
    [SerializeField] private int level;
    private Camera _mainCamera;
    private int _numEnemies;
    private int _numAliveEnemies;
    private bool _enemyCheck;
    private float _rdm;
    
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        startingLevelText.enabled = false;
        level = 1;
        _enemyCheck = true;
        StartLevel(level);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            StartLevel(level+1);
        }
        if (_enemyCheck)
        {
            StartCoroutine(nameof(EnemyCheck));
            enemyCounter.text = "Enemies Remaining: " + _numAliveEnemies;
        }
    }

    private void StartLevel(int _level)
    {
        level = _level;
        levelText.text = "Level: " + _level;
        _numEnemies = (int) (.4 * level) + level;
        StartCoroutine(SpawnEnemies(_numEnemies));
    }

    private IEnumerator EnemyCheck()
    {
        _enemyCheck = false;
        _numAliveEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (_numAliveEnemies == 0)
        {
            startingLevelText.enabled = true;
            startingLevelText.text = "Starting Level " + (level+1) + "...";
            yield return new WaitForSeconds(2f);
            startingLevelText.enabled = false;
            StartLevel(level+1);
        }
        yield return new WaitForSeconds(.5f);
        _enemyCheck = true;
    }

    private IEnumerator SpawnEnemies(int numEnemies)
    {
        int direction = 0;
        for (int i = 0; i < numEnemies;)
        {
            Vector2 spawnPoint = GenSpawnPoint(direction);
            _rdm = Random.Range(0f, 1f);
            if (WithinBounds(spawnPoint))
            {
                if (_rdm <= .4)
                {
                    Instantiate(rangedEnemy, spawnPoint, Quaternion.identity);
                    i++;
                }
                else
                {
                    Instantiate(meleeEnemy, spawnPoint, Quaternion.identity);
                    i++;
                }
            }
            direction++;
            yield return new WaitForSeconds(.5f);
        }
        enemyCounter.text = "Enemies Remaining: " + _numAliveEnemies;
        yield return null;
    }

    private bool WithinBounds(Vector2 spawnPoint)
    {
        float dx = Mathf.Abs(spawnPoint.x);
        float dy = Mathf.Abs(spawnPoint.y);

        if ((dx / 30) + (dy / 15) < 1)
        {
            return true;
        }
        return false;
    }

    private Vector2 GenSpawnPoint(int num)
    {
        int spawnDirection = num % 4;
        Vector3 point;
        switch (spawnDirection)
        {
            //Above
            case 0:
                _rdm = Random.Range(0f, 1f);
                point = _mainCamera.ViewportToWorldPoint(new Vector3(_rdm, 1, 0));
                point.y += Random.Range(2f, 4f);
                break;
            //Right
            case 1:
                _rdm = Random.Range(0f, 1f);
                point = _mainCamera.ViewportToWorldPoint(new Vector3(1, _rdm, 0));
                point.x += Random.Range(2f, 4f);
                break;
            //Below
            case 2:
                _rdm = Random.Range(0f, 1f);
                point = _mainCamera.ViewportToWorldPoint(new Vector3(_rdm, 0, 0));
                point.y -= Random.Range(2f, 4f);
                break;
            //Left
            case 3:
                _rdm = Random.Range(0f, 1f);
                point = _mainCamera.ViewportToWorldPoint(new Vector3(0, _rdm, 0));
                point.x -= Random.Range(2f, 4f);
                break;
            default:
                print("Error GenSpawnPoint(), spawnDirection = " + spawnDirection);
                return new Vector2(0, 0);
        }
        return point;
    }
}
