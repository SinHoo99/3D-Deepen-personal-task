using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject enhanceUIPanel; // 강화 UI 패널
    [SerializeField] private EnhanceUI enhanceUI; // EnhanceUI 스크립트 참조
    [SerializeField] private Player player; // Player 클래스 참조

    private bool isEnhanceUIVisible = false; // 강화 UI의 활성화 상태

    // 강화 UI를 켜거나 끄는 메서드
    public void OnToggleEnhanceUI()
    {
        isEnhanceUIVisible = !isEnhanceUIVisible;
        enhanceUIPanel.SetActive(isEnhanceUIVisible);
    }

    // 강화 버튼이 클릭될 때 호출되는 메서드
    public void OnEnhance()
    {
        if (player == null)
        {
            Debug.LogError("Player가 설정되지 않았습니다.");
            return;
        }

        // 강화 비용 계산
        int enhanceCost = enhanceUI.CalculateEnhanceCost();

        // 골드가 강화 비용보다 적으면 강화를 실행하지 않음
        if (player.gold < enhanceCost)
        {
            Debug.Log("골드가 부족하여 강화를 할 수 없습니다.");
            return;
        }

        // 골드 차감
        player.gold -= enhanceCost;

        // 강화 시도
        if (enhanceUI != null)
        {
            enhanceUI.TryEnhance(); // TryEnhance 메서드 호출
        }
        else
        {
            Debug.LogError("EnhanceUI가 설정되지 않았습니다.");
        }
    }
}
