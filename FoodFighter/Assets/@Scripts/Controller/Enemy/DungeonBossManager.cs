using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonBossManager : MonoBehaviour
{
    [SerializeField] private DungeonStageInfo dungeonInfo;
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private string selectedDungeonName;

    private float currentTime;
    private bool isActive = false;

    private DungeonStageInfo.Data currentData;
    private DungeonBossController statue;

    private int RewardDiamond;
    private int RewardGold;

    public GameObject SuccessPopup;
    public GameObject FailPopup;

    public TMP_Text RewardText;
    public GameObject GoldImage;
    public GameObject DiaImage;

    private void Start()
    {
        selectedDungeonName = GameManager.Instance.SelectDungeon;

        InitStage();

        //RewardGold = GameManager.Instance.GoldDungeonMoney;
        //RewardDiamond = GameManager.Instance.DiaDungeonMoney;

        if (selectedDungeonName == "골드광산")
        {
            RewardGold = GameManager.Instance.GoldDungeonMoney;
            RewardDiamond = 0; // 다이아는 주면 안 됨


            RewardText.text = $"{Utils.FormatKoreanNumber(RewardGold)}";
            DiaImage.SetActive(false);
        }
        else if (selectedDungeonName == "다이아광산")
        {
            RewardGold = 0; // 골드는 주면 안 됨
            RewardDiamond = GameManager.Instance.DiaDungeonMoney;

            RewardText.text = $"{Utils.FormatKoreanNumber(RewardDiamond)}";
            GoldImage.SetActive(false);
        }

        SuccessPopup.SetActive(false);
        FailPopup.SetActive(false);
    }

    private void Update()
    {
        if (!isActive) return;

        currentTime -= Time.deltaTime;
        timerText.text = $" {currentTime:F1}";

        if (currentTime <= 0f)
        {
            FailStage();
        }
    }

    private void InitStage()
    {
        currentData = dungeonInfo.list.Find(data => data.DungeonName == selectedDungeonName);

        if (currentData == null)
        {
            Debug.LogError($"던전 정보 없음: {selectedDungeonName}");
            return;
        }

        isActive = true;
        currentTime = currentData.TimeLimit;

        // 석상 생성
        //GameObject bossObj = Instantiate(currentData.BossPrefab, bossSpawnPoint.position, Quaternion.identity);
        //statue = bossObj.GetComponent<DungeonBossController>();
        Vector3 fixedPosition = new Vector3(1f, 1.83f, 0f);
        GameObject bossObj = Instantiate(currentData.BossPrefab, fixedPosition, Quaternion.identity);
        statue = bossObj.GetComponent<DungeonBossController>();

        // statue.MaxHP = currentData.MaxHP;
        statue.SetData(currentData);

        if (PlayerPrefs.HasKey("DungeonBossHP"))
        {
            float savedHp = PlayerPrefs.GetFloat("DungeonBossHP");
            statue.SetCurrentHP(savedHp);
        }

        statue.OnDestroyed += ClearStage;

        Debug.Log($"'{selectedDungeonName}' 던전 시작! HP: {currentData.MaxHP}, 시간제한: {currentData.TimeLimit}");
    }

    private void ClearStage()
    {
        isActive = false;
        PlayerPrefs.DeleteKey("DungeonBossHP");

        //  OnDefeted();

        if (SuccessPopup != null)
            SuccessPopup.SetActive(true);

        Time.timeScale = 0f;

        //StageManager.Instance.Player.StartCoroutine(StageManager.Instance.Player.ResetDeath());
        //SceneManager.LoadScene(Define.GameScene);
    }

    private void FailStage()
    {
        isActive = false;
        PlayerPrefs.DeleteKey("DungeonBossHP");

        if (FailPopup != null)
            FailPopup.SetActive(true);

        Time.timeScale = 0f;

        //StageManager.Instance.Player.StartCoroutine(StageManager.Instance.Player.ResetDeath());
        //SceneManager.LoadScene(Define.GameScene);
        Debug.Log("시간 초과! 실패");
    }

    public void SetSelectedDungeon(string dungeonName)
    {
        selectedDungeonName = dungeonName;
    }

    void OnDefeted()
    {
        GameManager.Instance.AddGold(RewardGold);
        GameManager.Instance.AddDiamond(RewardDiamond);

        if (selectedDungeonName == "골드광산")
        {
            RewardGold *= 5;
            GameManager.Instance.GoldDungeonMoney = RewardGold;
        }
        else if (selectedDungeonName == "다이아광산")
        {
            RewardDiamond *= 2;
            GameManager.Instance.DiaDungeonMoney = RewardDiamond;

        }
    }
    
    public void OnClickSuccess()
    {
        Time.timeScale = 1f;

        OnDefeted();

        StageManager.Instance.Player.StartCoroutine(StageManager.Instance.Player.ResetDeath());
        SceneManager.LoadScene(Define.GameScene);
    }

    public void OnClickFail()
    {
        Time.timeScale = 1f;

        StageManager.Instance.Player.StartCoroutine(StageManager.Instance.Player.ResetDeath());
        SceneManager.LoadScene(Define.GameScene);
    }


}