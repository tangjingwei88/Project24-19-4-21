using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowLogPart : MonoBehaviour {

    #region 引用
    public GameObject logShowItemTemplate;
    public GameObject LogShowScrollRoot;
    public GameObject LogShowWidget;
    #endregion


    #region 变量

    private List<GameObject> logList = new List<GameObject>();
    #endregion

    void Update()
    {
        //LogShowScrollRoot.GetComponent<UIScrollView>().verticalScrollBar.value = 1f;
    }


    public void Apply(Dictionary<LIGHT_TYPE, int> dic)
    {
        //LogShowScrollRoot.GetComponent<UIScrollView>().verticalScrollBar.value = 1f;
        GameObject go = Instantiate(logShowItemTemplate);
        go.SetActive(true);
        go.transform.parent = LogShowWidget.transform;
        go.transform.localScale = Vector3.one;
        logList.Add(go);

        LogShowItemTemplate sc = go.GetComponent<LogShowItemTemplate>();
        sc.Apply(dic);
        LogShowWidget.GetComponent<UIGrid>().repositionNow = true;
        GameData.Instance.gameRound++;
        transform.Find("LogShowScrollRoot/bar").gameObject.GetComponent<UIProgressBar>().value = 1;
    }

    /// <summary>
    /// 清空log列表
    /// </summary>
    public void Clear()
    {
        if (logList.Count != 0)
        {
            for (int i = 0; i < logList.Count; i++)
            {
                Destroy(logList[i]);
            }
        }
    }



}
