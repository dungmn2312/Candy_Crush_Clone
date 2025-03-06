//using System;
//using DG.Tweening;
//using Sirenix.OdinInspector;
//using UnityEngine;

//public abstract class BaseBox : MonoBehaviour
//{
//    [SerializeField] protected RectTransform mainPanel;

//    [ShowIf("isPopup", true)] public RectTransform contentPanel;

//    [Header("========= CONFIG BOX ===========")]
//    public bool isNotStack;
//    public bool isPopup;
//    [SerializeField] protected bool isAnim = true;
//    protected Canvas popupCanvas;
//    protected CanvasGroup canvasGroupPanel;
//    [HideInInspector] public bool isBoxSave;a

//    public int IDLayerPopup { get; set; }

//    protected string iDPopup;
//    protected string GetIDPopup()
//    {
//        return iDPopup;
//    }
//    protected virtual void SetIDPopup()
//    {
//        iDPopup = GetType().ToString();
//    }

//    //Call Back
//    public Action OnCloseBox;
//    public Action<int> OnChangeLayer;
//    protected Action actionOpenSaveBox;


//    /// <summary>
//    /// Call back khi đây là Box đang được hiển thị (có layer cao nhất)
//    /// </summary>
//    public Action OnThisIsCurrent;

//    public virtual T SetupBase<T>(bool isSaveBox = false, Action actionOpenBoxSave = null) where T : BaseBox
//    {
//        InitBoxSave(isSaveBox, actionOpenBoxSave);
//        return null;
//    }


//    private void Awake()
//    {
//        popupCanvas = GetComponent<Canvas>();
//        if (popupCanvas != null && isPopup)
//        {
//            popupCanvas.renderMode = RenderMode.ScreenSpaceCamera;
//            popupCanvas.worldCamera = Camera.main;
//            popupCanvas.sortingLayerID = SortingLayer.NameToID("Popup");
//        }

//        if (mainPanel != null)
//        {
//            var tweenAnimation = mainPanel.GetComponent<DOTweenAnimation>();
//            if (tweenAnimation != null)
//            {
//                tweenAnimation.isIndependentUpdate = true;//Không phục thuộc vào time scale
//                isAnim = false;
//            }
//        }

//        OnAwake();
//    }

//    protected virtual void OnAwake()
//    {

//    }

//    public void InitBoxSave(bool isBoxSave, Action actionOpenSaveBox)
//    {
//        this.isBoxSave = isBoxSave;
//        this.actionOpenSaveBox = actionOpenSaveBox;
//    }

//    #region Init Open Handle 
//    protected virtual void OnEnable()
//    {
//        if (!isNotStack)
//        {
//            BoxController.Instance.AddNewBackObj(this);
//        }
//        SetIDPopup();
//        DoAppear();
//        OnStart();
//    }

//    protected virtual void DoAppear()
//    {
//        if (isAnim)
//        {
//            if (mainPanel != null)
//            {
//                mainPanel.localScale = Vector3.zero;
//                mainPanel.DOScale(1, 0.2f).SetUpdate(true).SetEase(Ease.OutBack);
//            }
//        }
//    }

//    protected virtual void OnStart()
//    {

//    }
//    #endregion

//    public virtual void Show()
//    {
//        gameObject.SetActive(true);
//    }

//    /// <summary>
//    /// Đưa popup vào trong Stack save
//    /// </summary>
//    public virtual void SaveBox()
//    {
//        if (isBoxSave)
//            BoxController.Instance.AddBoxSave(GetIDPopup(), actionOpenSaveBox);
//    }

//    /// <summary>
//    /// Chủ động gọi Remove Save Box theo trường hợp cụ thể
//    /// </summary>
//    public virtual void RemoveSaveBox()
//    {
//        BoxController.Instance.RemoveBoxSave(GetIDPopup());
//    }

//    #region Close Box
//    public virtual void Close()
//    {
//        if (!isNotStack)
//            BoxController.Instance.Remove(this);
//        DoClose();
//    }

//    protected virtual void DoClose()
//    {
//        gameObject.SetActive(false);
//    }

//    protected virtual void OnDisable()
//    {
//        if (OnCloseBox != null)
//        {
//            OnCloseBox();
//            OnCloseBox = null;
//        }

//        BoxController.Instance.OnActionOnClosedOneBox();
//    }

//    protected void DestroyBox()
//    {
//        if (OnCloseBox != null)
//            OnCloseBox();
//        Destroy(gameObject);
//    }
//    #endregion

//    #region Change layer Box
//    public void ChangeLayerHandle(int indexInStack)
//    {
//        if (isPopup)
//        {
//            if (popupCanvas != null)
//            {
//                popupCanvas.sortingOrder = indexInStack;
//                popupCanvas.planeDistance = 5;

//                OnChangeLayer?.Invoke(indexInStack);

//                IDLayerPopup = indexInStack;
//            }
//        }
//        else
//        {
//            if (contentPanel != null)
//                transform.SetAsLastSibling();
//        }
//    }
//    #endregion

//    public virtual bool IsActive()
//    {
//        return true;
//    }
//}
