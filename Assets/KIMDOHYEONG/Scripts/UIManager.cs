using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    // ���� ȭ�� �г�
    [SerializeField] private GameObject titlePanel;
    // ���� ȭ�� �г�
    [SerializeField] private GameObject descPanel;
    // ���� ���� ȭ�� �г�
    [SerializeField] private GameObject gamePanel;
    // ���� ���� �� ��� ȭ�� �г�
    [SerializeField] private GameObject resultPanel;

    [Header("Game UI")]
    // ���� �� ���� ����
    [SerializeField] private TMP_Text scoreText;

    // ���� �� �ְ� ����
    [SerializeField] private TMP_Text bestScoreText;

    // ���� �� ���� �ð�
    [SerializeField] private TMP_Text timeText;

    [Header("Result UI")]
    // ���â ���� ����
    [SerializeField] private TMP_Text finalScoreText;

    // ���â �ְ� ����
    [SerializeField] private TMP_Text resultBestScoreText;

    // -----------------------------
    // Ÿ��Ʋ ȭ�� ǥ��
    // -----------------------------
    public void ShowTitlePanel()
    {
        titlePanel.SetActive(true);
        descPanel.SetActive(false);
        gamePanel.SetActive(false);
        resultPanel.SetActive(false);
    }


    // -----------------------------
    // ���� ȭ�� ǥ��
    // -----------------------------
    public void ShowDescPanel()
    {
        titlePanel.SetActive(false);
        descPanel.SetActive(true);
        gamePanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    // -----------------------------
    // ���� ȭ�� ǥ��
    // -----------------------------
    public void ShowGamePanel()
    {
        titlePanel.SetActive(false);
        gamePanel.SetActive(true);
        resultPanel.SetActive(false);
    }

    // -----------------------------
    // ��� ȭ�� ǥ��
    // -----------------------------
    public void ShowResultPanel()
    {
        titlePanel.SetActive(false);
        gamePanel.SetActive(false);
        resultPanel.SetActive(true);
    }

    // -----------------------------
    // ���� �� UI ����
    // -----------------------------
    public void UpdateGameUI(int score, int bestScore, float remainTime)
    {
        scoreText.text = "Score : " + score;
        bestScoreText.text = "Best : " + bestScore;
        timeText.text = "Time : " + Mathf.CeilToInt(remainTime);
    }

    // -----------------------------
    // ���â UI ����
    // -----------------------------
    public void UpdateResultUI(int finalScore, int bestScore)
    {
        finalScoreText.text = "Final Score : " + finalScore;
        resultBestScoreText.text = "Best Score : " + bestScore;
    }
}