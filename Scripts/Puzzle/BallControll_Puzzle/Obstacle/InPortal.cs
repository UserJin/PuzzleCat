using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class InPortal : MonoBehaviour
{
    public OutPortal outPortal;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball")) return;

        MovePortal(other);
    }

    void MovePortal(Collider collider)
    {
        collider.gameObject.GetComponent<Rigidbody>().MovePosition(outPortal.gameObject.transform.position + collider.transform.forward);// = outPortal.gameObject.transform.position.normalized + collider.transform.forward.normalized;
    }


}
