using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BottomButton", menuName = "ScriptableObjects/CreateBottomButtom", order = 1)]
public class BottomButtonSO : ScriptableObject
{
    public string buttonName;
    public Sprite buttonIcon;
    public bool isActive;
    public bool isLocked;  
    public bool isVisible;
}
