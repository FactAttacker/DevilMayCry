using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnVariableBreakScript : MonoBehaviour
{
    [SerializeField]
    float healthPoints = 100f; //this value can be changed in the inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Damaging")
        {
            float damageTaken = other.gameObject.GetComponent<DamageOutputScript>().DamageOutput;
            healthPoints = healthPoints - damageTaken;
            if (healthPoints <= 0f)
            {
                gameObject.GetComponent<Collider>().isTrigger = false;
                gameObject.AddComponent<Rigidbody>();
                this.enabled = false;
            }
        }
    }
}