using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript : MonoBehaviour
{
    public Animator anim;
    public bool spin = true;
    public int value;
    public bool eaten = false;
    public GameObject bloodsplatter;
    Rigidbody2D rigidbody2d;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }
    public void Stop(){
        if(eaten == false){
            value = Random.Range(1,7);
            anim.SetInteger("Roll Value", value);
        }
        else{
            value = -1;
            anim.SetInteger("Roll Value", 7);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(eaten == true){
            value = -1;
            anim.SetInteger("Roll Value", 7);
        }
    }
    public void Clicked(){
        Instantiate(bloodsplatter, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
    }
}
