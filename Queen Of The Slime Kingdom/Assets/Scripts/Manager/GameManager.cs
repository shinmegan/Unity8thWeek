using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform PlayerTransform { get; private set; } // 플레이어
    public ObjectPool ObjectPool { get; private set; } // 오브젝트 풀
    public Coin coin; // 재화
    [SerializeField] private string playerTag = "Player";
    public int currentStage = 1; // 현재 스테이지 정보

    private void Awake()
    {
        if (Instance == null)
        {   // 인스턴스 초기화
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {   // 중복 인스턴스 제거
                Destroy(gameObject);
                return;
            }
        }

        PlayerTransform = GameObject.FindGameObjectWithTag(playerTag)?.transform;
        ObjectPool = GetComponent<ObjectPool>();

        // Pool 정보 설정
        ObjectPool.Initialize(); // 오브젝트 풀 초기화
        coin = FindObjectOfType<Coin>(); // 코인 초기화
    }

    public void AdvanceToNextStage()
    {
        currentStage++;
        UpdateMonsterStatsForStage(currentStage);
    }

    private void UpdateMonsterStatsForStage(int stage)
    {
        Monster[] monsters = FindObjectsOfType<Monster>();
        foreach (var monster in monsters)
        {
            //monster.currentStage = stage;
            //var stageStats = monster.stats.GetStatsForStage(stage);
            //monster.stats.defaultHP = stageStats.HP;
            //monster.stats.defaultAttackPower = stageStats.attackPower;
            //monster.stats.defaultAttackSpeed = stageStats.attackSpeed;
            //monster.stats.defaultMoveSpeed = stageStats.moveSpeed;
        }
    }

    // 경험치 획득 메서드
    public void GainExperience(Player player, int amount)
    {
        player.currentExperience += amount;

        // 경험치 바 업데이트
        player.UpdateExperienceUI();

    }

    // 레벨 업 체크 메서드
    public void CheckLevelUp(Player player)
    {
        if (player.currentExperience >= player.maxExperience)
        {
            // 레벨 상승
            player.level++;

            // 경험치 초기화 및 최대 경험치 증가
            player.currentExperience = 0;
            player.maxExperience = Mathf.RoundToInt(player.maxExperience * 1.2f); // 기존 최대 경험치의 1.2배로 증가

            // 플레이어 레벨업 처리
            player.OnLevelUp();

            // 경험치 바 업데이트
            player.UpdateExperienceUI();

            // 레벨 UI 업데이트
            player.UpdateLevelUI();

        }
    }
}
