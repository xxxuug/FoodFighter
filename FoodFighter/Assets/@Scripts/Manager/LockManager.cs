using System;
using UnityEngine;
using UnityEngine.UI;

public enum LockType
{
    AttackLevel, // 공격력 레벨
    Stage, // 스테이지 레벨 
}

[Serializable]
public class LockInfo
{
    public LockType lockType; // 어떤 조건으로 잠기는지 (공격력 or 스테이지)
    public int Level; // 요구 레벨
    public GameObject LockObject; // 잠겨있을 때 보여줄 오브젝트
}

public class LockManager : MonoBehaviour
{
    private int AttackLevel = 0; // 공격력 레벨
    public int StageLevel = 0; // 스테이지 레벨

    public LockInfo[] lockInfo;

    public void RefreshUnlock()
    {
        foreach (var Locks in lockInfo)
        {
            bool UnLock = false; // 해제 여부

            switch (Locks.lockType)
            {
                case LockType.AttackLevel: // 공격력
                    UnLock = AttackLevel >= Locks.Level; // 현재 공격력 >= 요구 공격력이라면 잠금 해제
                    break;
                case LockType.Stage: // 스테이지
                    UnLock = StageLevel >= Locks.Level; // 현재 스테이지 >= 요구 공격력이라면 잠금 해제
                    break;
            }

            // Locks.UnlockObject.SetActive(UnLock);
            Locks.LockObject.SetActive(!UnLock); // 잠금 상태일 때만 true
        }
    }

    public void SetStage(int Level)
    {
        StageLevel = Level;

        RefreshUnlock();
    }

    public void SetAttackLevel(int Level)
    {
        AttackLevel = Level;
        RefreshUnlock();
    }
}
