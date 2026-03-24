using UnityEngine;
using UnityEngine.UI;

public class MoleHole : MonoBehaviour
{
    // 적 버튼
    [SerializeField] private Button enemyButton;

    // 점수 처리를 위해 GameManager 참조
    private GameManager gameManager;

    // 구멍에 적이 보이는 상태인지?
    private bool isVisible = false;

    // 상태 확인용 프로퍼티
    public bool IsVisible => isVisible;

    // GameManager가 시작할 때 호출
    // 각 Hole이 자기 GameManager를 기억하게 함
    public void Init(GameManager manager)
    {
        gameManager = manager;

        // 버튼 클릭 이벤트 초기화 (중복 방지)
        enemyButton.onClick.RemoveAllListeners();

        // 버튼 클릭 시 OnClickEnemy 실행되도록 연결
        enemyButton.onClick.AddListener(OnClickEnemy);

        // 시작할 때는 적 숨김 상태
        Hide();
    }

    // 적 등장
    public void Show()
    {
        isVisible = true;

        // Enemy 버튼 활성화 → 화면에 나타남
        enemyButton.gameObject.SetActive(true);
    }

    // 적 숨김
    public void Hide()
    {
        isVisible = false;

        // Enemy 버튼 비활성화
        enemyButton.gameObject.SetActive(false);
    }

    // 적 클릭 시 실행되는 함수
    private void OnClickEnemy()
    {
        // 없으면 무시
        if (!isVisible) return;

        // 점수 1 획득
        gameManager.AddScore(1);

        // 클릭시 숨김
        Hide();
    }
}