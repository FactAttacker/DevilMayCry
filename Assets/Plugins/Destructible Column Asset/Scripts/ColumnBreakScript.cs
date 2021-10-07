﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnBreakScript : MonoBehaviour {

	public GameObject unbrokenColumn;
	public GameObject brokenColumn;

	//this determines whether the column will be broken or unbroken at the at runtime
	public bool isBroken;


	void Start()
	{ 
		if (isBroken) {
			BreakColumn ();
		} else {
			unbrokenColumn.SetActive (true);
			brokenColumn.SetActive (false);
		}
	}

    private void OnTriggerEnter(Collider other)
    {
		Debug.Log("1 : "+other.name);
		if (other.gameObject.layer == LayerMask.NameToLayer("Boss"))
		{
			Debug.Log("2 : " + other.name);
			BreakColumn();
		}
    }

    void BreakColumn()
	{
		isBroken = true;
		unbrokenColumn.SetActive (false);
		brokenColumn.SetActive (true);
	}


	void Update()
	{
        //this is a placeholder activation for breaking the column when the space key is pressed
        if (!isBroken)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BreakColumn();
            }
        }
    }
}