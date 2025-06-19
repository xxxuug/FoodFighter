using UnityEngine;

public class MissionData // �̼� ����Ÿ
{
    public int ID;              // �̼� ID
    public string Title;        // �̼� Ÿ��Ʋ
    public string Description;  // �̼� ����
    public MissionType Type;    // �̼� ����
    public int TargetValue;     // ��ǥ �̼� ����(����)
    public string TargetParam;  // �߰� ����(������ �̸�, ������)
    public RewardType reward;   // ����
}

public enum MissionType // �̼� ����
{
    Merge, // ����
    Resource, // �ڿ�(��� ����, ��� ������ ��)
    Hunting, // ���
    Function, // ���(
}

public class MissionState // �̼� �����Ȳ
{
    public int MissionId;
    public int CurrentValue; // ���� ���� ī��Ʈ
    public bool IsCompleted; // �ϼ� �ߴ��� ����
}

public class RewardType // ���� ����
{

}

public class MissionManager : MonoBehaviour
{

}
