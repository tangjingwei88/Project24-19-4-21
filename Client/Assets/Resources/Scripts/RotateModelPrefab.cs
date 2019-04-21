using UnityEngine;
using System.Collections;

public class RotateModelPrefab : MonoBehaviour {

    public void OnDrag(Vector2 delta)
    {
        LoadingManager.Instance.RotateModel(delta.x);
    }

}
