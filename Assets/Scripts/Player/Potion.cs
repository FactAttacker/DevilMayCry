using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{

    public Transform[] potionPosition;


    private void Start()
    {
        int point = Random.Range(0,4);
        transform.position = potionPosition[point].position;
        print(point);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            other.gameObject.GetComponent<PlayerState>().TakeDamage = -1000f;

            gameObject.SetActive(true);
        }
    }
}
