using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Panels")]
    // 시작 화면 패널
    [SerializeField] private GameObject titlePanel;

    // 게임 화면 패널
    [SerializeField] private GameObject gamePanel;

    // 결과 화면 패널
    [SerializeField] private GameObject resultPanel;

    [Header("Game UI")]
    // 현재 점수 텍스트
    [SerializeField] private TMP_Text scoreText;

    // 최고 점수 텍스트 (게임 중 표시)
    [SerializeField] private TMP_Text bestScoreText;

    // 남은 시간 텍스트
    [SerializeField] private TMP_Text timeText;

    [Header("Result UI")]
    // 결과창의 최종 점수 텍스트
    [SerializeField] private TMP_Text finalScoreText;

    // 결과창의 최고 점수 텍스트
    [SerializeField] private TMP_Text resultBestScoreText;

    [Header("Holes")]
    // 9개의 Hole
    [SerializeField] private MoleHole[] holes;

    [Header("Game Settings")]
    // 전체 플레이 시간
    [SerializeField] private float gameDuration = 30f;

    // 적이 화면에 머무는 시간
    [SerializeField] private float appearTime = 1.0f;

    // 다음 적이 나오기 전까지 대기 시간
    [SerializeField] private float delayTime = 0.5f;

    // 현재 점수
    private int score = 0;

    // 최고 점수
    private int bestScore = 0;

    // 남은 시간
    private float remainTime = 0f;

    // 현재 게임 진행 중인지
    private bool isPlaying = false;

    // 두더지 반복 등장 코루틴 저장용
    private Coroutine moleRoutineCoroutine;

    // 최고 점수 저장용 키
    private const string BEST_SCORE_KEY = "WhackMoleBestScore";

    private void Start()
    {
        // 저장된 최고 점수 불러오기
        bestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);

        // 각 Hole에 GameManager 연결 + 초기화
        for (int i = 0; i < holes.Length; i++)
        {
            holes[i].Init(this);
        }

        // 시작 시 타이틀 화면 표시
        ShowTitle();
    }

    private void Update()
    {
        // 게임 중이 아니면 시간 감소 안 함
        if (!isPlaying)
            return;

        // 남은 시간 감소
        remainTime -= Time.deltaTime;

        // 0보다 작아지지 않도록
        if (remainTime < 0f)
            remainTime = 0f;

        // 게임 UI 갱신
        UpdateGameUI();

        // 시간이 다 되면 게임 종료
        if (remainTime <= 0f)
        {
            EndGame();
        }
    }

    // 타이틀 화면 표시
    public void ShowTitle()
    {
        // 게임 중지 상태
        isPlaying = false;

        // 돌고 있던 코루틴이 있으면 정지
        if (moleRoutineCoroutine != null)
        {
            StopCoroutine(moleRoutineCoroutine);
            moleRoutineCoroutine = null;
        }

        // 남아있는 적 전부 숨기기
        HideAllMoles();

        // 패널 전환
        titlePanel.SetActive(true);
        gamePanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    // 시작 버튼 또는 재시작 버튼이 눌리면 호출
    public void StartGame()
    {
        // 현재 점수 초기화
        score = 0;

        // 남은 시간 초기화
        remainTime = gameDuration;

        // 게임 진행 시작
        isPlaying = true;

        // 시작 전에 적 전부 숨김
        HideAllMoles();

        // 게임 UI 먼저 갱신
        UpdateGameUI();

        // 패널 전환
        titlePanel.SetActive(false);
        gamePanel.SetActive(true);
        resultPanel.SetActive(false);

        // 혹시 기존 코루틴이 남아있다면 정지
        if (moleRoutineCoroutine != null)
        {
            StopCoroutine(moleRoutineCoroutine);
        }

        // 두더지 등장 반복 시작
        moleRoutineCoroutine = StartCoroutine(MoleRoutine());
    }

    // 결과창에서 다시 시작 버튼용
    public void RestartGame()
    {
        StartGame();
    }

    // 점수 추가
    public void AddScore(int amount)
    {
        // 게임 중일 때만 점수 처리
        if (!isPlaying)
            return;

        score += amount;

        // 음수 방지
        if (score < 0)
            score = 0;

        UpdateGameUI();
    }

    // 게임 종료
    private void EndGame()
    {
        // 중복 종료 방지
        if (!isPlaying)
            return;

        isPlaying = false;

        // 코루틴 정지
        if (moleRoutineCoroutine != null)
        {
            StopCoroutine(moleRoutineCoroutine);
            moleRoutineCoroutine = null;
        }

        // 모든 적 숨기기
        HideAllMoles();

        // 최고 점수 갱신 및 저장
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt(BEST_SCORE_KEY, bestScore);
            PlayerPrefs.Save();
        }

        // 결과창 텍스트 갱신
        finalScoreText.text = "Final Score : " + score;
        resultBestScoreText.text = "Best Score : " + bestScore;

        // 결과 패널 표시
        titlePanel.SetActive(false);
        gamePanel.SetActive(false);
        resultPanel.SetActive(true);
    }

    // 게임 중 UI 갱신
    private void UpdateGameUI()
    {
        scoreText.text = "Score : " + score;
        bestScoreText.text = "Best : " + bestScore;
        timeText.text = "Time : " + Mathf.CeilToInt(remainTime);
    }

    // 모든 Hole의 적 숨기기
    private void HideAllMoles()
    {
        for (int i = 0; i < holes.Length; i++)
        {
            holes[i].Hide();
        }
    }

    // 적이 랜덤 위치에서 반복 등장하는 코루틴
    private IEnumerator MoleRoutine()
    {
        while (isPlaying)
        {
            // 랜덤 구멍 선택
            int randomIndex = Random.Range(0, holes.Length);
            MoleHole hole = holes[randomIndex];

            // 현재 안 보이는 구멍이면 적 등장
            if (!hole.IsVisible)
            {
                hole.Show();

                // 일정 시간 동안 보이기
                yield return new WaitForSeconds(appearTime);

                // 아직 클릭 안 됐다면 숨기기
                if (hole.IsVisible)
                {
                    hole.Hide();
                }
            }

            // 다음 적 나오기 전 잠깐 대기
            yield return new WaitForSeconds(delayTime);
        }
    }
}