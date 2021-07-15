using UnityEngine;

/// <summary>
/// Moves texture to create illusion of movement and adds force to touching objects.
/// </summary>
public class ConveyorBelt : MonoBehaviour
{
    private Renderer rend;

    // Get GameManager difficulty settings
    private float speed => GameManager.instance.conveyorSpeed;

    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        // Move both base texture and bump map
        rend.material.SetTextureOffset("_BaseMap", new Vector2(0, speed * Time.time));
        rend.material.SetTextureOffset("_BumpMap", new Vector2(0, speed * Time.time));
    }

    private void OnCollisionStay(Collision collision)
    {
        collision.rigidbody.MovePosition(collision.transform.position + transform.TransformDirection(Vector3.forward) * Time.deltaTime * speed * 6f);
    }
}
