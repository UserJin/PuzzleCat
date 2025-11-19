using LaserPuzzle;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 레이저 퍼즐의 입력에 관한 매니저
/// 생성과 동시에 입력을 활성화
/// 이후 클리어 하면 비활성화를 호출(그런데 어차피 제거되면서 상관없나?)
/// </summary>
public class LaserPuzzleInputManager : MonoBehaviour
{
    [SerializeField] PlayerInput input;

    public Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        input = GetComponent<PlayerInput>();
        ActivateInput();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            Vector2 point = input.actions["Point"].ReadValue<Vector2>();
            Ray ray = cam.ScreenPointToRay(point);
            RaycastHit hit;

            Physics.Raycast(ray, out hit);
            if(hit.collider != null)
            {
                IInteractable interactable = hit.collider.transform.parent.GetComponent<IInteractable>();
                interactable?.OnClick();
            }
        }
    }

    public void ActivateInput()
    {
        input.ActivateInput();
    }

    public void DeactivateInput()
    {
        input.DeactivateInput();
    }
}
