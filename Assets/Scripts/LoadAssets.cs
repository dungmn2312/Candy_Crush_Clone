//using Cysharp.Threading.Tasks;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AddressableAssets;
//using UnityEngine.ResourceManagement.AsyncOperations;

//public class LoadAssets : MonoBehaviour
//{
//    private void Awake()
//    {
//        AddressableController.Instance.Init();
//        StartCoroutine(WaitForInit());
//    }

//    private IEnumerator WaitForInit()
//    {
//        while (AddressableController.Instance.isInited == false)
//        {
//            //Debug.Log("");
//            yield return null;
//        }
//        TestLoadAssets();
//    }

//    private void TestLoadAssets()
//    {
//        GameObject obj = AddressableController.Instance.LoadAssetByName<GameObject>("Prefabs/Earn Star");

//    }
//}