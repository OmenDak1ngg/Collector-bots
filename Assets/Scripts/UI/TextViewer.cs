using System.Collections;
using TMPro;
using UnityEngine;

public class TextViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private float _fadeTime;
    [SerializeField] private float _textDownOffset;

    private float _fadeStep;
    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.localPosition;
        _fadeStep = _textMeshPro.color.a / _fadeTime;
    }

    private IEnumerator FadeToZero()
    {
        Color tmpColor = _textMeshPro.color;
        float tmpAlpha = _textMeshPro.color.a;

        while (tmpAlpha != 0)
        {
            tmpAlpha = Mathf.MoveTowards(tmpAlpha, 0, _fadeStep * Time.deltaTime);
            tmpColor.a = tmpAlpha;

            _textMeshPro.color = tmpColor;

            yield return null;
        }
    }

    private IEnumerator MoveTextDown()
    {
        Vector3 tmpPostion = _textMeshPro.transform.localPosition;
        float targetPosition = tmpPostion.y - _textDownOffset;
        float downStep = _textDownOffset / _fadeStep;

        while (tmpPostion.y != targetPosition)
        {
            tmpPostion.y = Mathf.MoveTowards(tmpPostion.y, tmpPostion.y - _textDownOffset, downStep * Time.deltaTime);

            _textMeshPro.transform.localPosition = tmpPostion;

            yield return null;
        }
    }

    public void ShowText(string text)
    {
        StopAllCoroutines();
        transform.localPosition = _startPosition;
        Color tmpColor = _textMeshPro.color;
        tmpColor.a = 1f;
        _textMeshPro.color = tmpColor;

        _textMeshPro.text = text.ToString();
        StartCoroutine(FadeToZero());
        StartCoroutine(MoveTextDown());
    }
}
