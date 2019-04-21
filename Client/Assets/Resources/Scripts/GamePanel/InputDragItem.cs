using UnityEngine;
using System.Collections;
using System;

public class InputDragItem : UIDragDropItem {

    #region 引用
    //该item的父亲
    private GameObject itemParent;
    public AudioClip dragMusic;
    public UILabel nameLabel;
    public UISprite icon;
    public UISprite lockIcon;
    public GameObject DragInputWidget;

    #endregion
    void Start()
    {
        DragInputWidget.GetComponent<UIGrid>().repositionNow = false;
    }

    #region 方法

    public void Apply(string nameIconStr)
    {
        string[] strList = nameIconStr.Split(':');
        nameLabel.text = strList[0];
        icon.spriteName = strList[1];

    }


    /// <summary>
    /// 重写父类的拖拽开始函数
    /// </summary>
    protected override void OnDragDropStart()
    {

        //得到该item的父亲
        this.itemParent = this.transform.parent.gameObject;

        base.OnDragDropStart();
    }


    /// <summary>
    /// 拖拽释放
    /// </summary>
    /// <param name="surface"></param>
    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);
        if (surface.tag == "DragInput")
        {
            //被拖拽item的显示内容
            string tempStr = transform.GetComponentInChildren<UILabel>().text;
            //将改变过的item暂存起来，log显示时改变背景色以示区分
            GameData.Instance.changedItemOne = int.Parse(tempStr);
            string tempSpriteName = transform.Find("icon").GetComponentInChildren<UISprite>().spriteName;
            bool isLock = transform.Find("lockIcon").gameObject.activeSelf;
        //    string tempBgName = transform.FindChild("bg").GetComponentInChildren<UISprite>().spriteName;
            //被拖拽item的名字
            string itemName = this.gameObject.name;
            //名字裁剪（Clone）
            itemName = itemName.Substring(0,(itemName.Length - 7));

            string surfaceStr = surface.transform.Find("Label").GetComponentInChildren<UILabel>().text;
            
            //将改变过的item暂存起来，log显示时改变背景色以示区分
            GameData.Instance.changedItemTwo = int.Parse(surfaceStr);

            //将拖拽碰撞检测到item显示内容赋值给被拖拽item
            string str = itemParent.transform.Find(itemName + "/Label").GetComponent<UILabel>().text;
            if (!str.Equals("0"))
            {
                if (!surfaceStr.Equals("0"))
                {
                    itemParent.transform.Find(itemName + "/Label").GetComponent<UILabel>().text = surfaceStr;
                    GameObject go1 = itemParent.transform.Find(itemName + "/icon").gameObject;
                    go1.GetComponent<UISprite>().spriteName = surface.transform.Find("icon").GetComponentInChildren<UISprite>().spriteName;
                    go1.transform.localScale = Vector3.zero;
                    TweenScale.Begin(go1, 1, Vector3.one);
                    //      itemParent.transform.Find(itemName + "/bg").GetComponent<UISprite>().spriteName = surface.transform.FindChild("bg").GetComponentInChildren<UISprite>().spriteName;
                    itemParent.transform.Find(itemName + "/lockIcon").gameObject.SetActive(surface.transform.Find("lockIcon").gameObject.activeSelf);

                    //将被拖拽item的内容赋值给拖拽检测到的item
                    surface.transform.Find("Label").GetComponentInChildren<UILabel>().text = tempStr;
                    GameObject go2 = surface.transform.Find("icon").gameObject;
                    go2.GetComponentInChildren<UISprite>().spriteName = tempSpriteName;
                    go2.transform.localScale = Vector3.zero;
                    TweenScale.Begin(go2, 1, Vector3.one);
                    //     surface.transform.FindChild("bg").GetComponentInChildren<UISprite>().spriteName = tempBgName;
                    surface.transform.Find("lockIcon").gameObject.SetActive(isLock);
                }
                else {
                    //将拖拽item的值赋给碰撞检测到的item，原来的绿“+”隐藏，label和icon显示出来
                    surface.transform.Find("plusIcon").gameObject.SetActive(false);
                    surface.transform.Find("Label").gameObject.SetActive(true);
                    GameObject go3 = surface.transform.Find("icon").gameObject;
                    go3.SetActive(true);
                    go3.transform.localScale = Vector3.zero;
                    TweenScale.Begin(go3, 1, Vector3.one);
                    surface.transform.Find("lockIcon").gameObject.SetActive(isLock);

                    surface.transform.Find("Label").GetComponentInChildren<UILabel>().text = tempStr;
                    surface.transform.Find("icon").GetComponentInChildren<UISprite>().spriteName = tempSpriteName;

                    //被拖拽item的值置空，隐藏label和icon，显示绿“+”
                    itemParent.transform.Find(itemName + "/Label").GetComponent<UILabel>().text = "0";
                    itemParent.transform.Find(itemName + "/Label").gameObject.SetActive(false);
                    GameObject go4 = itemParent.transform.Find(itemName + "/icon").gameObject;
                    go4.SetActive(false);
                    go4.transform.localScale = Vector3.zero;
                    TweenScale.Begin(go4, 1, Vector3.one);
                    //显示绿“+”
                    itemParent.transform.Find(itemName + "/plusIcon").gameObject.SetActive(true);
                    itemParent.transform.Find(itemName + "/lockIcon").gameObject.SetActive(false);
                }
            }


            GamePanel.Instance.RefreshDragItemState();

            NGUITools.PlaySound(dragMusic, 0.1f);
        }
    }

    #endregion
}
