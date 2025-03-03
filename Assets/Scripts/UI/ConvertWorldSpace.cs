using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertWorldSpace : MonoBehaviour
{
    public static ConvertWorldSpace Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Vector3 _screenPosition;
    private Vector3 _worldPosition;

    public Vector3 GetWorldSpace(Transform _uiElement)
    {
        _screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, _uiElement.position);
        _worldPosition = Camera.main.ScreenToWorldPoint(_screenPosition);

        return _worldPosition;
    }
}