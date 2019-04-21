using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 直接挂载这个脚本即可通过陀螺仪来控制摄像机的旋转
/// </summary>
public class GyroControllCamera : MonoBehaviour
{
    //陀螺仪
    private Gyroscope gyro;

    private Quaternion quatMult;
    //设备的状态(即，空间方向)。
    private Quaternion quatMap;
    //锁定
    private bool isLock;
    //移动速度 
    private float speed = 0.01f;
    //摄像机的父对象
    public GameObject camParent;
    protected void Start()
    {
        camParent.transform.position = transform.position;
        transform.parent = camParent.transform;


        isLock = false;
        //获取陀螺仪
        gyro = Input.gyro;
        //启用陀螺仪
        gyro.enabled = true;
        //启用陀螺仪钱需要翻转一下摄像机，否则角度不对，大家可以删除这段代码实验一下
        camParent.transform.eulerAngles = new Vector3(90, 90, 0);
        //用来和
        quatMult = new Quaternion(0, 0, 1, 0);


    }


    protected void Gyro()
    {
        //获取设备的空间方向
        quatMap = new Quaternion(gyro.attitude.x, gyro.attitude.y, gyro.attitude.z, gyro.attitude.w);
        Quaternion qt = quatMap * quatMult;
        //在localRotation和qt之间的插值
        transform.localRotation = Quaternion.Slerp(transform.localRotation, qt, speed);


        if (isLock)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }


    protected void Update()
    {
        Gyro();
    }
}

