using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private float moveSpeed = 150f;
    [SerializeField] private float duration = 0.8f;

    public void Setup(string text, Color textColor)
    {
        if (textMesh != null)
        {
            textMesh.text = text;
            textMesh.color = textColor;
        }
        StartCoroutine(AnimateRoutine());
    }

    private IEnumerator AnimateRoutine()
    {
        float elapsed = 0f;
        Vector3 startPos = transform.localPosition;
        // 약간 좌우로 랜덤하게 퍼지며 위로 상승
        Vector3 randomOffset = new Vector3(Random.Range(-30f, 30f), 0, 0);
        transform.localPosition += randomOffset;

        Color initialColor = textMesh != null ? textMesh.color : Color.white;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // 위로 이동
            transform.localPosition += Vector3.up * (moveSpeed * Time.deltaTime);

            // 서서히 페이드 아웃
            if (textMesh != null)
            {
                textMesh.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1f - t);
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}