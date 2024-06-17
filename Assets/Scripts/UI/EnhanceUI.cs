using TMPro;
using UnityEngine;

public class EnhanceUI : MonoBehaviour
{
    public PlayerSO playerSO; // PlayerSO ����
    public TextMeshProUGUI CurrentAttackPower; // ���� ���ݷ� �ؽ�Ʈ
    public TextMeshProUGUI EnhancementProbability; // ��ȭ Ȯ�� �ؽ�Ʈ
    public TextMeshProUGUI EnhanceCostText; // ��ȭ ��� �ؽ�Ʈ

    private float enhancementProbability = 0.99f; // �ʱ� ��ȭ Ȯ��
    private float probabilityDecreaseRate = 0.05f; // ��ȭ ���� �� ������ Ȯ��
    private int attackPowerDecrease = 2; // ��ȭ ���� �� ������ ���ݷ�
    private int minAttackPower = 1; // ���ݷ� �ּҰ�
    private int baseEnhanceCost = 100; // �ʱ� ��ȭ ���
    private int enhanceCount = 0; // ��ȭ Ƚ��

    void Start()
    {
        if (playerSO != null)
        {
            playerSO.OnDamageChanged += UpdateUI; // �̺�Ʈ ����
            Debug.Log("��ȭ �ý��� ����! �ʱ� ���ݷ�: " + playerSO.Damage);
            DisplayStatus();
        }
        else
        {
            Debug.LogError("PlayerSO�� �������� �ʾҽ��ϴ�.");
        }

        UpdateUI(); // �ʱ� UI ������Ʈ
    }

    void OnDestroy()
    {
        if (playerSO != null)
        {
            playerSO.OnDamageChanged -= UpdateUI; // �̺�Ʈ ���� ����
        }
    }

    // ��ȭ �õ� �޼���
    public void TryEnhance()
    {
        if (playerSO == null)
        {
            Debug.LogError("PlayerSO�� �������� �ʾҽ��ϴ�.");
            return;
        }

        float roll = Random.Range(0f, 1f); // 0�� 1 ������ ���� ����

        if (roll < enhancementProbability)
        {
            // ��ȭ ����
            int previousDamage = playerSO.Damage;
            playerSO.Damage = CalculateNextDamage(previousDamage);
            enhancementProbability -= probabilityDecreaseRate;

            if (enhancementProbability < 0.5f)
            {
                enhancementProbability = 0.5f; // �ּ� ��ȭ Ȯ�� ���� (���÷� 0.5�� ����)
            }

            Debug.Log("��ȭ ����! ���ο� ���ݷ�: " + playerSO.Damage + ", ���ο� ���� Ȯ��: " + enhancementProbability);
            enhanceCount++;
        }
        else
        {
            // ��ȭ ����
            playerSO.Damage -= attackPowerDecrease;

            if (playerSO.Damage < minAttackPower)
            {
                playerSO.Damage = minAttackPower; // �ּ� ���ݷ� ����
            }

            Debug.Log("��ȭ ����... ���ο� ���ݷ�: " + playerSO.Damage);
            enhanceCount--;
        }

        DisplayStatus();
    }

    // ���� ���ݷ� ��� �޼���
    private int CalculateNextDamage(int previousDamage)
    {
        // ��ȭ�� ������ ���� ������ ���ݷ� ����
        float increasePercentage = 0.1f; // 10% ���� ����

        int increaseAmount = Mathf.CeilToInt(previousDamage * increasePercentage); // ���� ���ݷ��� 10% ���

        // �ּ� ������ ����
        if (increaseAmount < 1)
        {
            increaseAmount = 1;
        }

        return previousDamage + increaseAmount;
    }

    // UI ���� ������Ʈ �޼���
    void DisplayStatus()
    {
        if (playerSO != null)
        {
            UpdateUI(); // UI ������Ʈ
        }
    }

    // UI�� ������Ʈ�ϴ� �޼���
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

    // ��ȭ Ƚ���� ���� ��ȭ ��� ���
    public int CalculateEnhanceCost()
    {
        int enhanceCost = baseEnhanceCost + (enhanceCount * 50); // ��ȭ Ƚ���� ����Ͽ� ��� ����

        return enhanceCost;
    }
}
