using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    private PlayerController _player;
    public PlayerController Player { get => _player; }

    public HashSet<FoodBullet> Food { get; set; } = new HashSet<FoodBullet>();

    private GameObject _playerResource;
    private GameObject _cactusResource;

    protected override void Initialize()
    {
        base.Initialize();

        if (_player == null)
            _player = FindAnyObjectByType<PlayerController>();

       _playerResource = Resources.Load<GameObject>(Define.Player);
    }

    public void ResourceAllLoad()
    {
       _playerResource = Resources.Load<GameObject>(Define.Player);
        _cactusResource = Resources.Load<GameObject>(Define.Bullet);
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
            return playerController as T;
        }
        else if (type == typeof(FoodBullet))
        {
            GameObject obj = Instantiate(_cactusResource, spawnPos, Quaternion.identity);
            FoodBullet cactusController = obj.GetOrAddComponent<FoodBullet>();
            Food.Add(cactusController);
            return cactusController as T;
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
        Food.Clear();
       // Mushrooms.Clear();
        _player = null;
        Resources.UnloadUnusedAssets();
    }
}