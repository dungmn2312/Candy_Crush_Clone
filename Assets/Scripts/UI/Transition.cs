using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{

    [SerializeField] private static RectTransform rectTransform;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        DontDestroyOnLoad(gameObject.transform.parent);
    }

    private void Start()
    {
        rectTransform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    public static void LoadingInEffect()
    {
        rectTransform.DOScale(new Vector3(25, 25, 0), 1f)
            .SetEase(Ease.InQuad);
    }

    public static void LoadingOutEffect()
    {
        rectTransform.DOScale(new Vector3(0, 0, 0), 1f)
            .SetEase(Ease.InQuad);
    }
}