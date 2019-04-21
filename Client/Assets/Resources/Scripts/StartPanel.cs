/***
 *  Author: TSoft
 *  Date:   2018-7-4-22:33
 */

using UnityEngine;
using System.Collections;

public class StartPanel : MonoBehaviour
{

    #region 引用

    public RegisterPanel theRegisterPanel;
    public UIInput AccountInput;
    public UIInput passWordInput;

    #endregion

    #region 变量

    #endregion

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void OnStartGameClick()
    {
        if (CheckAccountPassWord())
        {
            Debug.LogError("登录成功");
            LoadingManager.Instance.EnterScene("MainScence");
        }
        else {
            Debug.LogError("登录失败！请检查用户名密码！");
        }
    }


    /// <summary>
    /// 校验用户名密码
    /// </summary>
    /// <returns></returns>
    public bool CheckAccountPassWord()
    {
        if (AccountInput.value == PlayerPrefs.GetString("Account") && passWordInput.value == PlayerPrefs.GetString("PassWord"))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 注册按钮
    /// </summary>
    public void OnRegisterBtnClick()
    {
        theRegisterPanel.ApplyInfo();
        this.gameObject.SetActive(false);
        theRegisterPanel.gameObject.SetActive(true);
        Debug.LogError("OnRegisterBtnClick");
    }
}
