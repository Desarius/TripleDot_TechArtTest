using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;

public class AnimatedButtonManager : MonoBehaviour
{
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private float scale = 1.15f;
    [SerializeField] private bool adjustLayout = true;
    
    private AnimatedMenuButton selected;
    private HorizontalLayoutGroup layout;
    private float originalSpacing;
    
    private void Awake()
    {
        layout = GetComponent<HorizontalLayoutGroup>();
        if (layout) originalSpacing = layout.spacing;
        
        foreach (var btn in GetComponentsInChildren<AnimatedMenuButton>())
            btn.Setup(this, duration, scale);
    }
    
    public void OnClick(AnimatedMenuButton button)
    {
        bool deselecting = (selected == button);
        
        selected?.SetSelected(false);
        selected = deselecting ? null : button;
        selected?.SetSelected(true);
        
        UpdateLayout();
    }
    
    public void DeselectAll()
    {
        selected?.SetSelected(false);
        selected = null;
        UpdateLayout();
    }
    
    private void UpdateLayout()
    {
        if (!adjustLayout || !layout) return;
        
        float target = selected ? originalSpacing * scale : originalSpacing;
        DOTween.To(() => layout.spacing, x => {
            layout.spacing = x;
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }, target, duration);
    }
}