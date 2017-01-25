using UnityEngine;
using System.Collections;



public class BeeBehavior1 : MonoBehaviour {

	public GameObject stinger;
	public float speed;
	public float localXRange;
	public float localYRange;
	public Animator anim;
	public float stingerInterval;
	public bool shooted = false;

	private GameObject StingerPoint;
	private GameObject Player;
	private Transform player;
	private Vector3 firstPosition;
//	private float firstX;
//	private float firstY;
//	private float firstZ;
	private float deltaX;
	private float deltaY;
	private float deltaZ;
	private bool dead = false;
//	private float maxLocalX;
//	private float minLocalX;
//	private float maxLocalY;
//	private float minLocalY;
	private float deltaTime = 0f;
	private float animTimeTracker = 0f;

	private bool stingerShooted = false;


//	public Vector3 stingerOffset = new Vector3(0.1f,0.0f,0.4f);

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
		player = Player.transform; //player is just transform of game object "Player"
		StingerPoint = GameObject.FindGameObjectWithTag("BeeSting");
		firstPosition = transform.localPosition;
		LookAtPlayer (); //face the player
		//define the area the bee can move
		anim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		if (shooted == false) {
			deltaTime += Time.deltaTime;
			animTimeTracker += Time.deltaTime;
			GetXYDisplacement ();
			transform.Translate (deltaX, deltaY, 0);
			LookAtPlayer ();
			transform.Rotate (0, 10, 0);
			ShootStinger ();
			StartCoroutine ("CreateStinger");
			stingerShooted = false;

		}else{
			anim.Play("Death1");
			StartCoroutine ("KillBee");
			if (dead)
			{
				transform.position = Vector3.MoveTowards (transform.position, new Vector3(transform.position.x,0,transform.position.z) , 0.05f); //fall down
			}
		}
	}

	void LookAtPlayer(){
		if(player != null)
		{
			transform.LookAt(player);
		}
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
			Instantiate (stinger, StingerPoint.transform.position, transform.rotation);
		}
	}

	IEnumerator KillBee (){
		yield return new WaitForSeconds (0.6f);
		dead = true;
		yield return new WaitForSeconds (1.4f);
		anim.Stop ();
		gameObject.SetActiveRecursively(false);
	}

}
