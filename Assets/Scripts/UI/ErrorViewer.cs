using System.Collections;
using TMPro;
using UnityEngine;

public class ErrorViewer : MonoBehaviour
{
    [SerializeField] private TextMeshPro _textMeshPro;
    [SerializeField] private float _fadeTime;
    [SerializeField] private float _textDownOffset;

    private WaitForSeconds _fadeWait;
    private float _fadeStep;

    private void Awake()
    {
        _fadeWait = new WaitForSeconds(_fadeTime);
        _fadeStep = _textMeshPro.color.a / _fadeTime;
    }

    private void ShowErrorText(string errorText)
    {
        _textMeshPro.text = errorText.ToString();
        StartCoroutine(FadeToZero());
        StartCoroutine(MoveTextDown());
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
        Vector3 tmpPostion = _textMeshPro.transform.position;

        float reductionDistance;
        float remainingDistance = _textDownOffset;

        while (remainingDistance > 0)
        {
            reductionDistance = Mathf.MoveTowards(tmpPostion.y, tmpPostion.y - _textDownOffset, _fadeStep * Time.deltaTime);
            remainingDistance -= reductionDistance;
            tmpPostion.y -= reductionDistance;

            _textMeshPro.transform.position = tmpPostion;

            yield return null;
        }
    }
}
