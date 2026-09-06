using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinClickEffect : MonoBehaviour
{
    // touch to force recompile
    [Header("Punch Animation")]
    [SerializeField] private RectTransform targetTransform;
    [SerializeField] private float punchScale = 0.88f; // ��¦ ���ȴٰ� ����
    [SerializeField] private float punchDuration = 0.12f;

    [Header("Floating Text")]
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private Transform effectContainer;

    private Vector3 originalScale;
    private Coroutine punchCoroutine;

    private void Awake()
    {
        if (targetTransform == null)
            targetTransform = GetComponent<RectTransform>();

        originalScale = targetTransform.localScale;

        if (TryGetComponent<Button>(out var button))
        {
            button.onClick.AddListener(PlayEffects);
        }
    }

    public void PlayEffects()
    {
        // 1. ���� ��ġ �ִϸ��̼�
        if (punchCoroutine != null) StopCoroutine(punchCoroutine);
        punchCoroutine = StartCoroutine(PunchRoutine());

        // 2. ȹ�� ��� �÷��� �ؽ�Ʈ ����
        if (floatingTextPrefab != null && effectContainer != null && CurrencyManager.Instance != null)
        {
            GameObject obj = Instantiate(floatingTextPrefab, effectContainer);
            obj.transform.position = targetTransform.position;

            if (obj.TryGetComponent<FloatingText>(out var floatText))
            {
                double gained = CurrencyManager.Instance.GoldPerClick;
                floatText.Setup($"+{gained:N0}", new Color(1f, 0.85f, 0.2f, 1f)); // ��� ����
            }
        }
    }

    private IEnumerator PunchRoutine()
    {
        // ������ ��׷���
        float halfDuration = punchDuration * 0.5f;
        float elapsed = 0f;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            targetTransform.localScale = Vector3.Lerp(originalScale, originalScale * punchScale, elapsed / halfDuration);
            yield return null;
        }

        // ź�� �ְ� ����
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            targetTransform.localScale = Vector3.Lerp(originalScale * punchScale, originalScale, elapsed / halfDuration);
            yield return null;
        }

        targetTransform.localScale = originalScale;
    }
}