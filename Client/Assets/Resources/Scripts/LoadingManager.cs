using UnityEngine;
using System.Collections;

public class LoadingManager : MonoBehaviour {


    #region 引用
    public GameObject mainSceneGO;
    private GameObject existingModel;
    private GameObject chestModel;
    public GameObject modelCameraPrefab;
    public Transform modelPositionTransform;
    public Transform chestModelPositionTransform;

    public Skybox targetSkyBoxOnModelCamera;

    #endregion


    #region 变量
    private bool isConfigInit = false;
    private bool loadingFinished = false;


    #endregion



    private static LoadingManager _instance;
    public static LoadingManager Instance {
        get 
        {
            return _instance;
        }
    }


    void Awake()
    {
        _instance = this;
        InitConfig();
    }

    void Start()
    {
        DontDestroyOnLoad(modelCameraPrefab);
    }


    void Update()
    {

    }


    public void EnterScene(string sceneName)
    {
        StartCoroutine(WaitLoadingTargetScene(sceneName));
    }



    #region 方法

    /// <summary>
    /// 异步场景加载
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator WaitLoadingTargetScene(string sceneName)
    {
        AsyncOperation async = Application.LoadLevelAsync(sceneName);
        yield return async;

        loadingFinished = true;
    }




    /// <summary>
    /// 游戏配置初始化
    /// </summary>
    private void InitConfig()
    {
        if (isConfigInit)
            return;
        else
            isConfigInit = true;

        StageConfigManager.Init();           //关卡配置
    }



    private void AfterLoadMainScene()
    {
        if (mainSceneGO == null)
        {
            mainSceneGO = GameObject.Find("UIMain");
        }

        if (mainSceneGO != null)
        {
            mainSceneGO.SetActive(true);
        }
    }


    public static GameObject NewUI(string name)
    {
        Object obj = Resources.Load(name);
        if (obj == null)
        {
            Debug.LogError(name + "is not exist");
            return null;
        }

        return (GameObject)Object.Instantiate(obj);
    }


    /// <summary>
    /// 实例化模型
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject NewCharacter(string name)
    {
        Object obj = Resources.Load(GameDefine.CharacterPath + name);
        if (obj == null)
        {
            Debug.LogError(name + "is not exist!");
            return null;
        }

        return (GameObject)Object.Instantiate(obj);
    }



    public GameObject NewUIParticle(string name)
    {
        Object obj = Resources.Load(GameDefine.UIParticlePath + name);
        Debug.LogError("obj " + obj);
        if (obj == null)
        {
            Debug.LogError(name + "is not existed!");
        }
        return (GameObject)Object.Instantiate(obj);
    }

    /// <summary>
    /// 加载模型相机
    /// </summary>
    public void LoadModelCameraPrefab()
    {
        Object go;
        if (modelCameraPrefab == null)
        {
            go = Resources.Load("UIPrefab/ModelCameraPrefab");
            GameObject objCamera = (GameObject)Instantiate(go);
            DontDestroyOnLoad(objCamera);
            modelCameraPrefab = objCamera;

            modelPositionTransform = objCamera.transform.GetChild(1);
//            Debug.LogError("#modelPositionTransform " + modelPositionTransform);
            targetSkyBoxOnModelCamera = objCamera.transform.GetChild(0).GetComponent<Skybox>();
        }
    }

    /// <summary>
    /// 加载模型
    /// </summary>
    /// <param name="name"></param>
    public void LoadModelPrefab(string name)
    {
        existingModel = NewCharacter(name);
        if (existingModel.GetComponent<Animation>().GetClip("Idle") != null)
        {
            existingModel.GetComponent<Animation>().Play("Idle");
        }

        existingModel.transform.parent = modelPositionTransform;
        existingModel.transform.localPosition = Vector3.zero;
        existingModel.transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// 加载宝箱
    /// </summary>
    /// <param name="name"></param>
    public void LoadChestPrefab(string name)
    {
        chestModel = NewCharacter(name);
        if (chestModel.GetComponent<Animation>().GetClip("Move") != null)
        {
            chestModel.GetComponent<Animation>().Play("Move");
        }
        if (chestModel.GetComponent<Animation>().GetClip("Die") != null)
        {
            chestModel.GetComponent<Animation>().PlayQueued("Die");
        }

        chestModel.transform.parent = chestModelPositionTransform;
        chestModel.transform.localPosition = Vector3.zero;
        chestModel.transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// 旋转模型
    /// </summary>
    /// <param name="delta">旋转角度</param>
    public void RotateModel(float delta)
    {
        Vector3 formerLocalRotation = existingModel.transform.localRotation.eulerAngles;
        formerLocalRotation.y -= delta;
        existingModel.transform.localRotation = Quaternion.Euler(formerLocalRotation);
    }


    public void DestroyModel()
    {
        if (existingModel != null)
        {
            Destroy(existingModel);
        }
    }

    #endregion

}
