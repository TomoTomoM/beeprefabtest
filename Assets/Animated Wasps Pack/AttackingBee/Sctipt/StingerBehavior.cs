﻿using UnityEngine;
using System.Collections;

public class StingerBehavior : MonoBehaviour {

	//public Transform player;
	//public Transform controllerPointer;
	public float speed ;
	public Vector3 playerHoldingOffset;
	public BeeBehavior1 beeScript;
	public GameObject explosion;
	//public float shootingSpeed = 1000.0f;

	//public Vector3 pointer;
	private GameObject player;
	private GameObject pointer;
	private Vector3 shootingDestination;

	//private Rigidbody rb;
	//private GameObject controllerPointer;

	public int stingerState = 0; //0:coming, 1:getting 2:holding, 2:shooting

	// Use this for initialization
	void Start () {
		//rb = GetComponent<Rigidbody>();
		player = GameObject.FindGameObjectWithTag ("Player");
		pointer = GameObject.FindGameObjectWithTag ("ControllerPointer");
		transform.parent = player.transform;
		//controllerPointer = GameObject.FindGameObjectWithTag ("ControllerPointer");
		 //set transform parent to the playr so that stinger does not move with bee
		LookAtPlayer ();
	}

	// Update is called once per frame
	void Update () {
		if (stingerState == 0) {
			comingUpdate ();
		}
		if (stingerState == 1) {
			stoppingUpdate ();
		}
		if (stingerState == 2) {
			gettingUpdate ();
		}
		if (stingerState == 3) {
			holdingUpdate ();
		}
		if (stingerState == 4) {
			shootingUpdate ();
		}
		if (stingerState == 5) {
			Instantiate (explosion, transform.position, transform.rotation);
			Destroy (this.gameObject);
		}
	}


	void comingUpdate(){
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, player.transform.position, step); //move stinger to the player with constant speed
		if (Input.GetKeyDown("1")){
				stingerState = 1;
			}
		if (Vector3.Distance(player.transform.position,transform.position) < 0.1f){
			print("Stinger Destroyed");
			Destroy(this.gameObject);
		}
	}

	void stoppingUpdate(){
		//transform.position = transform.position;
		if (GvrController.Gyro[0] > 0.5){
			stingerState = 2;
		}
				if (Input.GetKeyDown("2")){
					stingerState = 2;
				}
	}

	void gettingUpdate(){
		float step = speed * 10.0f * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, player.transform.position + playerHoldingOffset, step); //move stinger to the player with constant speed
		if (Vector3.Distance(player.transform.position+playerHoldingOffset,transform.position) < 0.3){
			stingerState = 3;
		}
	}
	void holdingUpdate(){
		transform.position = player.transform.position + playerHoldingOffset; //fix position of the stinger near the player. 
		transform.LookAt(pointer.transform.position );
		//bee should be replaced by controller pointer
		transform.Rotate (90, 0, 0);
				if (Input.GetKeyDown("3")){
					shootingDestination = 2*pointer.transform.position-transform.position;
					stingerState = 4;
				}
	}
	void shootingUpdate(){
		//use add force instead
		float step = speed * 10.0f * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, shootingDestination , step); //move stinger to the player with constant speed
		//rb.AddForce ((shootingDestination-transform.position) * speed);
		//print ("shooted");
		Destroy(this.gameObject,1);
	}
	void collidedUpdate(){
		
	}



	void LookAtPlayer(){
		if(player != null)
		{
			transform.LookAt(player.transform); 
			transform.Rotate (90, 0, 0);
		}
	}

	public void StoppingStinger(){
		stingerState += 1;
	}

	public void ShootingStinger(){
		if (stingerState == 1) {
			stingerState = 0;
		} else {
			shootingDestination = 5*pointer.transform.position-4*transform.position;
			stingerState = 4;
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Bee"))
		{
			if (stingerState == 4 ) {
				print ("Bee shooted");
				//other.gameObject.SetActive (false);
				beeScript = other.gameObject.GetComponent<BeeBehavior1>();
				beeScript.shooted = true;
				Rigidbody rb = other.GetComponent<Rigidbody> ();
				Vector3 movement = shootingDestination - transform.position;
				rb.AddForce (movement * 300.0f);
			}
		}
		if (other.gameObject.CompareTag("Stinger")){
			StingerBehavior stingerScript = other.GetComponent<StingerBehavior> ();
			stingerScript.stingerState = 5;
		}
	}
		
}
