using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewBehaviourScript : MonoBehaviour {
     
	void Start () {
        cube.position = a.position;
	}
    public Transform a;
    public Transform b;
    public Transform cube;
    // Update is called once per frame
    public float speed = 20;
    bool isDown = false;

  public   string names = "";
    void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            //names = GetCurr.instance. GetCurrentGameObject(); 
        }
        if (names=="Cube")
        {
            Move();
        }
      
    }
    private void Move()
    {

        if (Input.GetMouseButtonDown(0))
        {
            isDown = !isDown;
        }
        if (isDown)
        {
            Vector3 pos = Vector3.Lerp(cube.position, b.position, speed * Time.deltaTime);
            if (Vector3.Distance(pos, b.position) <= 1)
            {
                pos = b.position;
            }
            cube.transform.position = pos;
        }
        else
        {
            Vector3 pos = Vector3.Lerp(cube.position, a.position, speed * Time.deltaTime);
            if (Vector3.Distance(pos, a.position) <= 1)
            {
                pos = a.position;
            }
            cube.transform.position = pos;
        }
    }

    public Camera mainCrma;//这个相机用ARCamera下的相机

    private RaycastHit objhit;

    private Ray _ray;
    public string GetCurrentGameObject()
    { 
        if (Input.GetMouseButtonDown(0)) 
        { 
            _ray = mainCrma.ScreenPointToRay(Input.mousePosition);//从摄像机发出一条射线,到点击的坐标
             
            if (Physics.Raycast(_ray, out objhit )) 
            { 
                GameObject gameObj = objhit.collider.gameObject;//获取到射线碰撞到的物体
                                                                //TODO：然后进行你想要的事件处理

                if (gameObj != null)
                {
                    print(gameObj.name);
                  return  gameObj.name ;
                } 
            } 
        }
        return "";
    }
}




