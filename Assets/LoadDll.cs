
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

public class LoadDll : MonoBehaviour
{
    void Start()
    {
        // Editor环境下，HotUpdate.dll.bytes已经被自动加载，不需要加载，重复加载反而会出问题。
#if !UNITY_EDITOR
        Assembly hotUpdateAss = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/HotUpdate.dll.bytes"));
#else
        // Editor下无需加载，直接查找获得HotUpdate程序集
        Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
#endif
        
        Type type = hotUpdateAss.GetType("Hello");
        type.GetMethod("Run").Invoke(null, null);
        
        // 加载AB包，创建预制体
        // AssetBundle ab1 = AssetBundle.LoadFromFile("http://127.0.0.1:80" + "/AssetBundles/ab1");
        // GameObject cube = ab1.LoadAsset<GameObject>("GameObj");
        // Instantiate(cube);
        
        StartCoroutine(load());
    }

    IEnumerator load()
    {
        string urlPath = @"http://127.0.0.1:80" + "/AssetBundles/ab1";
        UnityWebRequest request = UnityWebRequest.Get(urlPath);
        yield return request.SendWebRequest();

        byte[] results = request.downloadHandler.data;
        AssetBundle ab1 = AssetBundle.LoadFromMemory(results);
        GameObject cube = ab1.LoadAsset<GameObject>("GameObj");
        Instantiate(cube);
    }

}

