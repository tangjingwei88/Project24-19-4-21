using UnityEngine;
using System.Collections;

public class UIStart : MonoBehaviour {

    #region 引用

    public TipsPopUpPanel theTipsPopUpPanel;
    public StartPanel theLoginPartPanel;
    public RegisterPanel theRegisterPanel;

    #endregion

    #region 属性
    private static UIStart _instance;
    public static UIStart Instance {
        get {
            return _instance;
        }
    }

    #endregion



    void Awake()
    {
        _instance = this;
    }



    void Start () {
 //       theLoginPartPanel.gameObject.SetActive(true);
 //       theRegisterPanel.gameObject.SetActive(false);
	}


    #region 方法



    #endregion
}
