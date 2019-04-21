using UnityEngine;
using System.Collections;

public class GameOverPanel : MonoBehaviour
{
    #region 引用
    public UILabel winLabel;
    public UILabel LoseLabel;
    public UILabel goldLabel;
    public UILabel timeLabel;

    public GameObject NextStageBtn;
    #endregion


    #region 变量

    #endregion


    #region 方法
    public void Apply()
    {
        timeLabel.text = "耗时：" + GamePanel.Instance.timer.ToString();

        if (GameData.Instance.win)
        {
            
            int curStage = PlayerPrefs.GetInt("GameStage");
            winLabel.gameObject.SetActive(true);
            LoseLabel.gameObject.SetActive(false);
            //只有是最新关卡通过，关卡等级才加1
            if (GameData.Instance.GameStage == curStage)
            {
                GameData.Instance.GameStage += 1;
                PlayerPrefs.SetInt("GameStage", GameData.Instance.GameStage);
                NextStageBtn.SetActive(true);
            }
        }
        else {
            NextStageBtn.SetActive(false);
            winLabel.gameObject.SetActive(false);
            LoseLabel.gameObject.SetActive(true);
        }
    
    }

    /// <summary>
    /// 确定按钮返回选关界面
    /// </summary>
    public void OKBtnClick()
    {
        this.gameObject.SetActive(false);
        StopAllCoroutines();
        GamePanel.Instance.timer = 0;

        int curStage = PlayerPrefs.GetInt("GameStage");
        UIMain.Instance.FadeToUIState(UIState.StagePassedPanelState);
        UIMain.Instance.theStagePassedPanel.Apply(curStage);
    }

    /// <summary>
    /// 再玩一局
    /// </summary>
    public void NextStageBtnClick()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
        GamePanel.Instance.timer = 0;


        UIMain.Instance.FadeToUIState(UIState.GamePanelState);
        GamePanel.Instance.Apply(GameData.Instance.GameStage);
    }


    /// <summary>
    /// 下一关卡
    /// </summary>
    public void NextStage()
    {
        GameData.Instance.GameStage += 1;
        GamePanel.Instance.Apply(GameData.Instance.GameStage);
    }

    #endregion
}
