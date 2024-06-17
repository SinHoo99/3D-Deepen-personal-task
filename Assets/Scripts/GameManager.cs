using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public ObjectPool ObjectPool { get; private set; }
    private Player _player;
    public float spawnInterval = 5f; // ���� �����ϴ� ���� (��)
    public GameObject player; // �÷��̾� GameObject ����


    //�ٸ� ��ũ��Ʈ�� �ű� ���� �ӽ÷� ���ӸŴ����� �־����

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

        // ObjectPool ������Ʈ�� ã�Ƽ� ObjectPool�� �Ҵ�
        ObjectPool = FindObjectOfType<ObjectPool>();
        if (ObjectPool == null)
        {
            Debug.LogError("ObjectPool ������Ʈ�� ã�� ���߽��ϴ�.");
        }

        // �÷��̾� GameObject���� Player ������Ʈ�� ã�Ƽ� PlayerSO�� ������
        _player = player.GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("�÷��̾�(Player) ������Ʈ�� ã�� ���߽��ϴ�.");
        }
        else
        {
            if (_player.Data != null)
            {
                _player.Data.OnExperienceChanged += UpdateExpUI;
                _player.Data.OnLevelChanged += UpdateLevelUI;
                _player.Data.OnGoldChanged += UpdateGoldUI;
            }
            else
            {
                Debug.LogError("PlayerSO reference is null in GameManager.");
            }

            // GameManager������ PlayerSO�� ���� ����ϴ� ���, Player Ŭ������ Data �ʵ带 ���
            UpdateExpUI(_player.Data?.Experience ?? 0);
            UpdateLevelUI(_player.Data?.Level ?? 1);
            UpdateGoldUI(_player.Data?.Gold ?? 0);
        }
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
            Debug.Log("�ʱ� �� ���� �Ϸ�");
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
                Debug.Log("�߰� �� ���� �Ϸ�");
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (_player == null)
        {
            Debug.LogError("�÷��̾ ã�� �� �����ϴ�.");
            return Vector3.zero;
        }

        Vector3 playerPosition = player.transform.position;

        // �÷��̾��� ��ġ���� ���� ���� ������ ������ ��ġ ���
        float randomXOffset = Random.Range(-30f, 30f);
        float randomZOffset = Random.Range(-30f, 30f);

        // ������ ��ġ ���
        Vector3 spawnPosition = new Vector3(
            playerPosition.x + randomXOffset,
            0.7f, // Y�� ����
            playerPosition.z + randomZOffset
        );

        return spawnPosition;
    }

    private void OnDestroy()
    {
        // �̺�Ʈ ���� ����
        if (_player != null)
        {
            _player.Data.OnExperienceChanged -= UpdateExpUI;
            _player.Data.OnLevelChanged -= UpdateLevelUI;
            _player.Data.OnGoldChanged -= UpdateGoldUI;
        }
    }
    private void UpdateExpUI(int experience)
    {
        if (_player != null && ExpGaugeSlider != null)
        {
            ExpGaugeSlider.value = (float)_player.Data.Experience / _player.Data.ExperienceToNextLevel;
            ExpText.text = $"{_player.Data.Experience} / {_player.Data.ExperienceToNextLevel}";
            Debug.Log($"����ġ UI ������Ʈ: {_player.Data.Experience}/{_player.Data.ExperienceToNextLevel}");
        }
        else
        {
            Debug.LogError("�÷��̾� �Ǵ� ExpGaugeSlider�� ã�� �� �����ϴ�.");
        }
    }

    private void UpdateLevelUI(int level)
    {
        if (_player != null && LevelText != null)
        {
            LevelText.text = $"Level :{level}";
            Debug.Log($"���� UI ������Ʈ: {level}");
        }
        else
        {
            Debug.LogError("�÷��̾� �Ǵ� LevelText�� ã�� �� �����ϴ�.");
        }   
    }
    void UpdateGoldUI(int gold)
    {
        GoldText.text = $"Gold: {gold}";
    }

}
