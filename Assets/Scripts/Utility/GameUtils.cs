using System;
using Frictionless;
using UniRx;
using UnityEngine;

public class GameUtils
{
    //  PlayerPrefs
    public static void RaiseMessage(object msg) {
        var router = ServiceFactory.Instance.Resolve<MessageRouter>();
        router.RaiseMessage(msg);
    }

    public static void AddHandler<T>(Action<T> handler)
    {
        var router = ServiceFactory.Instance.Resolve<MessageRouter>();
        router.AddHandler(handler);
    }

    public static void RemoveHandler<T>(Action<T> handler)
    {
        var router = ServiceFactory.Instance.Resolve<MessageRouter>();
        router.RemoveHandler(handler);
    }

    public static IObservable<T> AsObservable<T>() {
        return Observable.FromEvent<T>(AddHandler, RemoveHandler);
    }

    public class SceneName
    {
        public const string HOME_SCENE = "MainHome";
    }

    public static int Difficult
    {
        get
        {
            return PlayerPrefs.GetInt("Current_Difficult");
        }
        set
        {
            PlayerPrefs.SetInt("Current_Difficult", value);
        }
    }

    public static Canvas GetParentCanvas(Transform trans)
    {
        var ParentObj = trans.parent;
        while (ParentObj != null)
        {
            var pCanvas = ParentObj.gameObject.GetComponent<Canvas>();
            if (pCanvas != null)
            {
                return pCanvas;
            }
            ParentObj = ParentObj.parent;
        }

        return null;
    }
}