using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowResultPart : MonoBehaviour {

    #region 引用

    public GameObject LightTemplate;
    public GameObject LightWidget;

    public Transform anchorRoot;
    public GameObject numberInputRoot;
    public AudioClip showResultMusic;
    public AudioClip showLightMusic;
    public Transform showEffectParent;

    public GameOverPanel theGameOverPanel;
    #endregion


    #region 变量
    public List<GameObject> lightItemList = new List<GameObject>();

    #endregion

    void Awake()
    {

    }



    public void Apply(Dictionary<LIGHT_TYPE, int> dictionary)
    {
        if (lightItemList != null) Clear();
        //游戏胜利
        if (dictionary[LIGHT_TYPE.green] == GameData.Instance.gameLv)
        {
            GamePanel.Instance.StopTimer();
            GameData.Instance.win = true;
            StopAllCoroutines();
            //theGameOverPanel.gameObject.SetActive(true);
            UIMain.Instance.FadeToUIState(UIState.GameOverState);
            UIMain.Instance.theGameOverPanel.Apply();
            GamePanel.Instance.theShowLogPart.Clear();
            Clear();
            NGUITools.PlaySound(showResultMusic, 0.1f);
        }
        else
        {
            StartCoroutine(ShowLight(dictionary));
        }

        ShowResultEffect();
    }


    public IEnumerator ShowLight(Dictionary<LIGHT_TYPE, int> dictionary)
    {
        foreach (var item in dictionary)
        {
            for (int i = 0; i < item.Value; i++)
            {
                GameObject go = Instantiate(LightTemplate);
                go.SetActive(true);
                go.transform.parent = LightWidget.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;

                LightItemTemplate sc = go.GetComponent<LightItemTemplate>();
                sc.Apply(item);
                LightWidget.GetComponent<UIGrid>().repositionNow = true;
                LightWidget.GetComponent<UIGrid>().maxPerLine = GameData.Instance.resultColumn;
                lightItemList.Add(go);
                NGUITools.PlaySound(showLightMusic,0.1f);
                NGUITools.soundVolume = 1;
                yield return new WaitForSeconds(0.1f);
            }

        }
    }

    /// <summary>
    /// 显示结果特效
    /// </summary>
    /// <param name="name"></param>
    public void ShowResultEffect()
    {
       // NGUITools.PlaySound(showResultMusic,0.1f);
        GameObject showEffectGO = LoadingManager.Instance.NewUIParticle("LevelUp_Big_Scale");
        showEffectGO.transform.parent = showEffectParent;
        showEffectGO.transform.localPosition = Vector3.zero;

        Destroy(showEffectGO,1.0f);
    }



    /// <summary>
    /// 刷新显示
    /// </summary>
    public void Clear()
    {
        if (lightItemList != null)
        {
            for (int i = 0; i < lightItemList.Count; i++)
            {
                Destroy(lightItemList[i]);
            }
        }
    }
}
