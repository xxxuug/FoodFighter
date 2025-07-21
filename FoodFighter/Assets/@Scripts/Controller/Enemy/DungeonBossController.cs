using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;
using NUnit.Framework.Interfaces;
using UnityEngine.Experimental.GlobalIllumination;

public class DungeonBossController : MonoBehaviour
{
    private float _currentHP;
    public float _maxHP;
    private int Stage;

    public float CurrentHP => _currentHP;

    public Action OnDestroyed;

    private Image _hpImage;
    private TMP_Text _hpText;

    private DungeonStageInfo.Data currentData;

    private void Start()
    {
        Stage = (GameManager.Instance.SelectDungeon == "골드광산") ?
            GameManager.Instance.GoldStageDungeon : GameManager.Instance.DiaStageDungeon;

        Debug.Log("석상 Start() 호출됨");

        if (SceneManager.GetActiveScene().name.Contains("Dungeon"))
        {
            StageManager.Instance.Player.SetDungeonStage();
        }

        _hpImage = GameObject.Find("BossHpBar - Image").GetComponent<Image>();
        _hpText = GameObject.Find("BossHpBar Text - Text")?.GetComponent<TMP_Text>();

        if (_hpImage != null) _hpImage.fillAmount = 1f;
        if (_hpText != null) _hpText.text = "석상 등장!";
    }

    public void SetData(DungeonStageInfo.Data data)
    {
        currentData = data;

        // 현재 스테이지 다시 세팅 (GameManager 기준)
        Stage = (GameManager.Instance.SelectDungeon == "골드광산")
            ? GameManager.Instance.GoldStageDungeon
            : GameManager.Instance.DiaStageDungeon;

        // 단계에 따른 보정값 (1단계는 그대로, 이후는 +30%씩 증가)
        float hpMultiplier = 1f + (Stage - 1) * 0.3f;

        _maxHP = Mathf.RoundToInt(data.MaxHP * hpMultiplier);
        _currentHP = _maxHP;

        UpdateHPUI();
        Debug.Log($"석상 HP 세팅 완료: {_maxHP} (단계: {Stage}, 배수: {hpMultiplier})");
    }


    public void TakeDamage(float dmg)
    {
        _currentHP -= dmg;
        Debug.Log($"석상 데미지: -{dmg}, 현재 HP: {_currentHP}");

        UpdateHPUI();

        if (_currentHP <= 0)
        {
            _currentHP = 0;
            Debug.Log("석상 파괴됨!");
            OnDestroyed?.Invoke();

            Stage++;

            //  GameManager.Instance.StageDungeon = Stage;
            if (GameManager.Instance.SelectDungeon == "골드광산")
            {
                GameManager.Instance.GoldStageDungeon = Stage;
            }

            if (GameManager.Instance.SelectDungeon == "다이아광산")
            {
                GameManager.Instance.DiaStageDungeon = Stage;
            }

            //StageManager.Instance.Player.StartCoroutine(StageManager.Instance.Player.ResetDeath());
            //SceneManager.LoadScene(Define.GameScene);
        }
    }

    private void UpdateHPUI()
    {
        float displayHP = Mathf.Clamp(_currentHP, 0f, _maxHP);


        if (_hpImage != null)
            _hpImage.fillAmount = (_maxHP > 0f) ? displayHP / _maxHP : 0f;

        if (_hpText != null) 
            _hpText.text = $"{displayHP:F0} / {_maxHP:F0}";

        //if (_maxHP > 0)
        //{
        //    if (_hpImage != null) _hpImage.fillAmount = _currentHP / _maxHP;
        //    if (_hpText != null) _hpText.text = $"{_currentHP:F0} / {_maxHP:F0}";
        //}
    }

    public void SetCurrentHP(float hp)
    {
        _currentHP = Mathf.Clamp(hp, 0f, _maxHP);
    }
}
