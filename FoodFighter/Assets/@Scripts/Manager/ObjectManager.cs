using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    private PlayerController _player;
    public PlayerController Player { get => _player; }

    public HashSet<FoodBullet> Foods { get; set; } = new HashSet<FoodBullet>();
    // "게임 씬 위에 현재 활성화되어 존재하는 몬스터 인스턴스들"을 실시간으로 관리하는 컬렉션
    public HashSet<EnemyController> Enemies { get; set; } = new HashSet<EnemyController>();
    public HashSet<GoldController> Golds { get; set; } = new HashSet<GoldController>();
    public HashSet<BossStageController> Boss { get; set; } = new HashSet<BossStageController>();

    // 스폰할 수 있는 몬스터 프리팹 목록
    public List<GameObject> EnemyPrefabs = new List<GameObject>();
    public List<GameObject> GoldPrefabs = new List<GameObject>();
    public List<GameObject> HitEffectPrefabs = new List<GameObject>();
    public List<GameObject> BossPrefabs = new List<GameObject>();
    public List<GameObject> HitDamagePrefabs = new List<GameObject>();
    public List<GameObject> PopUpPrefabs = new List<GameObject>();

    private GameObject _playerResource;
    private GameObject _foodResource;
    private GameObject _goldResource;
    private GameObject _hitEffectResource;
    private GameObject _bossResource;
    private GameObject _hitDamageResource; // 피격 데미지
    private GameObject _popUpResource;

    protected override void Initialize()
    {
        base.Initialize();

        if (_player == null)
            _player = FindAnyObjectByType<PlayerController>();

        ResourceAllLoad();
    }

    public void ResourceAllLoad()
    {
        _playerResource = Resources.Load<GameObject>(Define.PlayerPath);
        _foodResource = Resources.Load<GameObject>(Define.BulletPath);
        _goldResource = Resources.Load<GameObject>(Define.GoldPath);
        _hitEffectResource = Resources.Load<GameObject>(Define.HitEffectPath);
        _bossResource = Resources.Load<GameObject>(Define.BossPath);
        _hitDamageResource = Resources.Load<GameObject>(Define.HitDamagePath);
        _popUpResource = Resources.Load<GameObject>(Define.PopUpPath);

        // enemy 폴더 프리팹 로드
        EnemyPrefabs.Clear();
        GameObject[] allPrefabs = Resources.LoadAll<GameObject>(Define.AllEnemyPath);

        foreach (var prefab in allPrefabs)
        {
            if (prefab.GetComponent<EnemyController>() != null)
            {
                EnemyPrefabs.Add(prefab);
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
            
            return playerController as T;
        }
        else if (type == typeof(FoodBullet))
        {
            GameObject obj = Instantiate(_foodResource, spawnPos, Quaternion.identity);
            FoodBullet foodBullet = obj.GetOrAddComponent<FoodBullet>();
            Foods.Add(foodBullet);
            return foodBullet as T;
        }
        else if (type == typeof(EnemyController))
        {
            foreach (var prefab in EnemyPrefabs)
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

        else if (type == typeof(BossStageController))
        {
            GameObject obj = Instantiate(_bossResource, spawnPos, Quaternion.identity);
            BossStageController boss = obj.GetOrAddComponent<BossStageController>();
            Boss.Add(boss);
            return boss as T;
        }
        else if (type == typeof(GoldController))
        {
            GameObject obj = Instantiate(_goldResource, spawnPos, Quaternion.identity);
            GoldController goldController = obj.GetOrAddComponent<GoldController>();
            Golds.Add(goldController);
            return goldController as T;
        }
        return null;
    }

    // 게임 오브젝트 스폰 함수
    public GameObject Spawn(GameObject prefab)
    {
        return null;
    }

    public void Despawn<T>(T obj) where T : BaseController
    {
        obj.gameObject.SetActive(false);
        /*
        EnemyController enemy = obj as EnemyController;

        if (enemy != null)
            StageManager.Instance.RemoveEnemy(enemy);
        */
    }

    // 게임오브젝트 despawn
    public void Despawn(GameObject obj)
    {
        obj?.SetActive(false);
        //Debug.Log("Despawn 실행");
    }

    protected override void Clear()
    {
        base.Clear();
        Foods.Clear();
        _player = null;
        Resources.UnloadUnusedAssets();
    }
}