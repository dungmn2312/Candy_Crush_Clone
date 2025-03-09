using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotate : MonoBehaviour
{

    [SerializeField] private RectTransform rectTransform;
    
    void Start()
    {
        rectTransform.DORotate(new Vector3(0, 0, 360), 5f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }
}