using TMPro;
using UnityEngine;

public class ResourceViewer : MonoBehaviour
{
    [SerializeField] private Storage _storage;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _storage.ResourceUpdated += UpdateResourceCount;
    }

    private void OnDisable()
    {
        _storage.ResourceUpdated -= UpdateResourceCount;
    }

    private void Start()
    {
        UpdateResourceCount(0);
    }

    private void UpdateResourceCount(int count)
    {
        _text.text = count.ToString();
    }
}
