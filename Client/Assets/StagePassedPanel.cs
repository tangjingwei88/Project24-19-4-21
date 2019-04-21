using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePassedPanel : MonoBehaviour {

    #region 引用
    public GameObject ItemListWidget;
    public GameObject StageInfoTemplate;
    public GameObject ScrollViewRect;
    public List<GameObject> itemList = new List<GameObject>();

    #endregion

    #region 变量
    private bool isConfigInit = false;
    private bool isItemListMove = false;
    private int itemListMove;
    #endregion

    void Awake()
    {
        InitConfig();
        Clear();
        int stageNum = PlayerPrefs.GetInt("GameStage");
        if (stageNum == 0)
        {
            stageNum = 1;
            PlayerPrefs.SetInt("GameStage",stageNum);
        }
        Apply(stageNum);

    }

    public void Apply(int stageNum)
    {
        Clear();
 //       ScrollViewRect.transform.localPosition = new Vector3(0, 0, 0);
 //       ScrollViewRect.GetComponent<UIPanel>().clipOffset = new Vector3(0, 0, 0);
        for (int i = 1; i <= stageNum; i++)
        {
            StageConfigManager.StageConfig cfg = StageConfigManager.GetStageConfig(i);
            if (cfg != null)
            {
                GameObject go = Instantiate(StageInfoTemplate);
                go.SetActive(true);
                go.transform.parent = ItemListWidget.transform;
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;

                StageInfoTemplate st = go.GetComponent<StageInfoTemplate>();
                st.Apply(cfg.ID,cfg.Icon);

                ItemListWidget.GetComponent<UIGrid>().repositionNow = true;
                itemList.Add(go);
            }
            else {
                return;
            }
        }
        int low = stageNum / 4;    //每行显示4列
        if (low >= 8 )
        {

            int offsetLow = low - 7;   //满屏显示8行
            offsetLow += 1;
            Debug.LogError("offset" + offsetLow);
            //itemListMove = offsetLow * 200;
            //ItemListWidget.transform.localPosition += new Vector3(0, itemListMove, 0);
            isItemListMove = true;
            ScrollViewRect.transform.localPosition = new Vector3(0, itemListMove, 0);
            ScrollViewRect.GetComponent<UIPanel>().clipOffset = new Vector3(0, -itemListMove, 0);

        }

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


    public void Clear()
    {
        if (itemList != null)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                Destroy(itemList[i]);
            }
        }
    }


}
