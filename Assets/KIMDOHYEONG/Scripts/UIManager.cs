using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    // 타이틀 패널
    [SerializeField] private GameObject titlePanel;
    // 설명 패널
    [SerializeField] private GameObject descPanel;
    // 게임 패널
    [SerializeField] private GameObject gamePanel;
    // 게임결과 패널
    [SerializeField] private GameObject resultPanel;

    [Header("Game UI")]
    // 현재 점수 텍스트
    [SerializeField] private TMP_Text scoreText;

    // 최고 점수 텍스트    
    [SerializeField] private TMP_Text bestScoreText;

    // 시간 텍스트
    [SerializeField] private TMP_Text timeText;

    [Header("Result UI")]
    // 최종 점수 텍스트
    [SerializeField] private TMP_Text finalScoreText;

    // 최종 최고 점수 텍스트
    [SerializeField] private TMP_Text resultBestScoreText;

    // -----------------------------
    // 타이틀 패널 출력
    // -----------------------------
    public void ShowTitlePanel()
    {
        titlePanel.SetActive(true);
        descPanel.SetActive(false);
        gamePanel.SetActive(false);
        resultPanel.SetActive(false);
    }


    // -----------------------------
    // 설명 패널 출력
    // -----------------------------
    public void ShowDescPanel()
    {
        titlePanel.SetActive(false);
        descPanel.SetActive(true);
        gamePanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    // -----------------------------
    // 게임 패널 출력
    // -----------------------------
    public void ShowGamePanel()
    {
        titlePanel.SetActive(false);
        gamePanel.SetActive(true);
        resultPanel.SetActive(false);
    }

    // -----------------------------
    // 결과 패널 출력
    // -----------------------------
    public void ShowResultPanel()
    {
        titlePanel.SetActive(false);
        gamePanel.SetActive(false);
        resultPanel.SetActive(true);
    }

    // -----------------------------
    // UI 갱신 (점수, 최고 점수, 남은 시간)
    // -----------------------------
    public void UpdateGameUI(int score, int bestScore, float remainTime)
    {
        scoreText.text = "Score : " + score;
        bestScoreText.text = "Best : " + bestScore;
        timeText.text = "Time : " + Mathf.CeilToInt(remainTime);
    }

    // -----------------------------
    // 결과 UI 갱신 (최종 점수, 최종 최고 점수)
    // -----------------------------
    public void UpdateResultUI(int finalScore, int bestScore)
    {
        finalScoreText.text = "Final Score : " + finalScore;
        resultBestScoreText.text = "Best Score : " + bestScore;
    }
}