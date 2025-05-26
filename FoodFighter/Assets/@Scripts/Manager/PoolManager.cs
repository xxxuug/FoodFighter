using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    // ������Ʈ Ǯ ������
    Dictionary<System.Type, List<GameObject>> _pooledObject = new Dictionary<System.Type, List<GameObject>>();

    // ������ ������Ʈ
    Dictionary<System.Type, GameObject> _parentObject = new Dictionary<System.Type, GameObject>();

    public T GetObject<T>(Vector3 pos) where T : BaseController
    {
        System.Type type = typeof(T);

        if (type.Equals(typeof(FoodBullet)) || type.Equals(typeof(EnemyController)))
        {
            if (_pooledObject.ContainsKey(type))
            {
                // ��Ȱ��ȭ �� ������Ʈ ����
                for (int i = 0; i < _pooledObject[type].Count; i++)
                {
                    if (!_pooledObject[type][i].activeSelf)
                    {
                        _pooledObject[type][i].SetActive(true);
                        _pooledObject[type][i].transform.position = pos;

                        return _pooledObject[type][i].GetComponent<T>();
                    }
                }
                // ��Ȱ��ȭ �� ������Ʈ�� ���ٸ� ���� ����
                var obj = ObjectManager.Instance.Spawn<T>(pos);
                obj.transform.parent = _parentObject[type].transform;
                _pooledObject[type].Add(obj.gameObject);
                return obj;
            }

            // Ǯ�� �������� �ʴٸ� �ʱ�ȭ
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

    protected override void Clear()
    {
        base.Clear();
        _pooledObject.Clear();
        _parentObject.Clear();
    }
}
