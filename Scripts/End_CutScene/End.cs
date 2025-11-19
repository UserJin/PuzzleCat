using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class End : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public Animator Door;
    public TextMeshPro txt;
    public GameObject cut;
    [SerializeField] float zoomDuration = 3f;
    [SerializeField] float targetFov = 50f;


    private void Awake()
    {
        GameManager.Instance.End += Camset;
    }

    private void Camset()
    {
        cam.gameObject.SetActive(true);

        StartCoroutine(ZoomInDoor());
    }

    private IEnumerator ZoomInDoor()
    {
        float currentTime = 0f;
        float startFov = cam.m_Lens.FieldOfView;

        while (currentTime < zoomDuration)
        {
            currentTime += Time.deltaTime;

            float nowFov = Mathf.Lerp(startFov, targetFov, currentTime / zoomDuration);
            cam.m_Lens.FieldOfView = nowFov;

            yield return null;
        }

        cam.m_Lens.FieldOfView = targetFov;

        DoorOpen();

        yield return new WaitForSeconds(3f);

        txt.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);
        cut.SetActive(true);
    }

    private void DoorOpen()
    {
        Door.Play("Opening 1");
    }

}
