using UnityEngine;
using System.Collections;

public class LogBtnDrag : UIDragDropItem
{
    public GameObject logPart;

    protected override void OnDragDropStart()
    {
        base.OnDragDropStart();
    }


    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);
        Vector3 dragReleasePos = this.gameObject.transform.localPosition;
        logPart.transform.localPosition = new Vector3( dragReleasePos.x - 170f,dragReleasePos.y - 390f,dragReleasePos.z);
    }



}