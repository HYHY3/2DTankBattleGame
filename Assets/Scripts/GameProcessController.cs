using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProcessController : MonoBehaviour
{
    [SerializeField]
    GameObject[] _enemyPrefab = null;

    private const int _GameMode1P = 1;
    private readonly string[] _EnemyTankType = { "EnemyTankA", "EnemyTankB", "EnemyTankC" };
    private const int _MaxTankCount = 15;
    private const int _ThresholdOfTankCreated = 5;
    private const float _TimeIntervalOfTankCreated = 1.2f;
    private const string _GameSceneName = "Level";
    private const string _GameMenuScene = "Level0";

    private static int _nextLevel = 1;
    private static int _totalGameScene = 1;

    private GameObject[] _enemiesGroup;
    private GameObject _enemyInstance;
    private int _totalTankCount;
    private EnemyController[] _enemyTanks;
    private float _timeCDOfTankCreated;
    private int _currentTankCount;
    private int _indexOfTankPosition;
    private PlayerAController _playerATank;
    private PlayerBController _playerBTank;

    void Awake()
    {
        _enemiesGroup = GameObject.FindGameObjectsWithTag("EnemiesGroupTag");
        if (_enemiesGroup == null)
        {
            Debug.Log("GameProcessController.Awake() EnemyPosition is null.");
            return;
        }
        //Debug.Log("GameProcessController.Awake() Enemy Count = " + _enemiesGroup.Length);

        if (GameMenuController._gameMode == _GameMode1P)
        {
            GameObject.FindObjectOfType<PlayerBController>().gameObject.SetActive(false);
        }

        //Debug.Log("GameProcessController.Awake() Game Scene Count = " + SceneManager.sceneCountInBuildSettings);
        _totalGameScene = SceneManager.sceneCountInBuildSettings - 1;
        _indexOfTankPosition = 0;
        _totalTankCount = 0;
        _currentTankCount = 0;
        _timeCDOfTankCreated = _TimeIntervalOfTankCreated;
    }

    void Update()
    {
        judgeAnyKeyPressed();
        judgeGameProcess();
    }

    private void judgeAnyKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Dialog Box (Save or Don't Save or Cancel)
            SceneManager.LoadScene(_GameMenuScene);
            Debug.Log("GameProcessController.judgeAnyKeyPressed  Esc key was pressed.");
            return;
        }
    }

    private void judgeGameProcess()
    {
        bool isGameOver = false;

        _playerATank = FindObjectOfType<PlayerAController>();
        if (_playerATank == null)
        {
            isGameOver = true;
        }
        if (isGameOver && GameMenuController._gameMode != _GameMode1P)
        {
            _playerBTank = FindObjectOfType<PlayerBController>();
            if (_playerBTank == null)
            {
                isGameOver = true;
            }
            else
            {
                isGameOver = false;
            }
        }

        if (isGameOver)
        {
            // Dialog Box (Again or Return to Game Menu)
            SceneManager.LoadScene(_GameMenuScene);
            Debug.Log("GameProcessController.judgeAnyKeyPressed  Game Over.");
            return;
        }

        _enemyTanks = FindObjectsOfType<EnemyController>();
        //Debug.Log("GameProcessController.judgeCreateEnemyTank Tank Count = " + _enemyTanks.Length);
        if (_totalTankCount > _MaxTankCount && _enemyTanks.Length == 0)
        {
            _nextLevel %= _totalGameScene; 
            ++_nextLevel;
            SceneManager.LoadScene(_GameSceneName + _nextLevel.ToString());
            return;
        }

        judgeCreateEnemyTank();
    }

    private void judgeCreateEnemyTank()
    {
        _timeCDOfTankCreated -= Time.deltaTime;
        if (_timeCDOfTankCreated > 0.001f)
        {
            return;
        }
        _timeCDOfTankCreated = _TimeIntervalOfTankCreated;

        _currentTankCount = _enemyTanks.Length;
        while (true)
        {
            if (_totalTankCount > _MaxTankCount || _currentTankCount > _ThresholdOfTankCreated)
            {
                return;
            }
            _indexOfTankPosition = (_indexOfTankPosition + 1) % _enemiesGroup.Length;
            GameObject enemy = _enemiesGroup[_indexOfTankPosition];
            int indexOfTankType = Random.Range(1, 100) % _EnemyTankType.Length;
            _enemyInstance = Instantiate(_enemyPrefab[indexOfTankType], enemy.transform.position, enemy.transform.rotation);
            _enemyInstance.name = _EnemyTankType[indexOfTankType];
            _enemyInstance.transform.parent = enemy.transform;
            ++_totalTankCount;
            ++_currentTankCount;
        }
    }

}
