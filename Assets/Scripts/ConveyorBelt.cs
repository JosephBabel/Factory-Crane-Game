using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] float speed = 0.5f;

    private Renderer rend;

    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, speed * Time.time));
        rend.material.SetTextureOffset("_BumpMap", new Vector2(0, speed * Time.time));
    }

    private void OnCollisionStay(Collision collision)
    {
        collision.rigidbody.MovePosition(collision.transform.position + transform.TransformDirection(Vector3.forward) * Time.deltaTime * speed * 6);
    }
}
