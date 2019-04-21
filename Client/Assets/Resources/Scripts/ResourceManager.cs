using UnityEngine;
using System.Collections;

public class ResourceManager : MonoBehaviour {

    private static ResourceManager _instance;

    public static ResourceManager Instance {
        get { 
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }


}
