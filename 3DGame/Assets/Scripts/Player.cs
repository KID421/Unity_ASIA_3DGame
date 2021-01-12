using UnityEngine;
using Invector.vCharacterController;    // 引用 套件

public class Player : MonoBehaviour
{
    private float hp = 100;
    private Animator ani;
    /// <summary>
    /// 連擊次數
    /// </summary>
    private int atkCount;

    /// <summary>
    /// 計時器
    /// </summary>
    private float timer;

    [Header("連擊間隔時間"), Range(0, 3)]
    public float interval = 1;
    [Header("攻擊中心點")]
    public Transform atkPoint;
    [Header("攻擊長度"), Range(0f, 5f)]
    public float atkLength;
    [Header("攻擊力"), Range(0, 500)]
    public float atk = 30;

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        Attack();
    }

    /// <summary>
    /// 繪製圖示事件：僅在 Unity 內顯示
    /// </summary>
    private void OnDrawGizmos()
    {
        // 圖示.顏色 = 紅色
        Gizmos.color = Color.red;
        // 圖示.繪製射線(中心點，方向)
        // (攻擊中心點的座標，攻擊中心點的前方 * 攻擊長度)
        Gizmos.DrawRay(atkPoint.position, atkPoint.forward * atkLength);
    }

    /// <summary>
    /// 射線擊中的物件
    /// </summary>
    private RaycastHit hit;

    private void Attack()
    {
        if (atkCount < 3)
        {
            if (timer < interval)
            {
                timer += Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    atkCount++;
                    timer = 0;

                    if (Physics.Raycast(atkPoint.position, atkPoint.forward, out hit, atkLength, 1 << 9))
                    {
                        hit.collider.GetComponent<Enemy>().Damage(atk);
                    }
                }
            }
            else
            {
                atkCount = 0;
                timer = 0;
            }
        }

        if (atkCount == 3) atkCount = 0;

        ani.SetInteger("連擊", atkCount);
    }

    /// <summary>
    /// 受傷
    /// </summary>
    /// <param name="damage">接收的傷害值</param>
    public void Damage(float damage)
    {
        hp -= damage;
        ani.SetTrigger("受傷觸發");

        if (hp <= 0) Dead();
    }

    /// <summary>
    /// 死亡
    /// </summary>
    private void Dead()
    {
        ani.SetTrigger("死亡觸發");
        
        // 鎖定移動與旋轉
        vThirdPersonController vt = GetComponent<vThirdPersonController>();
        vt.lockMovement = true;
        vt.lockRotation = true;
    }
}
