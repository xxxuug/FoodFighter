using UnityEngine;
using UnityEngine.SceneManagement;

// �̱����� ���׸����� ������ְ�, ���� ������ MonoBehaviour�� ���ش�.
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
                // @Manager��� �̸��� ���� ������Ʈ�� ���ٸ� 
                if (manager == null)
                {
                    // ������־��
                    manager = new GameObject("@Manager");
                    DontDestroyOnLoad(manager);
                }
                _instance = FindAnyObjectByType<T>();

                // �ν��Ͻ��� ã�Ƽ� �ν��Ͻ��� ���ٸ�
                if (_instance == null)
                {
                    // �ν��Ͻ��� ������־��
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
        // �� ��ȯ �� �ڵ����� OnSceneChanged�� ȣ�� ��
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    // �� ��ȯ ��
    protected void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        // �� ��ȯ �� Clear �Լ� ȣ��
        Clear();
    }

    protected virtual void Clear()
    {

    }

}
