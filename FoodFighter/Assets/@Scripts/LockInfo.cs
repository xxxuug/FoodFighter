using UnityEngine;
using System;

public enum LockType
{
    AttackLevel,
    Stage,
}

[Serializable]
public class LockInfo
{
    public LockType lockType;
    public int Level; // 요구 레벨
    //public GameObject UnlockObject; // 열려있을 때 보여줄 오브젝트
    public GameObject LockObject; // 잠겨있을 때 보여줄 오브젝트
}