using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    private PlayerController _player;
    public PlayerController Player { get => _player; }

    public HashSet<FoodBullet> Food { get; set; } = new HashSet<FoodBullet>();

    private GameObject _playerResource;
    private GameObject _foodResource;
   // private GameObject _effectResource;

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
      //  _effectResource = Resources.Load<GameObject>(Define.Effect);
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
            GameObject obj = Instantiate(_foodResource, spawnPos, Quaternion.identity);
            FoodBullet cactusController = obj.GetOrAddComponent<FoodBullet>();
            Food.Add(cactusController);
            return cactusController as T;
        }
        //else if (type == typeof(FoodBullet))
        //{
        //    GameObject obj = Instantiate(_effectResource, spawnPos, Quaternion.identity);
        //    FoodBullet effectController = obj.GetOrAddComponent<FoodBullet>();
        //    Food.Add(effectController);
        //    return effectController as T;
        //}
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
        _player = null;
        Resources.UnloadUnusedAssets();
    }
}