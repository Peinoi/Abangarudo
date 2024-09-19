using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float lifeTime = 2f;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);

        // ÀÏÁ¤ ½Ã°£ ÈÄ ÃÑ¾Ë ÆÄ±«
        Destroy(gameObject, lifeTime);
    }


    private void OnEnable()
    {
        Invoke("Delete", 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            Destroy(gameObject);
            Debug.Log("Bullet hit " + collision.gameObject.name);
        }else if (collision.gameObject.tag.Equals("Enemy"))
        {
            Destroy(gameObject);

            Debug.Log("Bullet hit " + collision.gameObject.name);
        }
        else if (collision.gameObject.tag.Equals("Player"))
        {
            Destroy(gameObject);

            Debug.Log("Bullet hit " + collision.gameObject.name);
        }
    }



    void Delete()
    {
        Destroy(gameObject);
    }
}
