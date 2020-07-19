using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderRemover : MonoBehaviour
{

    public GameObject Player;

    void Start(){
        Player = GameManager.Instance.Player.gameObject;
    }
    void OnTriggerEnter2D(Collider2D pOther){
        if(pOther.tag=="CollisionDetection"){
           Physics2D.IgnoreLayerCollision(10, 11,true);
        }
    }

    void OnTriggerExit2D(Collider2D pOther){
        if(pOther.tag=="CollisionDetection"){
           Physics2D.IgnoreLayerCollision(10,11,false);
        }
    }
}
