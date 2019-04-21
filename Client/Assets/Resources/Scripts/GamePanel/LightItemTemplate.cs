using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LightItemTemplate : MonoBehaviour {

    #region 引用
    public UISprite bgSprite;
    public UISprite colorSprite;


    #endregion

    #region 方法


    internal void Apply(KeyValuePair<LIGHT_TYPE, int> item)
    {
        switch (item.Key)
        {
            case LIGHT_TYPE.red:
                colorSprite.spriteName = "red";
                break;
            case LIGHT_TYPE.yellow:
                colorSprite.spriteName = "yellow";
                break;
            case LIGHT_TYPE.green:
                colorSprite.spriteName = "green";
                break;
        }
        colorSprite.gameObject.SetActive(true);
    }

    #endregion

}
