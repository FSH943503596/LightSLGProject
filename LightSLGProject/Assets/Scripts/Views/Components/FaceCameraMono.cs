/*
 *	Author:	贾树永
 *	CreateTime:	2019-03-22-10:40:02
 *	Vertion: 1.0	
 *
 *	Description:
 *
*/
using UnityEngine;

public class FaceCameraMono : MonoBehaviour
{
    private static Camera _Camera;
    private void Start()
    {
        if (_Camera == null) 
            _Camera = GameObject.FindGameObjectWithTag(GlobalSetting.TAG_BATTLE_SCENE_CAMERA_NAME).GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 vector3 = _Camera.WorldToViewportPoint(transform.position);
        Ray ray = _Camera.ViewportPointToRay(vector3);
        transform.LookAt(-ray.direction + transform.position,Vector3.forward);
    }
}
