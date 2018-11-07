using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour {
  //  public Transform targetTranform;
    //public Vector3 velocity = Vector3.forward;
   // public float speed = 1;
    public float animRandom = 2;
 
    public Animation animatio;
   //private void Start()
   // {
   //     StartCoroutine()
   // }

	private IEnumerator  Start () {
       // targetTranform = GameObject.Find("TargetPos").transform;
        animatio =GetComponentInChildren<Animation>();
        yield return new WaitForSeconds(Random.Range(0, animRandom));
        animatio.Play(); 
    }
	
	// Update is called once per frame
	void Update () {
     //   transform.Translate(targetTranform .position* Time.deltaTime,Space.World);
     //   transform.LookAt(targetTranform.position, Vector3.up);
      //  transform.Translate(transform.position*Time.deltaTime*speed);

    }
}
