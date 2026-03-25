using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    // 시작 화면 패널
    [SerializeField] private GameObject titlePanel;
    // 설명 화면 패널
    [SerializeField] private GameObject descPanel;
    // 실제 게임 화면 패널
    [SerializeField] private GameObject gamePanel;
    // 게임 종료 후 결과 화면 패널
    [SerializeField] private GameObject resultPanel;

    [Header("Game UI")]
    // 게임 중 현재 점수
    [SerializeField] private TMP_Text scoreText;

    // 게임 중 최고 점수
    [SerializeField] private TMP_Text bestScoreText;

    // 게임 중 남은 시간
    [SerializeField] private TMP_Text timeText;

    [Header("Result UI")]
    // 결과창 최종 점수
    [SerializeField] private TMP_Text finalScoreText;

    // 결과창 최고 점수
    [SerializeField] private TMP_Text resultBestScoreText;

    // -----------------------------
    // 타이틀 화면 표시
    // -----------------------------
    public void ShowTitlePanel()
    {
        titlePanel.SetActive(true);
        descPanel.SetActive(false);
        gamePanel.SetActive(false);
        resultPanel.SetActive(false);
    }


    // -----------------------------
    // 설명 화면 표시
    // -----------------------------
    public void ShowDescPanel()
    {
        titlePanel.SetActive(false);
        descPanel.SetActive(true);
        gamePanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    // -----------------------------
    // 게임 화면 표시
    // -----------------------------
    public void ShowGamePanel()
    {
        titlePanel.SetActive(false);
        gamePanel.SetActive(true);
        resultPanel.SetActive(false);
    }

    // -----------------------------
    // 결과 화면 표시
    // -----------------------------
    public void ShowResultPanel()
    {
        titlePanel.SetActive(false);
        gamePanel.SetActive(false);
        resultPanel.SetActive(true);
    }

    // -----------------------------
    // 게임 중 UI 갱신
    // -----------------------------
    public void UpdateGameUI(int score, int bestScore, float remainTime)
    {
        scoreText.text = "Score : " + score;
        bestScoreText.text = "Best Score : " + bestScore;
        timeText.text = "Time : " + Mathf.CeilToInt(remainTime);
    }

    // -----------------------------
    // 결과창 UI 갱신
    // -----------------------------
    public void UpdateResultUI(int finalScore, int bestScore)
    {
        finalScoreText.text = "Final Score : " + finalScore;
        resultBestScoreText.text = "Best Score : " + bestScore;
    }
}