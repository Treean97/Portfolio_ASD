using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // 이동 속도
    public float moveSpeed;
    // 회전 속도
    public float YrotateSpeed;
    // 중력 크기
    public float gravityScale;
    // 점프 높이
    public float jumpPower;

    CharacterController cc;
    Animator anim;

    // dir에 들어갈 y값 관리 임시변수
    float _y;

    // Start is called before the first frame update
    void Start()
    {
        // 재시작 시 게임 멈춤 방지
        Time.timeScale = 1;

        Apply();
        
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 바닥이 없을 때만
        if (!cc.isGrounded)
        {
            // 중력
            _y -= gravityScale * Time.deltaTime;
        }
        // 바닥이 있을 때만 점프
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            _y = jumpPower;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 이동 애니메이션
        anim.SetFloat("speed", cc.velocity.magnitude);

        // 이동할 방향
        Vector3 dir = new Vector3(h, 0, v);

        // 정규화 (= 방향만 남기기)
        dir.Normalize();  // dir = dir.normalized;

        // 플레이어가 바라보는 방향을 기준으로
        dir = transform.TransformDirection(dir);

        // 변경된 y값 할당
        dir.y = _y;

        // 앞,뒤,좌,우로 이동
        cc.Move(dir * moveSpeed * Time.deltaTime);

        // 마우스 좌우 움직임 저장
        float rotY = Input.GetAxis("Mouse X") * YrotateSpeed * Time.deltaTime;

        // 마우스의 움직임만큼 회전
        transform.Rotate(0, rotY, 0);

    }

    void Apply()
    {
        YrotateSpeed = GameManager.instance.xSensi * 10;
    }
}
