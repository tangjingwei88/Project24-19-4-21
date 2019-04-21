using UnityEngine;
using System.Collections;

public class HelperTools : MonoBehaviour {


    /// <summary>
    /// 根据UI路径实例化UI
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject NewUI(string name)
    { 
        Object obj = Resources.Load(GameDefine.UIPrefabPath + name);
        if (obj == null)
        {
            Debug.Log(name + "is not existed!");
            return null;
        }
        return (GameObject)Object.Instantiate(obj);
    }
}
