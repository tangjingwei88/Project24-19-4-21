using UnityEngine;
using System.Collections;

public class CircleRotationRevers : MonoBehaviour {

    float speed = 20;

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 0, -Time.deltaTime * speed);
    }
}
