using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BottomButtomData : MonoBehaviour
{

    public GameObject ButtonTexture;

    public GameObject CenterPoint;

    public GameObject ButtonText;
    
    public UnityEvent OnClickAction;
    public UnityEvent OnHoverAction;
    public UnityEvent OnHoverExitAction;
    
    private Collider buttonCollider;

    // Start is called before the first frame update
    void Start()
    {
        if(ButtonTexture != null)
        {
            buttonCollider = ButtonTexture.GetComponent<Collider>();
            if (buttonCollider == null)
            {
                Debug.LogError("ButtonTexture does not have a Collider component.");
            }
        }
        else
        {
            Debug.LogError("ButtonTexture is not assigned.");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //get the event from collider in Button Texture
    
    
    
}
