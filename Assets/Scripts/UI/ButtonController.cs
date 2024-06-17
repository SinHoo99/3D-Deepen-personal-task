using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject enhanceUIPanel; // ��ȭ UI �г�
    [SerializeField] private EnhanceUI enhanceUI; // EnhanceUI ��ũ��Ʈ ����
    [SerializeField] private Player player; // Player Ŭ���� ����

    private bool isEnhanceUIVisible = false; // ��ȭ UI�� Ȱ��ȭ ����

    // ��ȭ UI�� �Ѱų� ���� �޼���
    public void OnToggleEnhanceUI()
    {
        isEnhanceUIVisible = !isEnhanceUIVisible;
        enhanceUIPanel.SetActive(isEnhanceUIVisible);
    }

    // ��ȭ ��ư�� Ŭ���� �� ȣ��Ǵ� �޼���
    public void OnEnhance()
    {
        if (player == null)
        {
            Debug.LogError("Player�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // ��ȭ ��� ���
        int enhanceCost = enhanceUI.CalculateEnhanceCost();

        // ��尡 ��ȭ ��뺸�� ������ ��ȭ�� �������� ����
        if (player.gold < enhanceCost)
        {
            Debug.Log("��尡 �����Ͽ� ��ȭ�� �� �� �����ϴ�.");
            return;
        }

        // ��� ����
        player.gold -= enhanceCost;

        // ��ȭ �õ�
        if (enhanceUI != null)
        {
            enhanceUI.TryEnhance(); // TryEnhance �޼��� ȣ��
        }
        else
        {
            Debug.LogError("EnhanceUI�� �������� �ʾҽ��ϴ�.");
        }
    }
}
