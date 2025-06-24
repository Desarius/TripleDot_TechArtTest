using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BottomBarView : MonoBehaviour
{
    public GameObject BottomButtonPrefab; 
    public List<ScriptableObject> buttons;
    
    private bool isGenerated = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( !isGenerated && Application.isPlaying )
        {
            GenerateButtons();
            isGenerated = true;
        }
    }
    
    // Generates buttons based on the ScriptableObjects in the buttons list
    private void GenerateButtons()
    {
        foreach (var button in buttons)
        {
            if (button is BottomButtonSO bottomButton)
            {
                GameObject newButton = Instantiate(BottomButtonPrefab, transform);
               // newButton.GetComponentInChildren<Text>().text = bottomButton.buttonName;
               // newButton.GetComponent<Image>().sprite = bottomButton.buttonIcon;
                newButton.SetActive(bottomButton.isVisible);
                
                // You can add more properties and functionality here as needed
            }
            else
            {
                Debug.LogWarning("The provided ScriptableObject is not a BottomButtonSO.");
            }
        }
    }

    //in editor mode, we can add buttons to the bottom bar
    public void AddButton(ScriptableObject button)
    {
        if (button is BottomButtonSO bottomButton)
        {
            buttons.Add(bottomButton);
            // Here you can also instantiate a UI button and set its properties based on the BottomButtonSO
            // For example:
            // GameObject newButton = Instantiate(buttonPrefab, transform);
            // newButton.GetComponentInChildren<Text>().text = bottomButton.buttonName;
            // newButton.GetComponent<Image>().sprite = bottomButton.buttonIcon;
        }
        else
        {
            Debug.LogWarning("The provided ScriptableObject is not a BottomButtonSO.");
        }
    }
    
    
}
