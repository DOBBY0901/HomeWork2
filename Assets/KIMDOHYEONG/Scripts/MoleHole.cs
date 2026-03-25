using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MoleHole : MonoBehaviour
{
    // 두더지 종류
    public enum MoleType
    {
        Normal, // 일반
        Bonus,  // 추가 점수
        Trap    // 함정
    }

    [Header("Enemy Button")]
    // 클릭할 버튼
    [SerializeField] private Button enemyButton;

    [Header("Enemy Image")]
    // 버튼에 붙어 있는 이미지
    [SerializeField] private Image enemyImage;

    [Header("Normal Sprites")]
    // 일반 두더지 기본 얼굴
    [SerializeField] private Sprite normalSprite;

    // 일반 두더지 맞은 얼굴
    [SerializeField] private Sprite normalHitSprite;

    [Header("Bonus Sprites")]
    // 보너스 두더지 기본 얼굴
    [SerializeField] private Sprite bonusSprite;

    // 보너스 두더지 맞은 얼굴
    [SerializeField] private Sprite bonusHitSprite;

    [Header("Trap Sprites")]
    // 함정 기본 얼굴
    [SerializeField] private Sprite trapSprite;

    // 함정 맞은 얼굴
    [SerializeField] private Sprite trapHitSprite;

    [Header("Sound")]
    // 효과음을 재생할 AudioSource
    [SerializeField] private AudioSource audioSource;

    // 일반 클릭 소리
    [SerializeField] private AudioClip normalHitSound;

    // 보너스 클릭 소리
    [SerializeField] private AudioClip bonusHitSound;

    // 함정 클릭 소리
    [SerializeField] private AudioClip trapHitSound;

    [Header("Hit Effect")]
    // 클릭 후 맞은 얼굴이 잠깐 보이는 시간
    [SerializeField] private float hitHideDelay = 0.15f;

    // 게임 전체 관리
    private GameManager gameManager;

    // 현재 보이는 상태인지
    private bool isVisible = false;

    // 현재 어떤 타입이 떠 있는지
    private MoleType currentType;

    // 이미 클릭 처리 중인지
    private bool isHit = false;

    // 바깥에서 상태 확인용
    public bool IsVisible => isVisible;

    // 초기화
    public void Init(GameManager manager)
    {
        gameManager = manager;

        // 버튼 클릭 이벤트 중복 방지
        enemyButton.onClick.RemoveAllListeners();
        enemyButton.onClick.AddListener(OnClickEnemy);

        // 시작 시 숨김
        Hide();
    }

    // 타입을 받아서 보여주기
    public void Show(MoleType type)
    {
        currentType = type;
        isVisible = true;
        isHit = false;

        // 타입별 기본 얼굴 표시
        switch (currentType)
        {
            case MoleType.Normal:
                enemyImage.sprite = normalSprite;
                break;

            case MoleType.Bonus:
                enemyImage.sprite = bonusSprite;
                break;

            case MoleType.Trap:
                enemyImage.sprite = trapSprite;
                break;
        }

        enemyButton.gameObject.SetActive(true);
    }

    // 숨기기
    public void Hide()
    {
        isVisible = false;
        isHit = false;
        enemyButton.gameObject.SetActive(false);
    }

    // 클릭 시 실행
    private void OnClickEnemy()
    {
        // 안 보이거나 이미 맞은 상태면 무시
        if (!isVisible || isHit)
            return;

        isHit = true;

        // 타입별 점수 처리는 GameManager에게 맡김
        gameManager.OnMoleClicked(currentType);

        // 타입별 맞은 얼굴 + 타입별 소리
        switch (currentType)
        {
            case MoleType.Normal:
                enemyImage.sprite = normalHitSprite;
                PlaySound(normalHitSound);
                break;

            case MoleType.Bonus:
                enemyImage.sprite = bonusHitSprite;
                PlaySound(bonusHitSound);
                break;

            case MoleType.Trap:
                enemyImage.sprite = trapHitSprite;
                PlaySound(trapHitSound);
                break;
        }

        // 맞은 얼굴 잠깐 보여주고 숨김
        StartCoroutine(HideAfterHit());
    }

    // 클릭 후 잠깐 기다렸다가 숨김
    private IEnumerator HideAfterHit()
    {
        yield return new WaitForSeconds(hitHideDelay);
        Hide();
    }

    // 소리 재생
    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip);
    }
}