using UnityEngine;
using System.Collections;

public class CreditsScroll : MonoBehaviour {

    public float speed = 1.0f;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
    }
}
