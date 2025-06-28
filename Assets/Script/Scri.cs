using UnityEngine;
using UnityEngine.EventSystems;

public class Scri : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer Up");
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Pointer Click");
    }
}