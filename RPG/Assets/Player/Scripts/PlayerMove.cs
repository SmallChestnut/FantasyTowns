using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Transform mainCamera;
    public float rotationSpeed = 5;
    public float moveSpeed;
    [Tooltip("加速跑速度")]
    public float runSpeed;
    [Tooltip("无聊时间")]
    public float boringTime = 5;
    [Tooltip("攻击点")]
    public Transform hitPoint;
    [Tooltip("攻击距离")]
    public float hitDistance;
    [Tooltip("力量")]
    public int power;
    [Tooltip("最大连击时间")]
    public float doubleHitMaxTime;
    [HideInInspector]
    public float boringTimeNumber; // 感到无聊的剩余时间
    [HideInInspector]
    public bool isMove = true; // 是否可以移动

    private Rigidbody rig;
    private PlayerAnimation playerAnimation;
    private PlayerHit playerHit;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerHit = GetComponent<PlayerHit>();
        Cursor.lockState = CursorLockMode.Locked;
        boringTimeNumber = boringTime;


        playerHit.SetPlayerHit(hitPoint, hitDistance, power, doubleHitMaxTime);
    }

    private void FixedUpdate()
    {
        if (isMove && playerHit.isEndHit)
            Move();
        else
            playerAnimation.Idle();
    }

    private void Move()
    {
        float tempSpeed;
        // 按下shift加速跑
        if (Input.GetKey(KeyCode.LeftShift))
        {
            tempSpeed = runSpeed + moveSpeed;
        }
        else
        {
            tempSpeed = moveSpeed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            // 人物朝向镜头的方向，Lerp旋转线性插值
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, mainCamera.rotation.eulerAngles.y, 0), Time.deltaTime * rotationSpeed);
            rig.MovePosition(transform.position + transform.forward * tempSpeed * Time.fixedDeltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, mainCamera.rotation.eulerAngles.y + 180, 0), Time.deltaTime * rotationSpeed);
            rig.MovePosition(transform.position + transform.forward * tempSpeed * Time.fixedDeltaTime);

        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, mainCamera.rotation.eulerAngles.y - 90, 0), Time.deltaTime * rotationSpeed);
            rig.MovePosition(transform.position + transform.forward * tempSpeed * Time.fixedDeltaTime);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, mainCamera.rotation.eulerAngles.y + 90, 0), Time.deltaTime * rotationSpeed);
            rig.MovePosition(transform.position + transform.forward * tempSpeed * Time.fixedDeltaTime);

        }
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if (Mathf.Abs(x) + Mathf.Abs(y) != 0)
        {
            // 判断是否是加速跑
            if (tempSpeed == moveSpeed + runSpeed)
            {
                playerAnimation.Run();
            }
            else
            {
                playerAnimation.Walk();
            }
            boringTimeNumber = boringTime;
        }
        else
        {
            if (boringTimeNumber <= 0)
            {
                playerAnimation.Boring();
                boringTimeNumber = boringTime + 3;
            }
            else
            {
                boringTimeNumber -= Time.deltaTime;
                playerAnimation.Idle();
            }


        }
        //if(x > 0)
        //{
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, mainCamera.rotation.eulerAngles.y + 90, 0), Time.deltaTime * rotationSpeed);
        //    rig.MovePosition(transform.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
        //}
        //else if(x < 0)
        //{
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, mainCamera.rotation.eulerAngles.y - 90, 0), Time.deltaTime * rotationSpeed);
        //    rig.MovePosition(transform.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
        //}
        //else if(y > 0)
        //{
        //    // 人物朝向镜头的方向，Lerp旋转线性插值
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, mainCamera.rotation.eulerAngles.y, 0), Time.deltaTime * rotationSpeed);
        //    rig.MovePosition(transform.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
        //}
        //else if(y < 0)
        //{
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, mainCamera.rotation.eulerAngles.y + 180, 0), Time.deltaTime * rotationSpeed);
        //    rig.MovePosition(transform.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(hitPoint.position, hitPoint.position + transform.forward * hitDistance);

    }
}
