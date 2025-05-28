using UnityEngine;
using System;

public enum LockType
{
    PlayerLevel,
    AttackLevel
}

[Serializable]
public class LockInfo
{
    public LockType lockType;
    public int Level; // �䱸 ����
    public GameObject UnlockObject; // �������� �� ������ ������Ʈ
    public GameObject LockObject; // ������� �� ������ ������Ʈ
}
