using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthAndEnergyScript : MonoBehaviour {

	// Use this for initialization

	//public Image healthBar;
	//public int maxHealth, currentHealth = 150;

	public Image healthBar;
//	public Image healthImage;
//	List<Image> lifePoints = new List<Image>();
	public GameObject healthImage;
	List<GameObject> lifePoints = new List<GameObject>();


	public Image energyBar;
	public int maxEnergy = 100;
	public float currentEnergy = 50;
	private float energyCoeff;


	public int maxLifePoints = 3;
	public float spacingX = 3.0f;
	public float spacingY = 10.0f;
	private float startingX;

	public Image hurtEffect;

	void Start () {
		startingX = spacingX * (-1);
		setHurtAlpha(0f);
		AddLifePoints ();
	}

	void Update() {
		if (Input.GetKeyDown ("f")) {
			removeLifePoint ();
		}
		decrementEnergy (energyCoeff);
		if (currentEnergy > 0.01) {
			UpdateEnergy ();
		}
	}
	// Update is called once per frame

	void decrementEnergy(float coef){
		currentEnergy -= coef * Time.deltaTime;
	}

	void UpdateEnergy () {
		float energyRatio = currentEnergy / (maxEnergy * 1.0f);
		//Debug.Log ("ENERGY RATIO: " + energyRatio.ToString ());
		energyBar.rectTransform.localScale = new Vector3 (energyRatio, 1, 1);
	}

	void AddLifePoints(){
		for (int i = 0; i < maxLifePoints; i++) {
			Debug.Log ("Instantiating Helmet" + i.ToString ());
			GameObject magnetoHelmet = Instantiate (healthImage, healthBar.transform.position + new Vector3 (startingX, spacingY, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
			Debug.Log (magnetoHelmet);
			magnetoHelmet.transform.parent = transform.GetChild(0).gameObject.transform;
			magnetoHelmet.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			startingX += spacingX;
			lifePoints.Add (magnetoHelmet);
		}
				
	}
	//when get hit, removes one point
	public void removeLifePoint() {
		Debug.Log ("Life points left: ");
		Debug.Log(lifePoints.Count);
		//hurtEffect.color.a = 1
		setHurtAlpha(1f);
		int lastIndex = lifePoints.Count - 1;
		Debug.Log (lifePoints [lastIndex]);
		//lifePoints [lastIndex].enabled = false;
		maxLifePoints -= 1;
		if (maxLifePoints < 1) {
			Application.LoadLevel ("True Main Menu");
		} else {
			lifePoints [lastIndex].SetActive (false);
			lifePoints.RemoveAt (lastIndex);
			StartCoroutine (fadeAlphaToZero ());
		}
	}

	public void increaseEnergy(float energyAmount){
		currentEnergy = Mathf.Max (maxEnergy, currentEnergy + energyAmount);
	}

	public void changeEnergyCoeff(float newCoeff) {
		energyCoeff = newCoeff;
		Debug.Log ("Updated Energy Coeff: " + newCoeff.ToString());
	}

	void setHurtAlpha(float alpha) {
		Color tempHurtColor = hurtEffect.color;
		tempHurtColor.a = alpha;
		hurtEffect.color = tempHurtColor;
	}

	IEnumerator fadeAlphaToZero(){
		Color tempHurtColor = hurtEffect.color;
		float current_alpha = tempHurtColor.a;
		while (current_alpha > 0) {
			tempHurtColor.a -= 0.08f;
			hurtEffect.color = tempHurtColor;
			current_alpha = tempHurtColor.a;
			yield return null;
		}
	}



}
