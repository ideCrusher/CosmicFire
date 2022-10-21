using UnityEngine;
using UnityEngine.EventSystems;

public class MouseSpectr : MonoBehaviour, IPointerEnterHandler
{
    private GameObject _ShipTarget;

    public void OnPointerEnter(PointerEventData eventData)
    {      
        _ShipTarget = gameObject;       
        GameObject.Find("StarShipPlayer").GetComponent<ShipStats>().Target = _ShipTarget;
    }   
}
