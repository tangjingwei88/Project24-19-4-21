using UnityEngine;
using System.Collections;

public class TipsManager : MonoBehaviour {

    public UILabel tipsLabel;


    private static TipsManager _instance;
    public static TipsManager Instance {
        get {
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }



    public void ShowTips(string msg)
    {
        StartCoroutine("showTips",msg);
    }

    protected IEnumerator showTips(string msg)
    {
        tipsLabel.text = msg;
        tipsLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        tipsLabel.gameObject.SetActive(false);

    }
}
