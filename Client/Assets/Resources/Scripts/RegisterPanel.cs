using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class RegisterPanel : MonoBehaviour
{

    #region 引用

    public UIInput AccountInput;
    public UIInput passWordInput;
    public UIInput passWord2Input;

    public StartPanel theStartPanel;
    #endregion

    #region 变量

    private string account;
    private string passWord;
    private string repeatPassWord;

    #endregion


    #region 方法

    public void ApplyInfo(string account = null,string password = null)
    {
        AccountInput.value = account;
        passWordInput.value = password;
        passWord2Input.value = passWord;

    }



    /// <summary>
    /// 注册
    /// </summary>
    public void OnRegisterBtnClick()
    {
        account = AccountInput.value;
        passWord = passWordInput.value;
        repeatPassWord = passWord2Input.value;

        //校验用户名和密码
        if (!CheckAccountPassWordRule(account, passWord, repeatPassWord))
        {
            return;
        }
        else {
            RegisterNewAccount(account, passWord);
        }
        
        //Debug.LogError("OnRegisterBtnClick");
    }


    /// <summary>
    /// 校验注册信息
    /// </summary>
    /// <param name="account"></param>
    /// <param name="password"></param>
    /// <param name="repeatPassWord"></param>
    /// <returns></returns>
    public bool CheckAccountPassWordRule(string account, string password, string repeatPassWord)
    {
        //合法字符
        Regex Character = new Regex(@"^[_\.a-zA-Z0-9-]*$");
        if (account.Length <= 0 || passWord.Length <= 0)
        {
            Debug.LogError("用户名或密码不能为空！");
            return false;
        }
        else if (passWord != repeatPassWord)
        {
            Debug.LogError("输入密码不一致！");
        }
        else if (account.Length >= 4 && account.Length < 20 && passWord.Length >= 6 && passWord.Length < 20 && Character.IsMatch(account) && Character.IsMatch(passWord))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 注册新用户
    /// </summary>
    /// <param name="account"></param>
    /// <param name="passWord"></param>
    public void RegisterNewAccount(string account, string passWord)
    {
        PlayerPrefs.SetString("Account",account);
        PlayerPrefs.SetString("PassWord",passWord);

        Debug.LogError("注册成功！");
        this.gameObject.SetActive(false);
        theStartPanel.gameObject.SetActive(true);
    }



    /// <summary>
    /// 已有账号登录
    /// </summary>
    public void OnBackLoginClick()
    {
        this.gameObject.SetActive(false);
        theStartPanel.gameObject.SetActive(true);
    }


    #endregion
}
