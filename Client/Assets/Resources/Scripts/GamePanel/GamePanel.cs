/***
 * 
 * Title:游戏主界面
 * Author：TSoft
 * Date：18-7-11-1310
 * 
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GamePanel : MonoBehaviour {

    #region 引用

    public ShowLogPart theShowLogPart;
    public ShowResultPart theShowResultPart;
    public GameOverPanel theGameOverPanel;

    public GameObject DragInputItemTemplate;
    public GameObject dragInputItemWidget;
    public GameObject DragItemTemplate;
    public GameObject selectItemPoolRoot;
    public GameObject selectPoolWidget;
    public UIScrollView theScrollView;

    public AudioClip dragMusic;            //拖拽音效
    public AudioClip erroMusic;
    public AudioSource audioSource;

    public UILabel TimeLabel;             //倒计时显示
    public UILabel ResultLabel;           //测试用显示结果          
    public UILabel StageNameLabel;
    public GameObject Lock;
    public UILabel LockNumLabel;
    

    #endregion


    #region 变量
    private int resultNum;              //正确结果
    private int round = 1;              //游戏轮数
    private string gameInput = "";      //玩家输入数据
    public int timer = 0;               //计时器

    private Dictionary<int, int> inputDic = new Dictionary<int, int>();                     //玩家输入的数据
    private Dictionary<int, int> randomDic = new Dictionary<int, int>();                    //系统随机生成的数据
    private Dictionary<LIGHT_TYPE, int> resultDic = new Dictionary<LIGHT_TYPE, int>();      //游戏结果数据
    private Dictionary<int, int> selectPoolDic = new Dictionary<int, int>();                //供选择的数据池
//    private Dictionary<int, string> numIconPoolDic = new Dictionary<int, string>();         //num:Icon数据池
    Dictionary<int, int> inputTempDic = new Dictionary<int, int>();                         //玩家输入待检测的数据

    List<GameObject> inputList = new List<GameObject>();
    List<GameObject> dragItemList = new List<GameObject>();
    #endregion

    private static GamePanel _instance;
    public static GamePanel Instance {
        get {
            return _instance;
        }
    }


    void Awake()
    {
        _instance = this;
    }


    /// <summary>
    /// 游戏初始化
    /// </summary>
    public void Apply(int gameStage)
    {
        StopAllCoroutines();
        GameData.Instance.win = false;
        ShowStage(gameStage);
        GameData.Instance.GameStage = gameStage;
        //获取可选择数据池
        StageConfigManager.StageConfig stageConfig = StageConfigManager.GetStageConfig(gameStage);
        if (stageConfig != null)
        {
            Dictionary<int, string> numIconPoolDic = new Dictionary<int, string>(stageConfig.numIconPoolDic);
            GameData.Instance.gameLv = stageConfig.Level;
            GameData.Instance.showColumn = stageConfig.Column;
            GameData.Instance.resultColumn = stageConfig.ResultColumn;
            GameData.Instance.lockNum = stageConfig.Lock;

            dragInputItemWidget.transform.localScale = new Vector3 (stageConfig.InputItemScale,stageConfig.InputItemScale,1);
            selectPoolWidget.transform.localScale = new Vector3(stageConfig.SelectItemScale,stageConfig.SelectItemScale,1);
            theShowResultPart.transform.localScale = new Vector3(stageConfig.LightItemScale, stageConfig.LightItemScale, 1);

            Lock.SetActive(true);
            LockNumLabel.gameObject.SetActive(true);
            LockNumLabel.text = stageConfig.Lock.ToString();
            StageNameLabel.text = stageConfig.Name;
            //刷新界面
            RefreshPanel(numIconPoolDic);
            //获取系统生成的随机数据
            randomDic = GetRandomNumber(gameStage);
            selectPoolWidget.gameObject.SetActive(false);
            selectPoolWidget.gameObject.SetActive(true);
            //显示输入框(老版)
            ShowInputNumRoot(stageConfig.Level);

            //显示倒计时
            StartCoroutine(TimeSliping(stageConfig.TimeLong));
            //加载模型
            LoadingManager.Instance.LoadModelCameraPrefab();
            LoadingManager.Instance.LoadModelPrefab("DarkHunter_B");
        }
        else {
            return;
        }
        dragInputItemWidget.transform.GetComponent<UIGrid>().Reposition();
        dragInputItemWidget.transform.GetComponent<UIGrid>().repositionNow = true;
    }


    #region 方法

    public void ShowStage(int stageId)
    {
        int stageSum = 5;
        for (int i = 1; i <= stageSum;i++)
        {
            if (i == stageId)
            {
                transform.Find("StagePanel/stage_" + i).gameObject.SetActive(true);
            }else{
                transform.Find("StagePanel/stage_" + i).gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 刷新界面，重新开始游戏
    /// </summary>
    public void RefreshPanel(Dictionary<int,string> numIconDic)
    {
        this.Clear();
        theShowLogPart.Clear();
        theShowResultPart.Clear();
        LoadingManager.Instance.DestroyModel();
        ShowSelectPoolRoot(numIconDic);
    }

    /// <summary>
    /// 倒计时显示
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator TimeSliping(float time)
    {
        while (time >= 0)
        {
            yield return new WaitForSeconds(1);
            TimeLabel.text = time.ToString();
            time--;
            timer++;
            if (time < 40)
            {
                //TimeLabel.gameObject.transform.localScale = new Vector3(3.0f,3.0f,3.0f);
                //TimeLabel.gameObject.transform.GetComponent<Animator>().Play();
            }
        }

        //TODO
        //游戏结束
        Debug.LogError("游戏结束");
        theShowLogPart.Clear();
//        GamePanel.Instance.timer = 0;
        theGameOverPanel.gameObject.SetActive(true);
        theGameOverPanel.Apply();
    }

    public void StopTimer()
    {
        StopCoroutine("TimeSliping");
    }

    /// <summary>
    /// 确定按钮
    /// </summary>
    public void OnOkBtnClick()
   {
        //获得用户输入的数据
        inputDic = GetInputNumber(GameData.Instance.gameLv);
        //比较用户输入的数据和系统生成的数据
        if (inputDic.Count < GameData.Instance.gameLv)
        {
            TipsManager.Instance.ShowTips("数据不完整哦！");
            NGUITools.PlaySound(erroMusic, 0.1f);
            return;
        }
        else
        {
            resultDic = CompareInputAndRandomNum(inputDic, randomDic);
        }
        //刷新游戏结果
        RefreshLighterShow(resultDic);
       //刷新游戏记录
       RefreshLoggerShow(resultDic);
    }


    public bool CheckInputNotEmpty(Dictionary<int,int> dic)
    {
        for (int i = 1; i <= dic.Count; i++)
        {
            if (dic[i].Equals("0"))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 刷新红黄绿的显示
    /// </summary>
    /// <param name="dic"></param>
    private void RefreshLighterShow(Dictionary<LIGHT_TYPE, int> dic)
    {
        theShowResultPart.Apply(dic);
    }


    /// <summary>
    /// 刷新玩家游戏记录
    /// </summary>
    /// <param name="dic"></param>
    private void RefreshLoggerShow(Dictionary<LIGHT_TYPE, int> dic)
    {
        if(!GameData.Instance.win)
        theShowLogPart.Apply(dic);
    }


    /// <summary>
    /// 比较玩家输入数据和系统生成的随机数
    /// </summary>
    /// <param name="inputDic"></param>
    /// <param name="randomDic"></param>
    /// <returns></returns>
    private Dictionary<LIGHT_TYPE, int> CompareInputAndRandomNum(Dictionary<int,int> inputDic,Dictionary<int,int> randomDic)
    {
        int green = 0;
        int yellow = 0;
        int red = 0;

        Dictionary<LIGHT_TYPE, int> resultDic = new Dictionary<LIGHT_TYPE, int>();
        foreach (var idic in inputDic)
        {
            foreach (var randDic in randomDic)                                  
            {
                if (idic.Value == randDic.Value)                                //数字相同
                {
                    
                    if (idic.Key == randDic.Key)                                //数字顺序相同
                    {
                        green++;                                                //数字且顺序相同的个数
                    }
                    else {
                        yellow++;                                               //数字相同但顺序不同的个数
                    }
                    
                }
            }
        }
        red = inputDic.Count - green - yellow;                                  //数字不同且顺序不同的个数

        resultDic.Add(LIGHT_TYPE.green, green);
        resultDic.Add(LIGHT_TYPE.yellow, yellow);
        resultDic.Add(LIGHT_TYPE.red, red);

        return resultDic;
    }



    /// <summary>
    /// 显示输入框
    /// </summary>
    /// <param name="gameLv"></param>
    private void ShowInputNumRoot(int gameLv)
    {
        StageConfigManager.StageConfig stageConfig = StageConfigManager.GetStageConfig(GameData.Instance.GameStage);
        Dictionary<int, string> numIconPoolDic = stageConfig.numIconPoolDic;
        for (int i = 1; i <= gameLv; i++)
        {
            GameObject go = Instantiate(DragInputItemTemplate);
            go.name = "DragInputItem_" + i;
            go.SetActive(true);
            go.transform.parent = dragInputItemWidget.transform;
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;

            inputList.Add(go);
        }
        dragInputItemWidget.transform.GetComponent<UIGrid>().maxPerLine = GameData.Instance.resultColumn;
        dragInputItemWidget.transform.GetComponent<UIGrid>().Reposition();
        dragInputItemWidget.transform.GetComponent<UIGrid>().repositionNow = true;
    }



    /// <summary>
    /// 显示供选择的数据池
    /// </summary>
    /// <param name="selectPoolDic"></param>
    private void ShowSelectPoolRoot(Dictionary<int,string> numIconDic)
    {
        GameObject stageObj = transform.Find("StagePanel/stage_" + GameData.Instance.GameStage + "/DragItemsPanel").gameObject;
        int num = stageObj.transform.childCount;
        for (int i = 1; i <= num;i++)
        {
            DragItem sc = stageObj.transform.Find("DragItem_" + i).GetComponent<DragItem>();
            sc.Apply(numIconDic[i]);
        }


        /*   for (int i = 1; i <= numIconDic.Count; i++)
           {
               GameObject go = Instantiate(DragItemTemplate);
               go.name = "DragItem_" + i;
               go.SetActive(true);
               go.transform.parent = selectPoolWidget.transform;
               go.transform.localScale = Vector3.one;
             //  go.transform.localPosition = Vector3.zero + new Vector3(0,200,0);

               DragItem sc = go.GetComponent<DragItem>();
               sc.Apply(numIconDic[i]);

               dragItemList.Add(go);
           }
           //设置grid的显示列数
           selectPoolWidget.transform.GetComponent<UIGrid>().maxPerLine = GameData.Instance.showColumn;
           selectPoolWidget.transform.GetComponent<UIGrid>().Reposition();
           selectPoolWidget.transform.GetComponent<UIGrid>().repositionNow = true;*/
    }



    /// <summary>
    /// 获取玩家输入的数据
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, int> GetInputNumber(int gameLv)
    {
        Dictionary<int, int> reDic = new Dictionary<int, int>();
        Dictionary<int, int> dic = new Dictionary<int, int>();

        //获得用户输入的数据
        dic = GetTempInputNumber(gameLv);

        //检测输入数据是否合法
        if (CheckInputNumLegal(dic))
        {
            reDic = dic;
            GameData.Instance.gameInput = gameInput;
        }
        return reDic;
    }



    /// <summary>
    /// 读取用户输入的临时数据
    /// </summary>
    /// <param name="gameLv"></param>
    /// <returns></returns>
    public Dictionary<int, int> GetTempInputNumber(int gameLv)
    {
        string inputStr = "";
        Dictionary<int, int> dic = new Dictionary<int, int>();

        for (int i = 1; i <= gameLv; i++)
        {
            GameObject obj = transform.Find("CenterPart/DragInputWidget/DragInputItem_" + i + "/Label").gameObject;
            string inputValue = obj.GetComponent<UILabel>().text;
            inputStr += inputValue;
            //输入为“0”说明是绿“+”状态，不加入字典
            if (!inputValue.Equals("0"))
            {
                dic[i] = Convert.ToInt32(inputValue);
            }
        }
        gameInput = inputStr;
        GameData.Instance.curResultItemDic = new Dictionary<int, int>(dic);
        return dic;
    }



    /// <summary>
    /// 判断玩家输入的数据是否合法
    /// </summary>
    /// <param name="inputDic"></param>
    /// <returns></returns>
    public bool CheckInputNumLegal(Dictionary<int, int> inputDic) 
    {
        StageConfigManager.StageConfig cf = StageConfigManager.GetStageConfig(GameData.Instance.GameStage);
        //需要使用值拷贝而不是引用拷贝（坑！！要注意）
        Dictionary<int, int> checkDic = new Dictionary<int, int>(cf.NumPoolDic);

        foreach (var item in inputDic)
        {
            if (checkDic.ContainsValue(item.Value))                     
            {
                checkDic.Remove(item.Value);
            }
            else {
                NGUITools.PlaySound(erroMusic, 0.1f);
                TipsManager.Instance.ShowTips("有重复数字哦！");
                return false;
            }
        }
        return true;
    }


    /// <summary>
    /// 系统随机生成数据
    /// </summary>
    /// <param name="number">生成的随机数位数</param>
    /// <returns></returns>
    private Dictionary<int, int> GetRandomNumber(int gameStage)
    {
        int randNum;
        StageConfigManager.StageConfig stageConfig = StageConfigManager.GetStageConfig(gameStage);
        Dictionary<int, int> dic = stageConfig.NumPoolDic;
        randomDic = GetRandomNum(stageConfig.Level, dic);
        return randomDic;
    }



    /// <summary>
    /// 从numPool中的数字字典中随机生成不重复的数字
    /// </summary>
    /// <param name="num">生成几位数</param>
    /// <returns></returns>
    private Dictionary<int, int> GetRandomNum(int num,Dictionary<int,int> numArrayDic)
    {
        int randNum;
        int key = 1;
        int temp = num;
        string randNumStr = "";
        Dictionary<int, int> randDic = new Dictionary<int, int>();

        ///*****注意！！****：
        ///C#中Dictionary的“=”赋值是引用拷贝（拷贝的是地址），不是值拷贝！！坑！！
        ///值拷贝用如下方式
        Dictionary<int, int> tempDic = new Dictionary<int, int>(numArrayDic);

        List<int> tempList = DicToList(tempDic);

        System.Random rand = new System.Random();
        while (temp > 0)
        {
            //随机生成的数字
            randNum = rand.Next(1, tempDic.Count);
            //数字库中是否还存在这个key，即用来判断是否有重复的数字
            if (tempDic.ContainsValue(tempList[randNum-1]))
            {
                //将随机生成的1-9的数字保存到输出字典中
                randDic.Add(key++, tempList[randNum - 1]);
//                Debug.LogError(tempList[randNum - 1]);

                //显示随机生成的测试数据
                randNumStr += tempList[randNum - 1];
                randNumStr += ",";

                //移除数字库字典中已经使用过的数字
                tempDic.Remove(tempList[randNum - 1]);
                temp--;

                tempList = DicToList(tempDic);
            }
            
        }

        ResultLabel.text = randNumStr;
        return randDic;
    }

    public List<int> DicToList(Dictionary<int,int> dic)
    {
        List<int> list = new List<int>();
        foreach (var item in dic)
        {
            list.Add(item.Value);
        }
        return list;
    }

    /// <summary>
    /// 控制正确答案显示
    /// </summary>
    public void OnClickShowResultBtn()
    {
        if (ResultLabel.gameObject.activeSelf)
        {
            ResultLabel.gameObject.SetActive(false);
        }
        else {
            ResultLabel.gameObject.SetActive(true);
        }
    }

    public void OnShowLogBtnClick()
    {
        if (theShowLogPart.gameObject.activeSelf)
        {
            theShowLogPart.gameObject.SetActive(false);
        }
        else {
            theShowLogPart.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 上一关卡
    /// </summary>
    public void OnBeforeStageBtn()
    {
        GameData.Instance.GameStage -= 1;
        PlayerPrefs.SetInt("GameStage",GameData.Instance.GameStage);
        StopAllCoroutines();
        GamePanel.Instance.Apply(GameData.Instance.GameStage);
    }


    /// <summary>
    /// 下一关卡
    /// </summary>
    public void OnNextStageBtn()
    {
        GameData.Instance.GameStage += 1;
        PlayerPrefs.SetInt("GameStage", GameData.Instance.GameStage);
        StopAllCoroutines();
        GamePanel.Instance.Apply(GameData.Instance.GameStage);
    }

    /// <summary>
    /// 刷新被拖拽item的状态（灰显）
    /// </summary>
    public void RefreshDragItemState()
    {
        Debug.LogError("###RefreshDragItemState");
        for (int i = 0; i < dragItemList.Count;i++)
        {
            GameObject dragGo = dragItemList[i];
            if (dragGo != null)
            {
                if (dragGo.GetComponent<DragItem>().isDrag)
                {
                    dragGo.SetActive(false);
                }
                else
                {
                    dragGo.SetActive(true);
                }
            }

        }

    }



    /// <summary>
    /// 返回到选关界面
    /// </summary>
    public void BackToStagePanel()
    {
      //  int stageNum = PlayerPrefs.GetInt("GameStage");

        UIMain.Instance.ReturnToLastUIState();
    }


    public void Clear()
    {
        if (inputList!= null)
        {
            for (int i = 0; i < inputList.Count; i++)
            {
                Destroy(inputList[i]);
            }
        }

        if (dragItemList != null)
        {
            for (int i = 0; i < dragItemList.Count; i++)
            {
                Destroy(dragItemList[i]);
                
            }
        }
    }
    #endregion
}
