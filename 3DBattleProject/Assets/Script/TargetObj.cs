using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObj : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            Debug.Log(this.name);
            this.gameObject.SetActive(false);
        }
    }
}
