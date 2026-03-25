using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Reference")]
    // UI 전용 매니저
    [SerializeField] private UIManager uiManager;

    // 구멍 9개
    [SerializeField] private MoleHole[] holes;

    [Header("Game Settings")]
    // 전체 게임 시간
    [SerializeField] private float gameDuration = 30f;

    [Header("Difficulty Settings")]
    // 초반 : 두더지가 오래 보임
    [SerializeField] private float startAppearTime = 1.5f;

    // 후반 : 두더지가 짧게 보임
    [SerializeField] private float endAppearTime = 1f;

    // 초반 : 다음 등장까지 간격
    [SerializeField] private float startSpawnDelay = 0.8f;

    // 후반 : 다음 등장까지 간격
    [SerializeField] private float endSpawnDelay = 0.5f;

    // 초반 최대 동시 등장 수
    [SerializeField] private int startMaxSimultaneous = 1;

    // 후반 최대 동시 등장 수
    [SerializeField] private int endMaxSimultaneous = 2;

    [Header("Score Settings")]
    // 일반 두더지 점수
    [SerializeField] private int normalScore = 1;

    // 보너스 두더지 점수
    [SerializeField] private int bonusScore = 3;

    // 함정 두더지 패널티
    [SerializeField] private int trapPenalty = 2;

    [Header("Spawn Chance")]
    // 일반 등장 확률
    [Range(0f, 1f)]
    [SerializeField] private float normalChance = 0.7f;

    // 보너스 등장 확률
    [Range(0f, 1f)]
    [SerializeField] private float bonusChance = 0.2f;

    // 남는 확률은 함정

    // 현재 점수
    private int score = 0;

    // 최고 점수
    private int bestScore = 0;

    // 남은 시간
    private float remainTime = 0f;

    // 게임 진행 여부
    private bool isPlaying = false;

    // 두더지 생성 루프
    private Coroutine moleRoutine;

    // 최고 점수 저장 키
    private const string BEST_SCORE_KEY = "WhackMoleBestScore";

    private void Start()
    {
        // 저장된 최고 점수 불러오기
        bestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);

        // 각 Hole 초기화
        for (int i = 0; i < holes.Length; i++)
        {
            if (holes[i] != null)
            {
                holes[i].Init(this);
            }
        }

        // 시작 시 타이틀 화면 표시
        uiManager.ShowTitlePanel();
        uiManager.UpdateGameUI(0, bestScore, gameDuration);
    }

    private void Update()
    {
        // 게임 중일 때만 시간 감소
        if (!isPlaying)
            return;

        remainTime -= Time.deltaTime;

        if (remainTime < 0f)
            remainTime = 0f;

        // UI 갱신은 UIManager에게 맡김
        uiManager.UpdateGameUI(score, bestScore, remainTime);

        // 시간이 다 되면 종료
        if (remainTime <= 0f)
        {
            EndGame();
        }
    }

    // -----------------------------
    // 시작 버튼용
    // -----------------------------
    public void StartGame()
    {
        score = 0;
        remainTime = gameDuration;
        isPlaying = true;

        HideAllMoles();

        uiManager.ShowGamePanel();
        uiManager.UpdateGameUI(score, bestScore, remainTime);

        StartMoleRoutine();
    }

    // -----------------------------
    // 재시작 버튼용
    // -----------------------------
    public void RestartGame()
    {
        StartGame();
    }

    // -----------------------------
    // 타이틀로 버튼용
    // -----------------------------
    public void ReturnToTitle()
    {
        isPlaying = false;

        StopMoleRoutine();
        HideAllMoles();

        uiManager.ShowTitlePanel();
        uiManager.UpdateGameUI(0, bestScore, gameDuration);
    }

    // -----------------------------
    // 두더지 클릭 시 MoleHole이 호출
    // -----------------------------
    public void OnMoleClicked(MoleHole.MoleType type)
    {
        if (!isPlaying)
            return;

        switch (type)
        {
            case MoleHole.MoleType.Normal:
                score += normalScore;
                break;

            case MoleHole.MoleType.Bonus:
                score += bonusScore;
                break;

            case MoleHole.MoleType.Trap:
                score -= trapPenalty;
                break;
        }

        if (score < 0)
            score = 0;

        uiManager.UpdateGameUI(score, bestScore, remainTime);
    }

    // -----------------------------
    // 게임 종료
    // -----------------------------
    private void EndGame()
    {
        if (!isPlaying)
            return;

        isPlaying = false;

        StopMoleRoutine();
        HideAllMoles();

        // 최고 점수 저장
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt(BEST_SCORE_KEY, bestScore);
            PlayerPrefs.Save();
        }

        uiManager.UpdateResultUI(score, bestScore);
        uiManager.ShowResultPanel();
    }

    // -----------------------------
    // 모든 두더지 숨기기
    // -----------------------------
    private void HideAllMoles()
    {
        for (int i = 0; i < holes.Length; i++)
        {
            if (holes[i] != null)
            {
                holes[i].Hide();
            }
        }
    }

    // -----------------------------
    // 두더지 등장 루틴 시작
    // -----------------------------
    private void StartMoleRoutine()
    {
        StopMoleRoutine();
        moleRoutine = StartCoroutine(MoleRoutine());
    }

    private void StopMoleRoutine()
    {
        if (moleRoutine != null)
        {
            StopCoroutine(moleRoutine);
            moleRoutine = null;
        }
    }

    // -----------------------------
    // 현재 진행도(0~1) 구하기
    // 0 = 초반, 1 = 후반
    // -----------------------------
    private float GetProgress()
    {
        return 1f - (remainTime / gameDuration);
    }

    // 현재 두더지 표시 시간
    private float GetCurrentAppearTime()
    {
        float progress = GetProgress();
        return Mathf.Lerp(startAppearTime, endAppearTime, progress);
    }

    // 현재 다음 등장까지 간격
    private float GetCurrentSpawnDelay()
    {
        float progress = GetProgress();
        return Mathf.Lerp(startSpawnDelay, endSpawnDelay, progress);
    }

    // 현재 최대 동시 등장 수
    private int GetCurrentMaxSimultaneous()
    {
        float progress = GetProgress();
        float value = Mathf.Lerp(startMaxSimultaneous, endMaxSimultaneous, progress);
        return Mathf.RoundToInt(value);
    }

    // -----------------------------
    // 타입 랜덤 결정
    // -----------------------------
    private MoleHole.MoleType GetRandomType()
    {
        float rand = Random.value;

        if (rand < normalChance)
            return MoleHole.MoleType.Normal;
        else if (rand < normalChance + bonusChance)
            return MoleHole.MoleType.Bonus;
        else
            return MoleHole.MoleType.Trap;
    }

    // -----------------------------
    // 현재 비어 있는 Hole 목록 가져오기
    // -----------------------------
    private List<MoleHole> GetHiddenHoles()
    {
        List<MoleHole> hiddenHoles = new List<MoleHole>();

        for (int i = 0; i < holes.Length; i++)
        {
            if (holes[i] != null && !holes[i].IsVisible)
            {
                hiddenHoles.Add(holes[i]);
            }
        }

        return hiddenHoles;
    }

    // -----------------------------
    // 구멍 하나를 일정 시간 보여주고 숨기기
    // -----------------------------
    private IEnumerator ShowAndHideHole(MoleHole hole, MoleHole.MoleType type, float appearTime)
    {
        hole.Show(type);

        yield return new WaitForSeconds(appearTime);

        if (hole.IsVisible)
        {
            hole.Hide();
        }
    }

    // -----------------------------
    // 두더지 생성 루프
    // -----------------------------
    private IEnumerator MoleRoutine()
    {
        while (isPlaying)
        {
            float currentAppearTime = GetCurrentAppearTime();
            float currentSpawnDelay = GetCurrentSpawnDelay();
            int currentMaxSimultaneous = GetCurrentMaxSimultaneous();

            // 비어 있는 구멍 목록
            List<MoleHole> hiddenHoles = GetHiddenHoles();

            if (hiddenHoles.Count == 0)
            {
                yield return new WaitForSeconds(currentSpawnDelay);
                continue;
            }

            // 이번 턴에 몇 마리 나올지 결정
            int spawnCount = Random.Range(1, currentMaxSimultaneous + 1);
            spawnCount = Mathf.Min(spawnCount, hiddenHoles.Count);

            for (int i = 0; i < spawnCount; i++)
            {
                int randomIndex = Random.Range(0, hiddenHoles.Count);
                MoleHole selectedHole = hiddenHoles[randomIndex];

                // 중복 선택 방지
                hiddenHoles.RemoveAt(randomIndex);

                MoleHole.MoleType randomType = GetRandomType();

                StartCoroutine(ShowAndHideHole(selectedHole, randomType, currentAppearTime));
            }

            yield return new WaitForSeconds(currentSpawnDelay);
        }
    }
}