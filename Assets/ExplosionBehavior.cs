using UnityEngine;
using System.Collections;

public class ExplosionBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, 0.6f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
