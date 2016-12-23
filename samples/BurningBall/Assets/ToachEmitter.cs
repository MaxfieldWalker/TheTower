using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToachEmitter : MonoBehaviour
{
    public GameObject toach;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var clone = Instantiate(toach, transform.position, transform.rotation);
            clone.GetComponent<Rigidbody>().velocity = new Vector3(
                Random.Range(-10.0f, 10.0f), Random.Range(0.0f, 24.0f), Random.Range(10.0f, 100.0f));
        }
    }
}
