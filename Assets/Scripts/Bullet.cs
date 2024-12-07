using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 3f;

    void Start()
    {
        //Usuniêcie po czasie
        Destroy(gameObject, lifetime); 
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other) {
        print("lol");
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Wall")) {
            // Obs³uga kolizji z celem
            Destroy(this.gameObject);
        }

    }
}
