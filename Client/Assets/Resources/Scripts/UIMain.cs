using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIMain : MonoBehaviour {

    #region 引用

    public GamePanel theGamePanel;
    public StagePassedPanel theStagePassedPanel;
    public GameOverPanel theGameOverPanel;

    #endregion


    #region 变量

    public Stack<UIState> uiStateStack = new Stack<UIState>();

    #endregion


    #region 属性
    private static UIMain _Instance;
    public static UIMain Instance
    {
        get
        {
            return _Instance;
        }
    }

    private static UIState recentUIState = UIState.MainState;
    public static UIState RecentUIState {
        get {
            return recentUIState;
        }
        set {
            recentUIState = value;
        }
    }


    #endregion



    void Awake()
    {
        _Instance = this;
        uiStateStack.Push(UIState.MainState);
    }

    void Start() {

    }




    /// <summary>
    /// 获取UI的名称
    /// </summary>
    /// <param name="InputUIStateName"></param>
    /// <returns></returns>
    public string GetUIStateName(UIState InputUIStateName)
    {
        if (InputUIStateName == UIState.MainState)
        {
            return "主界面";
        }

        return "";
    }



    public class UIPanelHelper
    {
        public UIState targetState;
        public string prefabPath;
        public string typeName;

        public UIPanelHelper(UIState inputTargetState, string inputPrefabPath, string inputTypeName)
        {
            targetState = inputTargetState;
            prefabPath = inputPrefabPath;
            typeName = inputTypeName;
        }

    }

    public List<UIPanelHelper> uiPanelList = new List<UIPanelHelper>
    {
        new UIPanelHelper(UIState.GameOverState,"UIPrefab/UI/SmallPanel/GameOverPanel","GameOverPanel"),
        new UIPanelHelper(UIState.GamePanelState,"UIPrefab/UI/SmallPanel/GamePanel","GamePanel")
    };


    /// <summary>
    /// 获取UI脚本实例
    /// </summary>
    /// <param name="inputTargetState">UI状态</param>
    /// <param name="inputComponent">UI脚本组件</param>
    /// <returns></returns>
    public GameObject GetHelp(UIState inputUIState, Component inputComponet)
    {
        if (inputComponet == null)
        {
            for (int i = 0; i < uiPanelList.Count; i++)
            {
                if (uiPanelList[i].targetState == inputUIState)
                {
                    GameObject obj = LoadingManager.NewUI(uiPanelList[i].prefabPath);
                    GameObject targetGo = obj.GetComponent<UIMainLoadedPanel>().targetPanel;

                    targetGo.transform.parent = transform.GetChild(0);
                    targetGo.transform.localPosition = Vector3.zero;
                    targetGo.transform.localScale = Vector3.one;

                    Destroy(obj);
                    return targetGo;
                }
            }
        }

        return inputComponet.gameObject;
    }


    /// <summary>
    /// UI入栈
    /// </summary>
    /// <param name="targetState"></param>
    public void FadeToUIState(UIState targetState)
    {
        if (targetState == UIState.MainState)
            uiStateStack.Clear();
        else if (targetState != RecentUIState)
            uiStateStack.Push(targetState);

        FadeToUIStateWithOutPushToUIStateStack(targetState);
    }


    public void FadeToUIStateWithOutPushToUIStateStack(UIState targetState)
    {
        LeaveUIState(recentUIState);
        recentUIState = targetState;
        EnterUIState(targetState);
    }


    /// <summary>
    /// 进入UI
    /// </summary>
    /// <param name="targetState"></param>
    public void EnterUIState(UIState targetState)
    {
        if (targetState == UIState.MainState)
        {
            int stageNum = PlayerPrefs.GetInt("GameStage");
            theStagePassedPanel.gameObject.SetActive(true);
            theStagePassedPanel.Apply(stageNum);
        }
        else if (targetState == UIState.GamePanelState)
        {
            theGamePanel = GetHelp(targetState,theGamePanel).GetComponent<GamePanel>();
            theGamePanel.gameObject.SetActive(true);
        }
        else if (targetState == UIState.StagePassedPanelState)
        {
            theStagePassedPanel = GetHelp(targetState, theStagePassedPanel).GetComponent<StagePassedPanel>();
            theStagePassedPanel.gameObject.SetActive(true);
        }
        else if (targetState == UIState.GameOverState)
        {
            theGameOverPanel = GetHelp(targetState, theGameOverPanel).GetComponent<GameOverPanel>();
            theGameOverPanel.gameObject.SetActive(true);
        }


    }

    /// <summary>
    /// 退出UI
    /// </summary>
    /// <param name="targetState"></param>
    public void LeaveUIState(UIState targetState)
    {
        if (targetState == UIState.MainState)
        {
            theStagePassedPanel.gameObject.SetActive(false);
        }
        else if (targetState == UIState.GamePanelState)
        {
            theGamePanel.gameObject.SetActive(false);
        }
        else if (targetState == UIState.StagePassedPanelState)
        {
            theStagePassedPanel.gameObject.SetActive(false);
        }
        else if (targetState == UIState.GameOverState)
        {
            theGameOverPanel.gameObject.SetActive(false);
        }


    }



    public void ReturnToLastUIState()
    {
        LeaveUIState(recentUIState);
        //        uiStateStack.Pop();

        if (uiStateStack.Count > 0)
        {
            //栈顶的uistate和recentState一样需要pop掉，不然会进入两个一样的state
            if (recentUIState == uiStateStack.Peek())
            {
                uiStateStack.Pop();
            }

            if (uiStateStack.Count > 0)
            {
                recentUIState = uiStateStack.Pop();
                EnterUIState(recentUIState);

                if (recentUIState == UIState.GamePanelState)
                {
                    UIMain.Instance.theStagePassedPanel.Clear();
                    UIMain.Instance.theStagePassedPanel.Apply(GameData.Instance.GameStage);
                }
                else if (recentUIState == UIState.GameOverState)
                {
                    UIMain.Instance.theGameOverPanel.Apply();
                }

            }
            else
            {
                recentUIState = UIState.MainState;

                EnterUIState(recentUIState);
            }
        }
        else
        {
            recentUIState = UIState.MainState;

            EnterUIState(recentUIState);
        }
    }



}


public enum UIState
{
    GamePanelState,                         //游戏界面
    MainState,
    StagePassedPanelState,
    GameOverState,
}
