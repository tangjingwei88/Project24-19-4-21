using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfoTemplate : MonoBehaviour {

    #region 引用
    //关卡名称
    public UILabel stageNumLabel;
    public UILabel difficultLabel;
    public UISprite stageSprite;

    #endregion

    #region 变量
    private int stage;

    #endregion

    public void Apply(int stageNum,string iconName)
    {
        stage = stageNum;
        stageNumLabel.text = stageNum.ToString();
        stageSprite.name = iconName;
    }


    public void OnClick()
    {
        GameData.Instance.GameStage = int.Parse( stageNumLabel.text);
        //UIMain.Instance.theStagePassedPanel.gameObject.SetActive(false);
        //UIMain.Instance.theGamePanel.gameObject.SetActive(true);
        UIMain.Instance.FadeToUIState(UIState.GamePanelState);
        UIMain.Instance.theGamePanel.Apply(GameData.Instance.GameStage);
    }
}
