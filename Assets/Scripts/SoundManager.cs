using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip coinClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // AudioSource가 지정되지 않았을 경우 자동 컴포넌트 추가
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }
    }

    public void PlayCoinSound()
    {
        if (sfxSource != null && coinClip != null)
        {
            // 연타 시 소리가 끊기지 않고 겹쳐서 찰지게 출력되도록 PlayOneShot 사용
            sfxSource.PlayOneShot(coinClip);
        }
    }
}