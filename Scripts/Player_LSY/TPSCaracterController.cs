using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCaracterController : MonoBehaviour
{
    [SerializeField] private Transform CharacterBody;
    // 캐릭터의 몸체를 나타내는 Transform 컴포넌트입니다.
    private Vector2 curMovementInput; // 현재 이동 입력을 저장하는 변수입니다. Vector2는 2차원 벡터로, x축과 y축의 이동 값을 나타냅니다.
    [SerializeField] private Transform cameraContainer;
    // 카메라를 담고 있는 컨테이너의 Transform 컴포넌트입니다. 이 컨테이너는 카메라의 회전을 관리합니다.
    private float camCurXRot; // 카메라의 현재 x축 회전을 저장하는 변수입니다. 이 값은 카메라의 수직 회전을 제한하는 데 사용됩니다.
    private float camCurYRot;
    public float lookSensitivity = 0.1f; // 민감도
    private Vector2 mouseDelta; // 마우스에 이전과 현제 이동 값의차의를 나타내는 변수.
    private Vector3 cameraAngle; // 카메라의 회전 값을 저장하는 변수입니다. 이 값은 카메라의 회전을 계산하는 데 사용됩니다.

    private float curMouseX;
    // 현재 마우스의 수평 이동 값을 저장하는 변수입니다.
    private float curMouseY;
    // 현재 마우스의 수직 이동 값을 저장하는 변수입니다.
    private float curMoveX;
    // 현재 캐릭터의 수평 이동 값을 저장하는 변수입니다.
    private float curMoveY;
    // 현재 캐릭터의 수직 이동 값을 저장하는 변수입니다.

    Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        LookAround(); // LookAround 메서드를 반복 호출하여 카메라 회전을 처리합니다.
        Move();
        // Move 메서드를 반복 호출하여 캐릭터의 이동을 처리합니다.
    }

    private void Move() // 캐릭터의 이동을 처리하는 메서드입니다.
    {
        // 이동 기능을 구현하는 코드 작성하기.
        curMoveX = Input.GetAxis("Horizontal"); // 수평 이동 입력을 가져옵니다.
        curMoveY = Input.GetAxis("Vertical"); // 수직 이동 입력을 가져옵니다.
        Vector2 moveInput = new Vector2(curMoveX, curMoveY);
        // 변화하는 이동 값을 저장할 Vector2를 만듭니다. 안에는 수평 및 수직 이동 값이 들어갑니다.

        bool isMove = moveInput.magnitude != 0f;
        // 이동 입력의 크기가 0이 아닌 경우 이동 중임을 나타내는 bool 변수를 만듭니다. (길리가 0이면 이동 입력을 받고있지 않는 겁니다.)
        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraContainer.forward.x, 0f, cameraContainer.forward.z).normalized;
            // 카메라의 전방 방향을 캐릭터가 바라보는 방향으로 저장합니다.
            Vector3 lookRight = new Vector3(cameraContainer.right.x, 0f, cameraContainer.right.z).normalized;
            // 카메라의 오른쪽 방향을 캐릭터가 바라보는 방향으로 저장합니다.
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
            // lookForward와 lookRight를 moveInput에 곱하고 더하면 바라보고 있는 방향을 기준으로 이동 방향을 구할 수 있습니다.
            // moveInput.y는 수직 이동을, moveInput.x는 수평 이동을 나타냅니다.


            // 캐릭터가 바라보는 방향을 정하는 방법:
            // 1.캐릭터가 움직일 때 시선을 고정 시키는 방법:
            CharacterBody.forward = lookForward;

            // 2. 캐릭터가 카메라와 같은 정면을 바라보지않고 이동해야하는 방향으로 바라보는 방법:
            //CharacterBody.forward = moveDir;

            //var angle = CharacterBody.eulerAngles;
            //for (var i = 0f; i <= 1f; i += Time.deltaTime)
            //{
            //    CharacterBody.eulerAngles = Vector3.Lerp(angle, cameraContainer.rotation.eulerAngles, i);
            //}

            transform.position += moveDir * Time.deltaTime * 5f;
            // 캐릭터의 위치를 이동 방향으로 업데이트합니다. Time.deltaTime을 곱하여 프레임 독립적인 이동을 보장합니다.
            // 5f는 이동 속도를 나타냅니다. 이 값을 조정하여 캐릭터의 이동 속도를 변경할 수 있습니다.
        }


        Debug.DrawRay(cameraContainer.position, new Vector3(cameraContainer.forward.x, 0f, cameraContainer.forward.z).normalized, Color.red);
        // 카메라의 전방 방향을 시각적으로 나타내는 레이저를 그립니다.
        // DrawRay는 Unity에서 디버깅용으로 사용되는 메서드로, 주어진 위치에서 주어진 방향으로 레이저를 그립니다.
        // cameraContainer.position은 레이저의 시작 위치를 나타내고
        // Color.red는 색갈을 설정합니다
        // 두번째는 레이저의 방향을 나타내며, cameraContainer.forward는 카메라의 전방 방향을 나타냅니다.
        // (이렇게 하면 카메라가 바라보는 방얗으로 레이저가 나가지만 위아래도 같이 움직입니다.)
        // new Vector3(cameraContainer.forward.x, 0f, cameraContainer.forward.z).normalized 를 쓴 이유는
        // 카메라의 전방 방향에서 y축(수직 방향)을 제거하여 수평 평면에서의 방향을 나타내기 위함입니다.
        // 즉 new Vector3에 y 값만 없에고 x와 z값을 forward로 가져와서 수평 평면에서의 방향만 나오게 합니다.
        // normalized는 벡터의 길이를 1로 맞춰줍니다.
    }
    private void LookAround() // 카메라 회전을 처리하는 메서드입니다.
    {
        curMouseX = Input.GetAxis("Mouse X"); // 마우스의 수평 이동 입력을 가져옵니다.
        curMouseY = Input.GetAxis("Mouse Y"); // 마우스의 수직 이동 입력을 가져옵니다.
        
        Vector2 mouseDelta = new Vector2(curMouseX, curMouseY);
        // 마우스 델타를 저장할 Vector2를 만듭니다. 안에는 마우스 수평 및 수직 이동 값이 들어갑니다.
        // 프로그래밍에서는 "델타"라는 용어가 변화량을 나타내는 데 사용됩니다. (이전과 현제 값의 차이를 나타낸다.)


        Vector3 camAngle = cameraContainer.rotation.eulerAngles;
        // camAngl이라는 Vector3 값에 cameraContainer의 회전 값을 저장합니다.
        // rotation은 4차원 값으로 인간이 이해하기 쉬운 x, y, z 축의 회전 값을 나타내기 위해 eulerAngle을 사용해 변환합니다.
        // 최종 결과: camAngle.x = 위아래 각도, camAngle.y = 좌우 각도, camAngle.z = 기울기
        // 마지막으로 camAngle 과 mouseDelta를 합쳐 새로운 회전값을 만들어 줍니다.

        //위아래의 각도에 제한을 주기위해 camAngle.x - mouseDelta.y를 변수를 만들어 저장해 주고 조건을 걸어서 제한합니다
        float x = camAngle.x - mouseDelta.y;
        if (x < 180) // camAngle.x가 180도보다 작으면 위쪽으로 회전하는 것
        {
            x = Mathf.Clamp(x, -1f, 70f); 
            // -1f는 위로 회전할 수 있는 최소값, 70f는 위로 회전할 수 있는 최대값입니다.
        }
        else // camAngle.x가 180도보다 크면 아래쪽으로 회전하는 것
        {
            x = Mathf.Clamp(x, 335f, 361f);
            // 335f는 아래로 회전할 수 있는 최소값, 361f는 아래로 회전할 수 있는 최대값입니다.
        }

        // 그리고 새로만들어진 회전값을 cameraContainer.rotation에 넣어줍니다.
        cameraContainer.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, 0);
        // camAngle.y는 3차원에서는 수평을담당하고 mouseDelta.x 는 2차원에서 수평 이동을 담당합니다.
        // 따라서 camAngle.y + mouseDelta.x 는 수평 회전을 담당합니다. 즉 mouseDelta.x 마우가 좌우로 움직이는 실시간 변화하는 값을 
        // camAngle.y에 더해줍니다.
        // camAngle.x와 mouseDelta.y도 같은 원리로 위아래 방향을 답당합니다.
    }

    void CameraLook()
    {
        // 카메라의 회전을 처리하는 메서드입니다. (회전만 관리합니다)

        //camCurXRot += mouseDelta.y * lookSensitivity;
        //camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        //cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        //transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);

        // mouseDelta는 InputAction.CallbackContext에서 마우스의 수직 수평 이동 값을 가지고 있습니다.
        // 이제 카메라의 회전 값을 구해서 이동값과 회전값으로 새로운 카메라 회전 값을 만들어 줍니다.
        cameraAngle = cameraContainer.rotation.eulerAngles;
        // cameraContainer의 회전 값을 eulerAngles로 변환하여 cameraAngle에 저장합니다.
        // eulerAngles는 3차원 회전을 표현하는 방법 중 하나로, x, y, z 축의 회전 값을 나타냅니다.
        // cameraAngle.x는 위아래 각도, cameraAngle.y는 좌우 각도, cameraAngle.z는 기울기를 나타냅니다.
        // rotation은 4차원 값으로 인간이 이해하기 쉬운 x, y, z 축의 회전 값을 나타내기 위해 eulerAngle을 사용해 변환합니다.

        camCurYRot = cameraAngle.y + mouseDelta.x * lookSensitivity;

        // 카메라는 캐릭터를 중심으로 회전하는데 좌우의 회전을 제한를 주지않아도 되지만, 외아래는 제한이 필요합니다
        // 그래서 편수 float x에 cameraAngle.x - mouseDelta.y를 저장하고 if문을 사용해 제한을 걸어줍니다.
        camCurXRot = cameraAngle.x - mouseDelta.y * lookSensitivity; // 수직 회전 값 계산

        if (camCurXRot < 180) // camAngle.x가 180도보다 작으면 위쪽으로 회전하는 것
        {
            camCurXRot = Mathf.Clamp(camCurXRot, -1f, 70f);
            // -1f는 위로 회전할 수 있는 최소값, 70f는 위로 회전할 수 있는 최대값입니다.
        }
        else // camAngle.x가 180도보다 크면 아래쪽으로 회전하는 것
        {
            camCurXRot = Mathf.Clamp(camCurXRot, 335f, 361f);
            // 335f는 아래로 회전할 수 있는 최소값, 361f는 아래로 회전할 수 있는 최대값입니다.
        }

        // 이제 cameraContainer의 새로운 회전 값을 계산해 봅시다.
        cameraContainer.rotation = Quaternion.Euler(camCurXRot, camCurYRot, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            animator.SetBool("isWalking", true);
            curMovementInput = context.ReadValue<Vector2>();
            // 키보드에 입력되는 값을 읽어서 curMovementInput 변수에 저장합니다.

            // 기존에 키보드 입력 값을 읽어오는 방법은 다음과 같았습니다:

            // float curMoveX = Input.GetAxis("Horizontal"); // 수평 이동 입력을 가져옵니다.
            // float curMoveY = Input.GetAxis("Vertical"); // 수직 이동 입력을 가져옵니다.
            // Vector2 curMovementInput = new Vector2(curMoveX, curMoveY);
            // 변화하는 이동 값을 저장할 Vector2를 만듭니다. 안에는 수평 및 수직 이동 값이 들어갑니다.
            // 하지만 Input 시스템을 사용하면 변수를 따라 선언해서 입력 값을 읽어올 필요가 없이 
            // context.ReadValue<Vector2>();를 이용해 curMovementInput에 저장해 줍니다. 
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            animator.SetBool("isWalking", false);
            curMovementInput = Vector2.zero;
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
        // 마우스 이동 값을 읽어와 mouseDelta 변수에 저장합니다.

        // 기존에 마우스 이동 값을 읽어오는 방법은 다음과 같았습니다:

        // float curMouseX = Input.GetAxis("Mouse X"); // 마우스의 수평 이동 입력을 가져옵니다.
        // float curMouseY = Input.GetAxis("Mouse Y"); // 마우스의 수직 이동 입력을 가져옵니다.
        // Vector2 mouseDelta = new Vector2(curMouseX, curMouseY);
        // 마우스 델타를 저장할 Vector2를 만듭니다. 안에는 마우스 수평 및 수직 이동 값이 들어갑니다.
        // 프로그래밍에서는 "델타"라는 용어가 변화량을 나타내는 데 사용됩니다. (이전과 현제 값의 차이를 나타낸다.)
        // 하지만 Input 시스템을 사용하면 변수를 따라 선언해서 마우스의 이동 값을 읽어올 필요가 없이 
        // context.ReadValue<Vector2>();를 이용해 mouseDelta에 저장해 줍니다.

    }
}

// Quaternion.LookRotation
// Quaternion.RotateTowards(transform.rotation, target, turnSpeed * Time.deltaTime);
