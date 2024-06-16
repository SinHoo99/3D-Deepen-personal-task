using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public ObjectPool ObjectPool { get; private set; }
    private Player _player;
    public float spawnInterval = 5f; // 적을 생성하는 간격 (초)
    public GameObject player; // 플레이어 GameObject 참조


    //다른 스크립트로 옮길 변수 임시로 게임매니저에 넣어놓음

    [SerializeField] private TextMeshProUGUI GoldText;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI ExpText;
    [SerializeField] private Slider ExpGaugeSlider;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // ObjectPool 컴포넌트를 찾아서 ObjectPool에 할당
        ObjectPool = FindObjectOfType<ObjectPool>();
        if (ObjectPool == null)
        {
            Debug.LogError("ObjectPool 컴포넌트를 찾지 못했습니다.");
        }

        // 플레이어 참조 가져오기
        _player = player.GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("플레이어 컴포넌트를 찾지 못했습니다.");
        }
        else
        {
            _player.OnExperienceChanged += UpdateExpUI;
            _player.OnLevelChanged += UpdateLevelUI;
            _player.OnGoldChanged += UpdateGoldUI;
        }
        UpdateExpUI(_player.Experience); // 초기 UI 업데이트
        UpdateLevelUI(_player.Level);    // 초기 UI 업데이트
        UpdateGoldUI(_player.gold);

    }

    private void Start()
    {
        SpawnInitialEnemy();
        StartCoroutine(SpawnEnemyRoutine());
    }

    private void SpawnInitialEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject initialEnemy = ObjectPool.SpawnFromPool("Enemy", spawnPosition);
        if (initialEnemy != null)
        {
            Debug.Log("초기 적 생성 완료");
        }
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject newEnemy = ObjectPool.SpawnFromPool("Enemy", spawnPosition);
            if (newEnemy != null)
            {
                Debug.Log("추가 적 생성 완료");
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (_player == null)
        {
            Debug.LogError("플레이어를 찾을 수 없습니다.");
            return Vector3.zero;
        }

        Vector3 playerPosition = player.transform.position;

        // 플레이어의 위치에서 일정 범위 내에서 랜덤한 위치 계산
        float randomXOffset = Random.Range(-30f, 30f);
        float randomZOffset = Random.Range(-30f, 30f);

        // 랜덤한 위치 계산
        Vector3 spawnPosition = new Vector3(
            playerPosition.x + randomXOffset,
            0.7f, // Y축 고정
            playerPosition.z + randomZOffset
        );

        return spawnPosition;
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (_player != null)
        {
            _player.OnExperienceChanged -= UpdateExpUI;
            _player.OnLevelChanged -= UpdateLevelUI;
            _player.OnGoldChanged -= UpdateGoldUI;
        }
    }
    private void UpdateExpUI(int experience)
    {
        if (_player != null && ExpGaugeSlider != null)
        {
            ExpGaugeSlider.value = (float)_player.Experience / _player.ExperienceToNextLevel;
            ExpText.text = $"{_player.Experience} / {_player.ExperienceToNextLevel}";
            Debug.Log($"경험치 UI 업데이트: {_player.Experience}/{_player.ExperienceToNextLevel}");
        }
        else
        {
            Debug.LogError("플레이어 또는 ExpGaugeSlider를 찾을 수 없습니다.");
        }
    }

    private void UpdateLevelUI(int level)
    {
        if (_player != null && LevelText != null)
        {
            LevelText.text = $"Level :{level}";
            Debug.Log($"레벨 UI 업데이트: {level}");
        }
        else
        {
            Debug.LogError("플레이어 또는 LevelText를 찾을 수 없습니다.");
        }   
    }
    void UpdateGoldUI(int gold)
    {
        GoldText.text = $"Gold: {gold}";
    }

}
