using UnityEngine;

public class MissionData // 미션 데이타
{
    public int ID;              // 미션 ID
    public string Title;        // 미션 타이틀
    public string Description;  // 미션 내용
    public MissionType Type;    // 미션 종류
    public int TargetValue;     // 목표 미션 조건(숫자)
    public string TargetParam;  // 추가 정보(아이템 이름, 아이콘)
    public RewardType reward;   // 보상
}

public enum MissionType // 미션 종류
{
    Merge, // 머지
    Resource, // 자원(골드 수급, 재료 모으기 등)
    Hunting, // 사냥
    Function, // 기능(
}

public class MissionState // 미션 진행상황
{
    public int MissionId;
    public int CurrentValue; // 현재 조건 카운트
    public bool IsCompleted; // 완성 했는지 여부
}

public class RewardType // 보상 종류
{

}

public class MissionManager : MonoBehaviour
{

}
