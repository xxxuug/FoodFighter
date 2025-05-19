using UnityEngine;
using UnityEngine.SceneManagement;

// 싱글톤을 제네릭으로 만들어주고, 제약 조건을 MonoBehaviour로 해준다.
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance = null;

    public static bool IsInstance => _instance != null;
    public static T TryGetInstance() => IsInstance ? _instance : null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject manager = GameObject.Find("@Manager");
                // @Manager라는 이름을 가진 오브젝트가 없다면 
                if (manager == null)
                {
                    // 만들어주어라
                    manager = new GameObject("@Manager");
                    DontDestroyOnLoad(manager);
                }
                _instance = FindAnyObjectByType<T>();

                // 인스턴스를 찾아서 인스턴스가 없다면
                if (_instance == null)
                {
                    // 인스턴스를 만들어주어라
                    GameObject obj = new GameObject(typeof(T).Name);
                    T componnent = obj.AddComponent<T>();
                    obj.transform.parent = manager.transform;
                    _instance = componnent;
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        // 씬 전환 시 자동으로 OnSceneChanged가 호출 됨
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    // 씬 전환 시
    protected void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        // 씬 전환 시 Clear 함수 호출
        Clear();
    }

    protected virtual void Clear()
    {

    }

}
