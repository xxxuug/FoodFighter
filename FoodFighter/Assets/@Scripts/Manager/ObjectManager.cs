using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    private PlayerController _player;
    public PlayerController Player { get => _player; }

    public HashSet<FoodBullet> Foods { get; set; } = new HashSet<FoodBullet>();
    public HashSet<EnemyController> Enemies { get; set; } = new HashSet<EnemyController>();

    private GameObject _playerResource;
    private GameObject _foodResource;
    private List<GameObject> _enemyResource = new List<GameObject>();

    protected override void Initialize()
    {
        base.Initialize();

        if (_player == null)
            _player = FindAnyObjectByType<PlayerController>();

        _playerResource = Resources.Load<GameObject>(Define.PlayerPath);

        ResourceAllLoad();
    }

    public void ResourceAllLoad()
    {
        _playerResource = Resources.Load<GameObject>(Define.PlayerPath);
        _foodResource = Resources.Load<GameObject>(Define.BulletPath);

        // enemy 폴더 프리팹 로드
        _enemyResource.Clear();
        GameObject[] allPrefabs = Resources.LoadAll<GameObject>(Define.AllEnemyPath);

        foreach (var prefab in allPrefabs)
        {
            if (prefab.GetComponent<EnemyController>() != null)
            {
                _enemyResource.Add(prefab);
            }
        }
    }

    // 어디서든 생성 가능하도록
    public T Spawn<T>(Vector3 spawnPos) where T : BaseController
    {
        Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            GameObject obj = Instantiate(_playerResource, spawnPos, Quaternion.identity);
            PlayerController playerController = obj.GetOrAddComponent<PlayerController>();
            _player = playerController;
            FindAnyObjectByType<Scrolling>()?.SetPlayer(playerController);

            return playerController as T;
        }
        else if (type == typeof(FoodBullet))
        {
            GameObject obj = Instantiate(_foodResource, spawnPos, Quaternion.identity);
            FoodBullet foodBullet = obj.GetOrAddComponent<FoodBullet>();
            Foods.Add(foodBullet);
            return foodBullet as T;
        }
        else if (typeof(EnemyController).IsAssignableFrom(type))
        {
            foreach (var prefab in _enemyResource)
            {
                if (prefab.GetComponent(type))
                {
                    GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
                    EnemyController enemy = obj.GetComponent<EnemyController>();
                    Enemies.Add(enemy);
                    return enemy as T;
                }
            }
        }
        return null;
    }

    public void Despawn<T>(T obj) where T : BaseController
    {
        obj.gameObject.SetActive(false);
    }

    protected override void Clear()
    {
        base.Clear();
        Foods.Clear();
        _player = null;
        Resources.UnloadUnusedAssets();
    }
}