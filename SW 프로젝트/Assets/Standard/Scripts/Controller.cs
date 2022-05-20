using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public AudioSource mySfx;

    public AudioClip stepSfx;


    //속도 조정 변수
    [SerializeField]
    private float walkSpeed;  //걷는 속도
    [SerializeField]
    private float runSpeed;  //뛰는 속도
    [SerializeField]
    private float crouchSpeed;  //앉아서 움직일 때의 속도

    private float applySpeed; //하나의 함수에서 걷기 뛰기를 처리하기 위한 변수

    //점프 조정 변수
    [SerializeField]
    private float jumpForce; //어느 정도의 힘으로 점프할 것인가를 정하는 변수


    //상태 변수
    private bool isRun = false;  //뛰는지 걷는지를 판단하는 boolean타입 변수(true는 뛰기 false는 걷기)
    //땅에 있으면 점프를 뛸 수 있게 하고 공중에 있으면 뛸 수 없게 만들기 위한 변수 선언
    private bool isGround = true;  //player가 땅에 있는지 공중에 있는지 판단하는 변수
    private bool isCrouch = false; //앉았는지 아닌지 판단하는 변수

    //앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY; //앉은 후의 y축의 값
    private float originPosY; //앉기 전 원래 y축의 값 
    private float applyCrouchPosY; //위의 두 y축을 결정하는 변수 값을 저장할 변수

    //땅 착지 여부에 관한 컴포넌트
    //player로 만든 3d 물체에 밑면이 바닥에 닿아있으면 isGround가 true 아니면 false
    private CapsuleCollider capsuleCollider;


    //카메라 민감도
    [SerializeField]
    private float lookSensitivity; //카메라 민감도를 설정할 수 있는 변수

    //카메라 한계
    [SerializeField]
    private float cameraRotationLimit; //카메라 이동 범위를 설정할 변수
    private float currenCameraRotationX = 0; //카메라 위아래 각도

    //필요한 컴포넌트
    [SerializeField]
    private Camera theCamera;  //카메라를 사용하기 위한 변수
    private Rigidbody myRigid;  //물리적인 것과 관련된 요소를 사용하게 해주는 기능

    // Start is called before the first frame update
    void Start()
    {
        //컴포넌트를 사용하기 위해서는 꼭 GetComponent를 써주어야 한다.
        capsuleCollider = GetComponent<CapsuleCollider>(); //3d 물체(player)에 관한 컴포넌트
        myRigid = GetComponent<Rigidbody>();  //중력에 관한 컴포넌트
        applySpeed = walkSpeed; //달리기 키를 누르기 전에는 걷는 속도로 움직이게 하기 위해
        //player가 아닌 카메라가 움직여야하기 때문에 이와 같이 선언한다
        //localPosition을 사용한 이유는 카메라 위치를 player와 상대적으로 값을 가져기 위하여 사용
        originPosY = theCamera.transform.localPosition.y; //현재 y축의 값
        applyCrouchPosY = originPosY;  //기본값을 originPosY로 갖는다.(서있는 상태)
    }

    // Update is called once per frame
    void Update()
    {
        IsGround(); //player가 땅에 있는지 확인하는 함수
        TryJump();
        TryRun(); //반드시 Move함수 위에 있어야한다. -> Move함수가 동작하기 전에 이 함수가 동작하게 하기 위해
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
        isCrouch = !isCrouch;  //함수가 실행될 때마다 true는 false로 false는 true로 값을 바꾸어준다.

        if (isCrouch)  //앉아서 움직이는 경우
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else  //서서 움직이는 경우
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }

    //coroutine - 함수와의 병렬 처리를 위한 방법
    //플레이어가 앉을 때 바로 카메라가 밑으로 이동하여 부자연스러운 것을 방지하기 위해 사용
    //coroutine의 기본적인 형태
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;
        while (_posY != applyCrouchPosY) //_posY 변수 값이 원하는 값이 될 때까지 반복하는 반복문
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)  //15번 정도 반복하면 반복문 탈출
            {
                break;
            }
            yield return null; //한 프레임 대기를 의미
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
    }

    private void IsGround()
    {
        //Physics.Raycast -> player의 특정 위치에서 안 보이는 레이저를 발사(Physics 클래스 사용)
        //Physics.Raycast를 이용해 player의 위치를 파악
        //Vector3.down -> Vector3는 고정된 좌표 down은 Vector3의 아래를 의미
        //bounds -> capsuleCollider의 영역
        //extents -> bounds 크기의 반
        //Raycast(위치, 레이저를 쏘는 방향, 어느 정도 거리로 쏠것인가에 대한 수)
        //0.1f는 계단,경사 등과 같은 곳을 올라갔다 내려갔다 할 때 오차가 발생하여 생기는 오류가 생길수도 있는 것을 방지해주는 역할
        //지면에 닿아있으면 true 닿지 않으면 false를 반환 
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    private void TryJump()
    {
        //GetKey는 누르고 있는 경우에만 GetKeyDown은 한 번만 누르면 동작
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space)) && isGround)
        {
            Jump();
        }
    }

    private void Jump()
    {
        //velocity -> myRigid의 속도(player의 움직임),velocity는 속성이다.
        //jumpForce의 값만큼 위의 방향으로 움직이기 위한 속성
        myRigid.velocity = transform.up * jumpForce; //transform.up = (0,1,0)
    }

    private void TryRun()
    {
        //GetKey는 키를 누르고 있으면 true 아니면 false를 반환
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();  //뛰는 키를 누르는 경우 동작하는 함수
        }

        //GetKeyUp은 키를 누르지 않으면 true 누르고 있으면 false를 반환
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKey(KeyCode.DownArrow))
        {
            RunningCancel();  //뛰는 키를 누르지 않으면 동작하는 함수
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
        //캐릭터 이동

        //x축 이동
        float _moveDirX = Input.GetAxisRaw("Horizontal");

        //z축 이동
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;  //좌우 이동 
        Vector3 _moveVertical = transform.forward * _moveDirZ;  //앞뒤 이동

        //캐릭터의 이동에 사용되는 변수
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        //유니티에서 캐릭터 이동을 시켜줄 함수
        myRigid.MovePosition(transform.position + _velocity * Time.smoothDeltaTime);//Time.deltaTime);

    }

    private void CameraRotation()
    {
        //상하 카메라 회전

        //카메라는 3차원이기에 위아래가 x값이지만 마우스는 y이기에 이와 같이 작성
        float _xRotation = Input.GetAxisRaw("Mouse Y"); //위아래로 고개를 드는것
        float _cameraRotationX = _xRotation * lookSensitivity;
        //currenCameraRotationX += _cameraRotationX;//이와 같이 작성하면 위아래가 반대로 전환된다

        currenCameraRotationX -= _cameraRotationX; //이러면 위아래가 정상적으로 변환된다.
        currenCameraRotationX = Mathf.Clamp(currenCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        //유니티가 제공하는 카메라 앵글 전환 함수
        theCamera.transform.localEulerAngles = new Vector3(currenCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()
    {
        //좌우 캐릭터 회전
        float _yRotation = Input.GetAxisRaw("Mouse X");

        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        //마우스를 좌우로 움직이면 카메라와 함께 캐릭터도 좌우로 움직이도록 만드는 함수
        //Euler - x, y, z축을 기준으로 카메라를 회전시키는 함수
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
