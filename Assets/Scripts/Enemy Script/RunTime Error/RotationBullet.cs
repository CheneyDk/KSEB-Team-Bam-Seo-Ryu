using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBullet : MonoBehaviour
{
    // like brotato normal boss.
    // bullets are spread by time.
    // rotate in the same line and same orbit.

    public float bulletDamage = 30f;
    public float bulletMoveSpeed = 1f;
    public Vector3 direction;
    public float waitTime;
    private float timer = 0f;

    public void Start()
    {
        SetBulletPos().Forget();
    }

    public void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(direction * Time.deltaTime);
    }

    // - 30 ~ -10 , 10 ~ 30: bullets range. num: 3 ~ 4 bullet
    // if not enough add more bullets
    public void Init(Vector2 vec, float time)
    {
        direction = vec;
        waitTime = time;
    }

    private async UniTask SetBulletPos()
    {
        await UniTask.WaitForSeconds(waitTime);

        direction = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();

            if (player.playerCurHp <= 0)
            {
                player = null;
                return;
            }
            player.TakeDamage(bulletDamage);
        }
    }
}
