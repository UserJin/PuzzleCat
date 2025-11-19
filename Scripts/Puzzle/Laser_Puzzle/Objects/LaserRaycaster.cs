using Game.Common;
using LaserPuzzle;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 레이저를 발사 가능하게 하는 스크립트
/// 레이캐스트로 오브젝트간의 상호작용을 작동시킴
/// 이후 라인렌더러로 레이저를 시각적으로 표시
/// </summary>
public class LaserRaycaster : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Material baseMaterial;

    [SerializeField] private LineRenderer[] lineRenderers;
    [SerializeField] private List<LaserRaycastInfo> laserRaycastInfos = new List<LaserRaycastInfo>();

    public bool isRayCasting = false;
    public int canHitNum = 3; // 최대 반사 횟수
    public int curHitNum = 0; // 현재 반사 횟수

    private LaserPuzzleManager manager;

    private void Awake()
    {
        // Emitter용 라인렌더러
        lineRenderer = GetComponent<LineRenderer>();
        if(lineRenderer != null)
            lineRenderer.material = baseMaterial;

        // LaserReflecter & ColorLens용 라인렌더러 컴포넌트들
        lineRenderers = GetComponentsInChildren<LineRenderer>();
        foreach (LineRenderer lineRenderer in lineRenderers)
        {
            lineRenderer.material = baseMaterial;
        }
    }

    private void Start()
    {
        // 퍼즐 매니저를 찾고 매니저의 이벤트를 구독
        // 이게 정말 하드코딩이란걸 알지만 싱글턴이나 동적생성을 하지 않고 매니저를 찾을 방법 중 가장 빨라서 어쩔 수 없음
        // 이에 관해 고민을 좀 더 해봐야 할 듯
        manager = transform.parent.GetComponentInParent<LaserPuzzleManager>();
        manager.OnObjectChange += ClearAllLaserInfo;
    }

    // Emitter용 단일 레이저 발사 함수
    // 굳이 나눌 필요는 없긴한데 일단 혹시 몰라서 이대로 사용중
    public void CastLaser(Vector3 origin, Vector3 direction, Color color, float maxDistance)
    {
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, origin);
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = Constants.LASER_MAX_WIDTH;
        lineRenderer.endWidth = Constants.LASER_MAX_WIDTH;

        Vector3 currentPos = origin;
        Vector3 currentDir = direction;

        if (Physics.Raycast(currentPos, currentDir, out RaycastHit hit, maxDistance))
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

            ILaserInteractable interactable = hit.collider.transform.parent.GetComponent<ILaserInteractable>();
            if (interactable != null)
            {
                interactable.OnLaserHit(new LaserHitInfo(hit.point, currentDir, hit.normal, color));
            }
        }
        else
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPos + currentDir * maxDistance);
        }
    }

    // Emitter용 레이저 제거 함수
    public void ClearLaser()
    {
        lineRenderer.positionCount = 0;
        isRayCasting = false;
    }

    /// <summary>
    /// 발사해야 할 레이저가 2개 이상일 경우 사용하는 함수
    /// </summary>
    public void CastAllLaser()
    {
        ClearAllLaser();
        if(lineRenderers.Length >= laserRaycastInfos.Count)
        {
            for(int i = 0; i < laserRaycastInfos.Count; i++)
            {
                if (curHitNum > canHitNum+1) return;
                lineRenderers[i].positionCount = 1;
                lineRenderers[i].SetPosition(0, laserRaycastInfos[i].originPos);
                lineRenderers[i].startColor = laserRaycastInfos[i].laserColor;
                lineRenderers[i].endColor = laserRaycastInfos[i].laserColor;
                lineRenderers[i].startWidth = Constants.LASER_MAX_WIDTH;
                lineRenderers[i].endWidth = Constants.LASER_MAX_WIDTH;

                Vector3 currentPos = laserRaycastInfos[i].originPos;
                Vector3 currentDir = laserRaycastInfos[i].raycastDirection;

                curHitNum++;

                // 각 레이저 정보를 바탕으로 Raycast를 실행 및 라인렌더러 렌더링
                if (Physics.Raycast(currentPos, currentDir, out RaycastHit hit, laserRaycastInfos[i].maxDistance))
                {
                    lineRenderers[i].positionCount++;
                    lineRenderers[i].SetPosition(lineRenderers[i].positionCount - 1, hit.point);

                    ILaserInteractable interactable = hit.collider.transform.parent.GetComponent<ILaserInteractable>();
                    if (interactable != null)
                    {
                        interactable.OnLaserHit(new LaserHitInfo(hit.point, currentDir, hit.normal, laserRaycastInfos[i].laserColor));
                    }
                }
                else
                {
                    lineRenderers[i].positionCount++;
                    lineRenderers[i].SetPosition(lineRenderers[i].positionCount - 1, currentPos + currentDir * laserRaycastInfos[i].maxDistance);
                }
            }
        }
    }

    /// <summary>
    /// 제거해야할 레이저가 2개 이상일 때 사용하는 함수
    /// </summary>
    public void ClearAllLaser()
    {
        foreach (LineRenderer line in lineRenderers)
        {
            line.positionCount = 0;
        }
    }

    // 만약 레이저 인포 제거도 필요하면 사용하는 함수
    public void ClearAllLaserInfo()
    {
        ClearAllLaser();
        curHitNum = 0;
        laserRaycastInfos.Clear();
    }

    // 외부에서 레이저 정보를 등록할 때 사용하는 함수
    // 이미 존재하는 정보라면 종료
    public void AddLaserInfo(LaserRaycastInfo info)
    {
        if (laserRaycastInfos.Contains(info))
            return;
        else
            laserRaycastInfos.Add(info);
    }
}
