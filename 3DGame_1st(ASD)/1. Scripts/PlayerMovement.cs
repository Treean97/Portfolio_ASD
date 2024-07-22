using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // �̵� �ӵ�
    public float moveSpeed;
    // ȸ�� �ӵ�
    public float YrotateSpeed;
    // �߷� ũ��
    public float gravityScale;
    // ���� ����
    public float jumpPower;

    CharacterController cc;
    Animator anim;

    // dir�� �� y�� ���� �ӽú���
    float _y;

    // Start is called before the first frame update
    void Start()
    {
        // ����� �� ���� ���� ����
        Time.timeScale = 1;

        Apply();
        
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // �ٴ��� ���� ����
        if (!cc.isGrounded)
        {
            // �߷�
            _y -= gravityScale * Time.deltaTime;
        }
        // �ٴ��� ���� ���� ����
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            _y = jumpPower;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // �̵� �ִϸ��̼�
        anim.SetFloat("speed", cc.velocity.magnitude);

        // �̵��� ����
        Vector3 dir = new Vector3(h, 0, v);

        // ����ȭ (= ���⸸ �����)
        dir.Normalize();  // dir = dir.normalized;

        // �÷��̾ �ٶ󺸴� ������ ��������
        dir = transform.TransformDirection(dir);

        // ����� y�� �Ҵ�
        dir.y = _y;

        // ��,��,��,��� �̵�
        cc.Move(dir * moveSpeed * Time.deltaTime);

        // ���콺 �¿� ������ ����
        float rotY = Input.GetAxis("Mouse X") * YrotateSpeed * Time.deltaTime;

        // ���콺�� �����Ӹ�ŭ ȸ��
        transform.Rotate(0, rotY, 0);

    }

    void Apply()
    {
        YrotateSpeed = GameManager.instance.xSensi * 10;
    }
}
