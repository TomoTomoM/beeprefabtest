using UnityEngine;
using System.Collections;



public class BeeBehavior1 : MonoBehaviour {

	public GameObject stinger;


	public float speed;
	public float localXRange;
	public float localYRange;
	public Animator anim;
	public float stingerInterval;

	private GameObject Player;
	private Transform player;
	private Vector3 firstPosition;
//	private float firstX;
//	private float firstY;
//	private float firstZ;
	private float deltaX;
	private float deltaY;
	private float deltaZ;

//	private float maxLocalX;
//	private float minLocalX;
//	private float maxLocalY;
//	private float minLocalY;
	private float deltaTime = 0f;
	private float animTimeTracker = 0f;

	private bool stingerShooted = false;
	private int stingerCounter = 0;

	public Vector3 stingerOffset = new Vector3(0.1f,0.0f,0.4f);

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
		player = Player.transform; //player is just transform of game object "Player"
		firstPosition = transform.localPosition;
		LookAtPlayer (); //face the player
		//define the area the bee can move
		DefineMovingRange();
		anim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		deltaTime += Time.deltaTime;
		animTimeTracker += Time.deltaTime;
		GetXYDisplacement ();
		transform.Translate(deltaX, deltaY, 0);
		LookAtPlayer ();
		transform.Rotate (0, 10, 0);
		ShootStinger ();
		StartCoroutine ("CreateStinger");
		stingerShooted = false;
	}

	void LookAtPlayer(){
		if(player != null)
		{
			transform.LookAt(player);
		}
	}

	void DefineMovingRange(){
//		maxLocalX = transform.localPosition.x + localXRange / 2.0f;
//		minLocalX = transform.localPosition.x - localXRange / 2.0f;
//		maxLocalY = transform.localPosition.y + localYRange / 2.0f;
//		minLocalY = transform.localPosition.y - localYRange / 2.0f;
//		print (maxLocalX);
//		print (minLocalX);
	}

	void GetXYDisplacement(){
		// if n second passed since last change in movement, change it
		if (deltaTime > 2.0f) {
			deltaX = Random.Range (-speed, speed);
			deltaY = Random.Range (-speed, speed);
			deltaZ = Random.Range (-speed, speed);
			deltaTime = 0.0f;
		}	
		// if bee is far from its starting position, change direction
		if (Vector3.Distance (firstPosition, transform.position) > 1) {
			deltaX = -deltaX;
			deltaY = -deltaY;
			deltaZ = -deltaZ;
		}

//		if (transform.localPosition.x > maxLocalX) {
//			deltaX = Random.Range (speed, 0.0f); //not -speed because +x direction of bee is -x direction of the player
//			deltaTime = 0.0f;
//			print ("hit x max");
//		}
//		if (transform.localPosition.x < minLocalX) {
//			deltaX = Random.Range (-speed, 0.0f);	//not +speed because -x direction of bee is +x direction of the player
//			deltaTime = 0.0f;
//			print ("hit x min");
//		}
//		if (transform.localPosition.y > maxLocalY) {
//			deltaY = Random.Range (-speed*0.5f, 0.0f);
//			deltaTime = 0.0f;
//			print("hit y max");
//		}
//		if (transform.localPosition.y < minLocalY) {
//			deltaY = Random.Range (speed*0.5f, 0.0f);
//			deltaTime = 0.0f;
//			print ("change direction");
//		}
//		if (deltaTime > 3.0f) {
//			deltaX = Random.Range (-speed, speed);
//			deltaY = Random.Range (-speed*0.5f, speed);
//			deltaTime = 0.0f;
//		}
	}
		
	void ShootStinger(){
		if (animTimeTracker > stingerInterval) {
			anim.Play ("Hit1");
			stingerShooted = true;
			print ("shoot stinger");
			animTimeTracker = 0.0f;
		}
	}
		

	IEnumerator CreateStinger (){
		if (stingerShooted == true) {
			yield return new WaitForSeconds (0.3f);
			Instantiate (stinger, transform.position + stingerOffset, transform.rotation);
		}
	}


}
