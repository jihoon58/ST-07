using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField]
    private float bulletSpeed;
    
    [SerializeField]
    private float bulletDamage = 10f;
    
    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 총알이 바라보는 방향(오른쪽)으로 발사
        rb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Zombie"))
        {
            // ZombieAI 컴포넌트 찾기
            var zombieAI = collision.gameObject.GetComponent<ST07.Enemies.ZombieAI>();
            if (zombieAI != null)
            {
                // 데미지 적용
                zombieAI.OnDamaged(bulletDamage);
            }
            
            // 총알 제거
            Destroy(gameObject);
        }
    }
} 
