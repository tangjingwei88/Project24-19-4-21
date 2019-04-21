using UnityEngine;
using System.Collections;

public class CircleRotation : MonoBehaviour {

    float speed = 10;
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(0,0,Time.deltaTime * speed);
	}
}
