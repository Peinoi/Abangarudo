using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Ground") || collision.gameObject.tag.Equals("Enemy"))
        {
            Destroy(this.gameObject);
            Debug.Log("Bullet");
        }

    }
}
