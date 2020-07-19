using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour {
public List<Sprite> Sprites;
	void Start () {
		int rand = Random.Range(0,Sprites.Count-1);
		GetComponent<SpriteRenderer>().sprite = Sprites[rand];
	}

}
