using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public LayerMask HitboxMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (ShouldDie())
        {
            transform.gameObject.GetComponent<Renderer>().enabled = false;
            transform.gameObject.GetComponent<Collider>().enabled = false;
        }

    }

    bool ShouldDie()
    {
        return Physics.CheckSphere(transform.position, 1.5f, HitboxMask);
    }


}
