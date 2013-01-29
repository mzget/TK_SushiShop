using UnityEngine;
using System.Collections;

public class Mz_billboardBeh : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
	}
}
