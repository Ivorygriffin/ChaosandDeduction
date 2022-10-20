using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float speed;
    public float damage;
    public float aliveTime;

    public Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        body.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = transform.position + transform.forward * speed * Time.deltaTime;

        aliveTime -= Time.deltaTime;
        if (aliveTime <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
