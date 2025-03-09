using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

/***
 * HoangTV 2/2020
 * Box = Popup + Panel
 * Bật các Popup nối tiếp => các Popup có thể hiện đè lên nhau
 * Từ Popup bật lên Panel => Popup phải đóng trước mới được mở Panel (vì Panel luôn luôn có layer nhỏ hơn Popup => không thể sắp xếp đè lên nhau)
 ***/
public class BoxController : SingletonClass<BoxController>, IService
{
    public const int BASE_INDEX_LAYER = 20;


    private Stack<BaseBox> boxStack = new Stack<BaseBox>();
    private readonly Subject<Unit> _pressEscapeSubject = new Subject<Unit>();
    public IObservable<Unit> PressEscapeObservable => _pressEscapeSubject;

    /// <summary>
    /// List chứa các Box thuộc dạng bắt buộc phải xem (Lưu lại ID Popup và hàm để mở Box)
    /// Box save Hiện đi hiện lại đến khi nào user ấn đóng thì mới remove ra khỏi List
    /// </summary>
    private Dictionary<string, Action> lstActionSaveBox = new Dictionary<string, Action>();

    public bool isLoadingShow;
    public bool m_isLockEscape;
    public bool isLockEscape
    {
        get
        {
            return m_isLockEscape;
        }
        set
        {
            // Debug.Log(" ===================== " + value);
            m_isLockEscape = value;
        }

    }

    public bool m_isLockOff;
    public bool isLockOff
    {
        get
        {
            return m_isLockOff;
        }
        set
        {
            // Debug.Log(" ===================== " + value);
            m_isLockOff = value;
        }

    }

    public event Action ActionStackEmpty;
    public event Action ActionOnClosedOneBox;

    public void Init()
    {
        Observable.EveryUpdate()
                  .Subscribe(_ =>
                  {
                      if (Input.GetKeyUp(KeyCode.Escape))
                      {
                          ProcessBackBtn();
                      }

                      //if (Input.GetKeyDown(KeyCode.Space))
                      //{
                      //    DebugStack();
                      //}
                  });

        SceneManager.sceneLoaded += (scene, mode) => OnLevelWasLoaded();
    }

    //public void GetBoxAddressableAsync<T>(T instance, Action<T> OnLoadedFirst, Action<T> OnLoaded, string resourcePath, bool autoHideWaiting = true)
    //where T : BaseBox
    //{
    //    if (instance == null)
    //    {
    //        Context.Waiting.ShowWaiting();
    //        AddressableController.Instance.LoadAssetByName<GameObject>(resourcePath, (_) =>
    //        {
    //            instance = Object.Instantiate(_).GetComponent<T>();
    //            OnLoadedFirst?.Invoke(instance);
    //            OnLoaded?.Invoke(instance);
    //            if (autoHideWaiting)
    //                Context.Waiting.HideWaiting();
    //        });
    //    }
    //    else
    //    {
    //        OnLoaded?.Invoke(instance);
    //    }
    //}

    //public void GetBoxAsync<T>(T instance, Action<T> OnLoadedFirst, Action<T> OnLoaded, string resourcePath)
    //    where T : BaseBox
    //{
    //    if (instance == null)
    //    {
    //        Context.Waiting.ShowWaiting();
    //        Resources.LoadAsync<T>(resourcePath)
    //                 .AsAsyncOperationObservable()
    //                 .Subscribe(resourceRequest =>
    //                 {
    //                     instance = Object.Instantiate(resourceRequest.asset) as T;
    //                     OnLoadedFirst?.Invoke(instance);
    //                     OnLoaded?.Invoke(instance);
    //                     Context.Waiting.HideWaiting();
    //                 });
    //    }
    //    else
    //    {
    //        OnLoaded?.Invoke(instance);
    //    }
    //}

    public IObservable<T> GetBoxAsync<T>(T instance, string resourcePath) where T : BaseBox
    {
        return instance != null
            ? Observable.Return(instance)
            : Resources.LoadAsync<T>(resourcePath)
                       .AsAsyncOperationObservable()
                       .Select(resourceRequest => instance = Object.Instantiate(resourceRequest.asset) as T);
    }

    public void AddNewBackObj(BaseBox obj)
    {
        boxStack.Push(obj);
        SettingOderLayerPopup();
    }

    private void SettingOderLayerPopup()
    {
        if (boxStack == null || boxStack.Count <= 0)
            return;

        int index = BASE_INDEX_LAYER + 10 * boxStack.Count;
        foreach (var box in boxStack)
        {
            box.ChangeLayerHandle(BASE_INDEX_LAYER + index);
            index -= 10;
        }
    }

    public void Remove(BaseBox box)
    {
        if (boxStack.Count == 0)
            return;

        if (!boxStack.Peek().Equals(box)) // check if box is at the top
        {
            if (!boxStack.Contains(box)) // box doesn't exist in stack
                return;

            RemoveMiddle(box);
        }
        else
            boxStack.Pop();

        if (boxStack.Count == 0)
            OnStackEmpty();

        SettingOderLayerPopup();
    }

    private void RemoveMiddle(BaseBox obj)
    {
        Stack<BaseBox> tmpStack = new Stack<BaseBox>();
        while (boxStack.Count > 0)
        {
            BaseBox topObj = boxStack.Pop();
            if (ReferenceEquals(topObj, obj))
                break;

            tmpStack.Push(topObj);
        }

        while (tmpStack.Count > 0)
        {
            boxStack.Push(tmpStack.Pop());
        }
    }

    /// <summary>
    /// Đang có Popup hiện
    /// </summary>
    /// <returns></returns>
    public bool IsShowingPopup()
    {
        BaseBox[] lst_backObjs = boxStack.ToArray();
        int lenght = lst_backObjs.Length;
        for (int i = lenght - 1; i >= 0; i--)
        {
            if (lst_backObjs[i].isPopup)
            {
                return true;
            }
        }

        return false;
    }

    public void DebugStack()
    {
        BaseBox[] lst_backObjs = boxStack.ToArray();
        int lenght = lst_backObjs.Length;
        for (int i = lenght - 1; i >= 0; i--)
        {
            Debug.Log(" =============== " + lst_backObjs[i].gameObject.name + " ===============");
        }
    }

    private void OnStackEmpty()
    {
        ActionStackEmpty?.Invoke();
    }

    public virtual void OnActionOnClosedOneBox()
    {
        ActionOnClosedOneBox?.Invoke();
    }

    public virtual void ProcessBackBtn()
    {
        if (isLoadingShow)
            return;

        if (isLockEscape)
            return;

        if (isLockOff)
            return;

        if (boxStack.Count != 0)
        {
            boxStack.Peek().Close();
        }
        else
        {
            if (!OpenBoxSave())
                OnPressEscapeStackEmpty();
        }
    }

    protected virtual void OnPressEscapeStackEmpty()
    {
        /*
        if(currentScene == null)
           currentScene = GameObject.FindObjectOfType<BaseScene>();

        if (currentScene != null)
            currentScene.OnEscapeWhenStackBoxEmpty();
            */
        _pressEscapeSubject.OnNext(Unit.Default);
    }

    protected void OnLevelWasLoaded()
    {
        if (boxStack != null)
            boxStack.Clear();

        OpenBoxSave();
    }

    /// <summary>
    /// Check không có Popup hay panel nào được bật
    /// </summary>
    /// <returns></returns>
    public bool IsEmptyStackBox()
    {
        return boxStack.Count == 0;
    }

    #region Box Save
    private bool OpenBoxSave()
    {
        bool isOpened = false;
        foreach (var save in lstActionSaveBox)
        {
            if (save.Value != null)
            {
                isOpened = true;
                save.Value();
            }
        }

        return isOpened;
    }

    public void AddBoxSave(string idPopup, Action actionOpen)
    {
        if (lstActionSaveBox.ContainsKey(idPopup))
            lstActionSaveBox.Add(idPopup, actionOpen);
    }

    public void RemoveBoxSave(string idPopup)
    {
        if (lstActionSaveBox.ContainsKey(idPopup))
            lstActionSaveBox.Remove(idPopup);
    }
    #endregion


    public bool IsPupopCurrent(BaseBox baseBox)
    {
        BaseBox boxShowing = boxStack.Count > 0 ? boxStack.Peek() : null;
        return boxShowing == null || baseBox == null ? false : boxShowing == baseBox;
    }

    public void CloseAll()
    {
        BaseBox[] lst_backObjs = boxStack.ToArray();
        int lenght = lst_backObjs.Length;
        for (int i = lenght - 1; i >= 0; i--)
        {
            if (lst_backObjs[i])
            {
                lst_backObjs[i].Close();
            }
        }
    }
}
