using TMPro;
using UnityEngine;

public class EnhanceUI : MonoBehaviour
{
    public PlayerSO playerSO; // PlayerSO 참조
    public TextMeshProUGUI CurrentAttackPower; // 현재 공격력 텍스트
    public TextMeshProUGUI EnhancementProbability; // 강화 확률 텍스트
    public TextMeshProUGUI EnhanceCostText; // 강화 비용 텍스트

    private float enhancementProbability = 0.99f; // 초기 강화 확률
    private float probabilityDecreaseRate = 0.05f; // 강화 성공 시 감소할 확률
    private int attackPowerDecrease = 2; // 강화 실패 시 감소할 공격력
    private int minAttackPower = 1; // 공격력 최소값
    private int baseEnhanceCost = 100; // 초기 강화 비용
    private int enhanceCount = 0; // 강화 횟수

    void Start()
    {
        if (playerSO != null)
        {
            playerSO.OnDamageChanged += UpdateUI; // 이벤트 구독
            Debug.Log("강화 시스템 시작! 초기 공격력: " + playerSO.Damage);
            DisplayStatus();
        }
        else
        {
            Debug.LogError("PlayerSO가 설정되지 않았습니다.");
        }

        UpdateUI(); // 초기 UI 업데이트
    }

    void OnDestroy()
    {
        if (playerSO != null)
        {
            playerSO.OnDamageChanged -= UpdateUI; // 이벤트 구독 해제
        }
    }

    // 강화 시도 메서드
    public void TryEnhance()
    {
        if (playerSO == null)
        {
            Debug.LogError("PlayerSO가 설정되지 않았습니다.");
            return;
        }

        float roll = Random.Range(0f, 1f); // 0과 1 사이의 난수 생성

        if (roll < enhancementProbability)
        {
            // 강화 성공
            int previousDamage = playerSO.Damage;
            playerSO.Damage = CalculateNextDamage(previousDamage);
            enhancementProbability -= probabilityDecreaseRate;

            if (enhancementProbability < 0.5f)
            {
                enhancementProbability = 0.5f; // 최소 강화 확률 설정 (예시로 0.5로 설정)
            }

            Debug.Log("강화 성공! 새로운 공격력: " + playerSO.Damage + ", 새로운 성공 확률: " + enhancementProbability);
            enhanceCount++;
        }
        else
        {
            // 강화 실패
            playerSO.Damage -= attackPowerDecrease;

            if (playerSO.Damage < minAttackPower)
            {
                playerSO.Damage = minAttackPower; // 최소 공격력 제한
            }

            Debug.Log("강화 실패... 새로운 공격력: " + playerSO.Damage);
            enhanceCount--;
        }

        DisplayStatus();
    }

    // 다음 공격력 계산 메서드
    private int CalculateNextDamage(int previousDamage)
    {
        // 강화할 때마다 일정 비율로 공격력 증가
        float increasePercentage = 0.1f; // 10% 증가 비율

        int increaseAmount = Mathf.CeilToInt(previousDamage * increasePercentage); // 이전 공격력의 10% 계산

        // 최소 증가량 설정
        if (increaseAmount < 1)
        {
            increaseAmount = 1;
        }

        return previousDamage + increaseAmount;
    }

    // UI 상태 업데이트 메서드
    void DisplayStatus()
    {
        if (playerSO != null)
        {
            UpdateUI(); // UI 업데이트
        }
    }

    // UI를 업데이트하는 메서드
    void UpdateUI()
    {
        if (CurrentAttackPower != null)
        {
            CurrentAttackPower.text = "CurrentATK: " + playerSO.Damage;
        }

        if (EnhancementProbability != null)
        {
            EnhancementProbability.text = "Probability: " + (enhancementProbability * 100).ToString("F2") + "%";
        }

        if (EnhanceCostText != null)
        {
            int enhanceCost = CalculateEnhanceCost();
            EnhanceCostText.text = "Enhance Cost: " + enhanceCost;
        }
    }

    // 강화 횟수에 따른 강화 비용 계산
    public int CalculateEnhanceCost()
    {
        int enhanceCost = baseEnhanceCost + (enhanceCount * 50); // 강화 횟수에 비례하여 비용 증가

        return enhanceCost;
    }
}
