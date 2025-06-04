using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    // 오브젝트 풀 관리용
    Dictionary<System.Type, List<GameObject>> _pooledObject = new Dictionary<System.Type, List<GameObject>>();
    // 정리용 오브젝트
    Dictionary<System.Type, GameObject> _parentObject = new Dictionary<System.Type, GameObject>();

    // 몬스터 프리팹 풀 관리용
    Dictionary<GameObject, List<GameObject>> _pooledPrefabs = new Dictionary<GameObject, List<GameObject>>();
    // 정리용
    Dictionary<GameObject, GameObject> _parentPrefabs = new Dictionary<GameObject, GameObject>();

    public T GetObject<T>(Vector3 pos) where T : BaseController
    {
        System.Type type = typeof(T);

        if (type.Equals(typeof(FoodBullet)) || type.Equals(typeof(EnemyController)) || type.Equals(typeof(GoldController)))
        {
            if (_pooledObject.ContainsKey(type))
            {
                // 비활성화 된 오브젝트 재사용
                for (int i = 0; i < _pooledObject[type].Count; i++)
                {
                    if (!_pooledObject[type][i].activeSelf)
                    {
                        _pooledObject[type][i].SetActive(true);
                        _pooledObject[type][i].transform.position = pos;

                        return _pooledObject[type][i].GetComponent<T>();
                    }
                }
                // 비활성화 된 오브젝트가 없다면 새로 생성
                var obj = ObjectManager.Instance.Spawn<T>(pos);
                obj.transform.parent = _parentObject[type].transform;
                _pooledObject[type].Add(obj.gameObject);
                return obj;
            }

            // 풀이 존재하지 않다면 초기화
            else
            {
                if (!_parentObject.ContainsKey(type))
                {
                    GameObject go = new GameObject(type.Name);
                    if (type == typeof(FoodBullet))
                    {
                        Transform player = GameObject.FindWithTag("Player")?.transform;
                        if (player != null)
                            go.transform.parent = player;
                    }
                    _parentObject.Add(type, go);
                }
                var obj = ObjectManager.Instance.Spawn<T>(pos);
                obj.transform.parent = _parentObject[type].transform;
                List<GameObject> newList = new List<GameObject>();
                newList.Add(obj.gameObject);
                _pooledObject.Add(type, newList);
                return obj;
            }
        }
        return null;
    }

    // 몬스터 프리팹용 함수
    public BaseController GetObject(GameObject prefab, Vector3 pos)
    {
        if (_pooledPrefabs.ContainsKey(prefab))
        {
            foreach (var obj in _pooledPrefabs[prefab])
            {
                if (!obj.activeSelf)
                {
                    obj.transform.position = pos;
                    obj.SetActive(true);
                    return obj.GetComponent<BaseController>();
                }
            }
        }
        else
        {
            _pooledPrefabs[prefab] = new List<GameObject>();

            if (!_parentPrefabs.ContainsKey(prefab))
            {
                GameObject go = new GameObject(prefab.name);
                _parentPrefabs[prefab] = go;
            }
        }

        GameObject newObj = GameObject.Instantiate(prefab, pos, Quaternion.identity);
        newObj.transform.parent = _parentPrefabs[prefab].transform;
        _pooledPrefabs[prefab].Add(newObj);

        return newObj.GetComponent<BaseController>();
    }

    protected override void Clear()
    {
        base.Clear();
        _pooledObject.Clear();
        _parentObject.Clear();
    }
}
