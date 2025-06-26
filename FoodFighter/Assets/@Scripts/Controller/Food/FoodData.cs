using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T>
{
    private static T _instance;
    public static T Instance
    {
        get 
        {
            if (_instance == null)
                _instance = Resources.Load<T>(typeof(T).Name);
            return _instance; 
        }
    }
}

[CreateAssetMenu(fileName ="FoodData", menuName = "Game/Data/FoodDB")]
public class FoodData : ScriptableSingleton<FoodData>
{
    [System.Serializable]
    public class FoodInfo
    {
        public int Level;
        public string Name;
        public Sprite Icon;
        public int AttackPower;
    }

    public List<FoodInfo> FoodLists;

    private Dictionary<int, FoodInfo> _info;

    private void OnEnable()
    {
        _info = new();
        foreach (var f in FoodLists)
        {
            _info[f.Level] = f;
        }
    }

    public FoodInfo GetFood(int level)
    {
        if (_info == null || !_info.ContainsKey(level)) return null;

        return _info[level];
    }
}
