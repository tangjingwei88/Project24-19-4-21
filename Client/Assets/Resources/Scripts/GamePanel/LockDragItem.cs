using UnityEngine;
using System.Collections;
using System;

public class LockDragItem : UIDragDropItem
{

    protected override void OnDragDropStart()
    {
        base.OnDragDropStart();
    }


    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);

        if (surface.CompareTag("DragInput"))
        {
            surface.transform.Find("lockIcon").gameObject.SetActive(true);
            if (GameData.Instance.lockNum >= 1)
            {
                GameData.Instance.lockNum--;
                if (GameData.Instance.lockNum <= 0)
                {
                    GameData.Instance.lockNum = 0;
                    //GamePanel.Instance.Lock.SetActive(false);
                }
                GamePanel.Instance.LockNumLabel.text = GameData.Instance.lockNum.ToString();
            }

        }
    }
}
