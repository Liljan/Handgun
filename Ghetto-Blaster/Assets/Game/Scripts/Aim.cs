
using UnityEngine;
using System.Collections;

public class Aim : MonoBehaviour {

    private Transform aim;

    private Vector3 pos;
    public float zDistance = -10f;

	// Use this for initialization
	void Start () {
        aim = GetComponent<Aim>().transform;
      //  Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistance);
        pos = Camera.main.ScreenToWorldPoint(pos);
        aim.position = pos;
	}
}
