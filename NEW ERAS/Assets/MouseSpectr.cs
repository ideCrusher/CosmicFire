using UnityEngine;
using UnityEngine.EventSystems;

public class MouseSpectr : MonoBehaviour, IPointerEnterHandler
{
    private Vector3 _ShipTarget;

    public void OnPointerEnter(PointerEventData eventData)
    {      
        _ShipTarget = gameObject.transform.position;       
        GameObject.Find("StarShipPlayer").GetComponent<ShipStats>().Target = _ShipTarget;
    }   
}
