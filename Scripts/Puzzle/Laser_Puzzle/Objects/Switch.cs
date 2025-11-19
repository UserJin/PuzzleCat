using UnityEngine;

/// <summary>
/// 스위치를 통해 장애물을 제거 가능
/// </summary>
public class Switch : MonoBehaviour, LaserPuzzle.IInteractable
{
    public bool isActive = false;
    public Color activatedColor = Color.red;
    public Color deactivatedColor = Color.gray;

    public Material material;
    public GameObject obstacle;

    public Transform activePos;
    public Transform deactivePos;

    private LaserPuzzleManager manager;

    private void Start()
    {
        material = GetComponentInChildren<MeshRenderer>().material;
        material.color = isActive ? activatedColor : deactivatedColor;

        obstacle = transform.parent.GetChild(1).gameObject;
        if(activePos == null || deactivePos == null )
        {
            obstacle.transform.localPosition = isActive ? new Vector3(obstacle.transform.localPosition.x, 0, obstacle.transform.localPosition.z) :
                                new Vector3(obstacle.transform.localPosition.x, -0.9f, obstacle.transform.localPosition.z);
        }
        else
        {
            obstacle.transform.localPosition = isActive ? activePos.localPosition : deactivePos.localPosition;
        }


            manager = transform.parent.GetComponentInParent<LaserPuzzleManager>();

    }

    public void OnClick()
    {
        ToggleSwitch();
    }

    private void ToggleSwitch()
    {
        isActive = !isActive;
        material.color = isActive ? activatedColor : deactivatedColor;
        if(activePos == null || deactivePos == null)
        {
            obstacle.transform.localPosition = isActive ? new Vector3(obstacle.transform.localPosition.x, 0, obstacle.transform.localPosition.z) :
                                new Vector3(obstacle.transform.localPosition.x, -0.9f, obstacle.transform.localPosition.z);
        }
        else
        {
            obstacle.transform.localPosition = isActive ? activePos.localPosition : deactivePos.localPosition;
        }

        manager.RecalculateLaser();
    }
}
