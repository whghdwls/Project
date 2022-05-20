using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public AudioSource mySfx;

    public AudioClip stepSfx;


    //�ӵ� ���� ����
    [SerializeField]
    private float walkSpeed;  //�ȴ� �ӵ�
    [SerializeField]
    private float runSpeed;  //�ٴ� �ӵ�
    [SerializeField]
    private float crouchSpeed;  //�ɾƼ� ������ ���� �ӵ�

    private float applySpeed; //�ϳ��� �Լ����� �ȱ� �ٱ⸦ ó���ϱ� ���� ����

    //���� ���� ����
    [SerializeField]
    private float jumpForce; //��� ������ ������ ������ ���ΰ��� ���ϴ� ����


    //���� ����
    private bool isRun = false;  //�ٴ��� �ȴ����� �Ǵ��ϴ� booleanŸ�� ����(true�� �ٱ� false�� �ȱ�)
    //���� ������ ������ �� �� �ְ� �ϰ� ���߿� ������ �� �� ���� ����� ���� ���� ����
    private bool isGround = true;  //player�� ���� �ִ��� ���߿� �ִ��� �Ǵ��ϴ� ����
    private bool isCrouch = false; //�ɾҴ��� �ƴ��� �Ǵ��ϴ� ����

    //�ɾ��� �� �󸶳� ������ �����ϴ� ����
    [SerializeField]
    private float crouchPosY; //���� ���� y���� ��
    private float originPosY; //�ɱ� �� ���� y���� �� 
    private float applyCrouchPosY; //���� �� y���� �����ϴ� ���� ���� ������ ����

    //�� ���� ���ο� ���� ������Ʈ
    //player�� ���� 3d ��ü�� �ظ��� �ٴڿ� ��������� isGround�� true �ƴϸ� false
    private CapsuleCollider capsuleCollider;


    //ī�޶� �ΰ���
    [SerializeField]
    private float lookSensitivity; //ī�޶� �ΰ����� ������ �� �ִ� ����

    //ī�޶� �Ѱ�
    [SerializeField]
    private float cameraRotationLimit; //ī�޶� �̵� ������ ������ ����
    private float currenCameraRotationX = 0; //ī�޶� ���Ʒ� ����

    //�ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCamera;  //ī�޶� ����ϱ� ���� ����
    private Rigidbody myRigid;  //�������� �Ͱ� ���õ� ��Ҹ� ����ϰ� ���ִ� ���

    // Start is called before the first frame update
    void Start()
    {
        //������Ʈ�� ����ϱ� ���ؼ��� �� GetComponent�� ���־�� �Ѵ�.
        capsuleCollider = GetComponent<CapsuleCollider>(); //3d ��ü(player)�� ���� ������Ʈ
        myRigid = GetComponent<Rigidbody>();  //�߷¿� ���� ������Ʈ
        applySpeed = walkSpeed; //�޸��� Ű�� ������ ������ �ȴ� �ӵ��� �����̰� �ϱ� ����
        //player�� �ƴ� ī�޶� ���������ϱ� ������ �̿� ���� �����Ѵ�
        //localPosition�� ����� ������ ī�޶� ��ġ�� player�� ��������� ���� ������ ���Ͽ� ���
        originPosY = theCamera.transform.localPosition.y; //���� y���� ��
        applyCrouchPosY = originPosY;  //�⺻���� originPosY�� ���´�.(���ִ� ����)
    }

    // Update is called once per frame
    void Update()
    {
        IsGround(); //player�� ���� �ִ��� Ȯ���ϴ� �Լ�
        TryJump();
        TryRun(); //�ݵ�� Move�Լ� ���� �־���Ѵ�. -> Move�Լ��� �����ϱ� ���� �� �Լ��� �����ϰ� �ϱ� ����
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
        Stepstart();

    }

    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;  //�Լ��� ����� ������ true�� false�� false�� true�� ���� �ٲپ��ش�.

        if (isCrouch)  //�ɾƼ� �����̴� ���
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else  //���� �����̴� ���
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }

    //coroutine - �Լ����� ���� ó���� ���� ���
    //�÷��̾ ���� �� �ٷ� ī�޶� ������ �̵��Ͽ� ���ڿ������� ���� �����ϱ� ���� ���
    //coroutine�� �⺻���� ����
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;
        while (_posY != applyCrouchPosY) //_posY ���� ���� ���ϴ� ���� �� ������ �ݺ��ϴ� �ݺ���
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)  //15�� ���� �ݺ��ϸ� �ݺ��� Ż��
            {
                break;
            }
            yield return null; //�� ������ ��⸦ �ǹ�
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
    }

    private void IsGround()
    {
        //Physics.Raycast -> player�� Ư�� ��ġ���� �� ���̴� �������� �߻�(Physics Ŭ���� ���)
        //Physics.Raycast�� �̿��� player�� ��ġ�� �ľ�
        //Vector3.down -> Vector3�� ������ ��ǥ down�� Vector3�� �Ʒ��� �ǹ�
        //bounds -> capsuleCollider�� ����
        //extents -> bounds ũ���� ��
        //Raycast(��ġ, �������� ��� ����, ��� ���� �Ÿ��� ����ΰ��� ���� ��)
        //0.1f�� ���,��� ��� ���� ���� �ö󰬴� �������� �� �� ������ �߻��Ͽ� ����� ������ ������� �ִ� ���� �������ִ� ����
        //���鿡 ��������� true ���� ������ false�� ��ȯ 
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    private void TryJump()
    {
        //GetKey�� ������ �ִ� ��쿡�� GetKeyDown�� �� ���� ������ ����
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space)) && isGround)
        {
            Jump();
        }
    }

    private void Jump()
    {
        //velocity -> myRigid�� �ӵ�(player�� ������),velocity�� �Ӽ��̴�.
        //jumpForce�� ����ŭ ���� �������� �����̱� ���� �Ӽ�
        myRigid.velocity = transform.up * jumpForce; //transform.up = (0,1,0)
    }

    private void TryRun()
    {
        //GetKey�� Ű�� ������ ������ true �ƴϸ� false�� ��ȯ
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();  //�ٴ� Ű�� ������ ��� �����ϴ� �Լ�
        }

        //GetKeyUp�� Ű�� ������ ������ true ������ ������ false�� ��ȯ
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKey(KeyCode.DownArrow))
        {
            RunningCancel();  //�ٴ� Ű�� ������ ������ �����ϴ� �Լ�
        }
    }

    private void Running()
    {
        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void Move()
    {
        //ĳ���� �̵�

        //x�� �̵�
        float _moveDirX = Input.GetAxisRaw("Horizontal");

        //z�� �̵�
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;  //�¿� �̵� 
        Vector3 _moveVertical = transform.forward * _moveDirZ;  //�յ� �̵�

        //ĳ������ �̵��� ���Ǵ� ����
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        //����Ƽ���� ĳ���� �̵��� ������ �Լ�
        myRigid.MovePosition(transform.position + _velocity * Time.smoothDeltaTime);//Time.deltaTime);

    }

    private void CameraRotation()
    {
        //���� ī�޶� ȸ��

        //ī�޶�� 3�����̱⿡ ���Ʒ��� x�������� ���콺�� y�̱⿡ �̿� ���� �ۼ�
        float _xRotation = Input.GetAxisRaw("Mouse Y"); //���Ʒ��� ���� ��°�
        float _cameraRotationX = _xRotation * lookSensitivity;
        //currenCameraRotationX += _cameraRotationX;//�̿� ���� �ۼ��ϸ� ���Ʒ��� �ݴ�� ��ȯ�ȴ�

        currenCameraRotationX -= _cameraRotationX; //�̷��� ���Ʒ��� ���������� ��ȯ�ȴ�.
        currenCameraRotationX = Mathf.Clamp(currenCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        //����Ƽ�� �����ϴ� ī�޶� �ޱ� ��ȯ �Լ�
        theCamera.transform.localEulerAngles = new Vector3(currenCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()
    {
        //�¿� ĳ���� ȸ��
        float _yRotation = Input.GetAxisRaw("Mouse X");

        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        //���콺�� �¿�� �����̸� ī�޶�� �Բ� ĳ���͵� �¿�� �����̵��� ����� �Լ�
        //Euler - x, y, z���� �������� ī�޶� ȸ����Ű�� �Լ�
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    public void Stepstart()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (mySfx.isPlaying == false)
                StepSound();

        }

    }
    public void StepSound()
    {
        mySfx.PlayOneShot(stepSfx);
    }
}
