using UnityEngine;

public class ColumnVariableBreak : MonoBehaviour
{
    enum Type{
        HIT,
        TIME
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boss")
        {
            gameObject.GetComponent<Collider>().isTrigger = false;
            gameObject.AddComponent<Rigidbody>();
            enabled = false;
            //float damageTaken = other.gameObject.GetComponent<DamageOutputScript>().DamageOutput;
            //healthPoints = healthPoints - damageTaken;
            //if (healthPoints <= 0f)
            //{

            //}
        }
    }
}
