using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {

    public float viewDistance = 5;
    public float viewAngle = 120; 
    public Transform PlayerTranform; 
    void Start () { 
        PlayerTranform = GameObject.Find("Player").transform; 
	} 
	void Update () { 
        if (Vector3.Distance(PlayerTranform.position, transform.position)<=viewDistance)
        { 
            Vector3 playerDir = PlayerTranform.position - transform.position;

           float angle=    Vector3.Angle(playerDir, transform.forward);

            if (angle<= viewAngle/2)
            {
                Debug.Log("在视野范围内");
            } 
        } 
	}
}
