using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogShowItemTemplate : MonoBehaviour {

    #region 引用
    public UILabel resultLabel;               //玩家输入的数据
    public UILabel roundLabel;                 //第几轮

    public GameObject lightWidget;
    public GameObject resultItemWidget;
    public GameObject LightItemTemplate;
    public GameObject DragInputItemTemplate;



    #endregion

    #region 变量
    public List<GameObject> lightItemList = new List<GameObject>();
    #endregion

    public void Apply(Dictionary<LIGHT_TYPE,int> dic)
    {
        //resultLabel.text = GameData.Instance.gameInput;
        roundLabel.text = "R" + GameData.Instance.gameRound.ToString();
        //显示提示
        foreach (var item in dic)
        {
            for (int i = 0; i < item.Value; i++)
            {
                GameObject go = Instantiate(LightItemTemplate);
                go.SetActive(true);
                go.transform.parent = lightWidget.transform;
                
                lightItemList.Add(go);

                LightItemTemplate sc = go.GetComponent<LightItemTemplate>();
                sc.Apply(item);
                lightWidget.GetComponent<UIGrid>().repositionNow = true;
                lightWidget.GetComponent<UIGrid>().maxPerLine = GameData.Instance.resultColumn;
                go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                go.GetComponent<TweenScale>().to = new Vector3(0.2f,0.2f,0.2f); ;
                
            }
        }
        ShowCurResultItem();
    }


    #region 方法
    /// <summary>
    /// 显示当前选择结果
    /// </summary>
    public void ShowCurResultItem()
    {
        StageConfigManager.StageConfig stageConfig = StageConfigManager.GetStageConfig(GameData.Instance.GameStage);
        Dictionary<int, string> numIconPoolDic = stageConfig.numIconPoolDic;

        for (int i = 1; i <= GameData.Instance.curResultItemDic.Count; i++)
        {
            foreach (var item in numIconPoolDic)
            {
                string[] str = item.Value.Split(':');
                if (GameData.Instance.curResultItemDic[i] == int.Parse(str[0]))
                {
                    GameObject go = Instantiate(DragInputItemTemplate);
                    go.SetActive(true);
                    go.transform.parent = resultItemWidget.transform;
                    go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                    go.transform.Find("Label").gameObject.SetActive(true);
                    go.transform.Find("icon").gameObject.SetActive(true);
                    go.transform.Find("plusIcon").gameObject.SetActive(false);

                    InputDragItem sc = go.GetComponent<InputDragItem>();
                    sc.interactable = false;
                    sc.Apply(item.Value);

                    //改变过的item标记出来
                    if (GameData.Instance.curResultItemDic[i] == GameData.Instance.changedItemOne ||
                        GameData.Instance.curResultItemDic[i] == GameData.Instance.changedItemTwo)
                    {
                        go.transform.Find("bg").GetComponent<UISprite>().spriteName = "ItemBack_Red";
                        go.transform.Find("effect").gameObject.SetActive(true);
                    }
                    else {
                        go.transform.Find("effect").gameObject.SetActive(false);
                    }
                }
            }
            resultItemWidget.GetComponent<UIGrid>().maxPerLine = GameData.Instance.resultColumn;
        }
        GameData.Instance.changedItemOne = 0;
        GameData.Instance.changedItemTwo = 0;
    }



    public void Clear()
    {
        for (int i = 0; i < lightItemList.Count; i++)
        {
            Destroy(lightItemList[i]);
        }
    }

    #endregion
}
