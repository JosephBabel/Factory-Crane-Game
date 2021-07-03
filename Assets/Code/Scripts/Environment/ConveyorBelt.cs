using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    private float speed => GameManager.instance.conveyorSpeed;

    private Renderer rend;

    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        rend.material.SetTextureOffset("_BaseMap", new Vector2(0, speed * Time.time));
        rend.material.SetTextureOffset("_BumpMap", new Vector2(0, speed * Time.time));
    }

    private void OnCollisionStay(Collision collision)
    {
        collision.rigidbody.MovePosition(collision.transform.position + transform.TransformDirection(Vector3.forward) * Time.deltaTime * speed * 6);
    }
}
