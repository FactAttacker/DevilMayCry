using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScript : MonoBehaviour
{
    //public GameObject player;
    public Vector3 hitPoint;
    public float speed = 5f;
    public LineRenderer lr;
    //public bool coll = false;
    //Player_Script playerScript
    public GameObject watch;

    private void Start()
    {
        lr.SetPosition(0, watch.transform.position);
    }

    private void Update()
    {
        transform.localPosition += Vector3.forward * speed *Time.deltaTime;
        
        lr.SetPosition(1, transform.position);
        //transform.LookAt();

    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Cube") 
        {
            
            print("Ãæµ¹");
            hitPoint= collision.contacts[0].point;
            print(hitPoint);
            GameObject player = GameObject.Find("xbot");
            Player_Script player_Script = player.GetComponent<Player_Script>();

            //player_Script.indicatorAttach = true;
            //player_Script.attachPosition = hitPoint;

            speed = 0f;
            transform.position = hitPoint;

            //player.transform.position = hitPoint;


        }

        
        
    }

   
}
