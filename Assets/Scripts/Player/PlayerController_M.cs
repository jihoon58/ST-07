using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController_M : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
     
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    public void Movement()
    {
        float vx = Input.GetAxisRaw("Horizontal");
        float vy = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(vx * speed, vy * speed);

        rb.linearVelocity = movement;
    }
}
