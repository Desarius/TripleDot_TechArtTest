using UnityEngine;
using UnityEngine.EventSystems;


    public class BottomButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        //On Touch Down
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("OnPointerDown");
        
        }

        //On Touch Up
        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("OnPointerUp");
        }

        //On Tap
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("OnPointerClick");
        }
    }

