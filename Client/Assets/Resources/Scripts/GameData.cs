using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData
{

    #region 变量

    public int GameStage = 1;          //游戏关卡
    public string gameInput;            //玩家输入数据
    public int gameRound = 1;           //玩家输入次数
    public float roundTime = 300;       //每局游戏时间
    public bool win = false;            //游戏结果
    public int gameLv = 4;
    public int showColumn = 5;          //游戏显示列数
    public int resultColumn = 4;

    public int changedItemOne = 0;      //改变过的item
    public int changedItemTwo = 0;      //改变过的item
    public int diamonds = 100;          //钻石总量
    public int lockNum = 0;             //锁标签的数量

    public Dictionary<int, int> curResultItemDic = new Dictionary<int, int>();
    #endregion

    private static GameData _instance;
    public static GameData Instance {
        get
        {
            if (_instance == null)
            {
                _instance = new GameData();
            }
            return _instance;
        }
    }

}


/// <summary>
/// 红黄绿类型
/// </summary>
public enum LIGHT_TYPE
{
    red,
    yellow,
    green
}

/// <summary>
/// 游戏等级
/// </summary>
public enum GAME_LEVEL
{ 
    Three = 3,
    Four =  4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9
}

/// <summary>
/// 模型朝向
/// </summary>
public enum MODEL_DIRECTION
{
    front,
    back,
    right
}