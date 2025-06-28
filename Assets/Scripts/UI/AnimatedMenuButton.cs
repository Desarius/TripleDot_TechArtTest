
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;

public class AnimatedMenuButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Components")]
    public Button button;
    public Image fadeImage;
    public TextMeshProUGUI text;
    public Transform centerPoint;
    
    [Header("Settings")]
    public string buttonText = "Button";
    [SerializeField] private bool disabled = false; // Torna SerializeField per Inspector control
    public Sprite enabledSprite, disabledSprite;
    
    [Header("Animation")]
    public float moveDistance = 20f;
    public float textMoveDistance = 15f;
    public float imageScale = 1.2f;

    public UnityEvent OnClick;
    public UnityEvent OnClosed;
    
    private AnimatedButtonManager manager;
    private bool selected;
    private float duration, selectedScale;
    private bool lastDisabledState; // Per controllare cambiamenti nell'Inspector
    
    // Cached components and original values
    private RectTransform rect;
    private LayoutElement layoutElement;
    private Image centerImage;
    private Vector3 originalScale, centerOriginalPos, centerOriginalScale;
    private Vector3 textOriginalPos, imageOriginalScale;
    private Vector2 originalLayoutSize;
    private Sequence animation;
    
    private void Awake()
    {
        CacheComponents();
        CacheOriginalValues();
        InitializeStates();
    }
    
    private void CacheComponents()
    {
        if (!button) button = GetComponent<Button>();
        rect = GetComponent<RectTransform>();
        
        if (centerPoint)
            centerImage = centerPoint.GetComponent<Image>() ?? centerPoint.GetComponentInChildren<Image>();
        
        layoutElement = GetComponent<LayoutElement>() ?? gameObject.AddComponent<LayoutElement>();
    }
    
    private void CacheOriginalValues()
    {
        originalScale = rect.localScale;
        
        if (centerPoint)
        {
            centerOriginalPos = centerPoint.localPosition;
            centerOriginalScale = centerPoint.localScale;
        }
        
        if (fadeImage) imageOriginalScale = fadeImage.transform.localScale;
        if (text) textOriginalPos = text.transform.localPosition;
        
        var size = rect.sizeDelta;
        originalLayoutSize = new Vector2(
            layoutElement.preferredWidth > 0 ? layoutElement.preferredWidth : size.x,
            layoutElement.preferredHeight > 0 ? layoutElement.preferredHeight : size.y);
        
        layoutElement.preferredWidth = originalLayoutSize.x;
        layoutElement.preferredHeight = originalLayoutSize.y;
    }
    
    private void InitializeStates()
    {
        if (fadeImage) SetAlpha(fadeImage, 0);
        if (text)
        {
            text.text = buttonText;
            SetAlpha(text, 0);
        }
        
        lastDisabledState = disabled;
        UpdateDisabledState();
    }
    
    private void Update()
    {
        // Controlla se lo stato disabled è cambiato nell'Inspector durante play
        if (lastDisabledState != disabled)
        {
            lastDisabledState = disabled;
            UpdateDisabledState();
        }
    }
    
    public void Setup(AnimatedButtonManager mgr, float dur, float scale)
    {
        manager = mgr;
        duration = dur;
        selectedScale = scale;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (disabled) return;
        manager?.OnClick(this);
        
    }
    
    public void SetSelected(bool value)
    {
        if (selected == value) return;
        selected = value;
        
        animation?.Kill();
        animation = DOTween.Sequence();
        
        if (value) AnimateIn();
        else AnimateOut();
    }
    
    private void AnimateIn()
    {
        // Main button scale
        animation.Append(rect.DOScale(originalScale * selectedScale, duration));
        
        // Layout element
        var newSize = originalLayoutSize * selectedScale;
        animation.Join(DOTween.To(() => layoutElement.preferredWidth, x => layoutElement.preferredWidth = x, newSize.x, duration));
        animation.Join(DOTween.To(() => layoutElement.preferredHeight, x => layoutElement.preferredHeight = x, newSize.y, duration));
        OnClick?.Invoke();
        
        // Fade image: appear + scale
        if (fadeImage)
        {
            animation.Join(fadeImage.DOFade(1, duration));
            animation.Join(fadeImage.transform.DOScale(imageOriginalScale * imageScale, duration));
        }
        
        // Text: appear + move up
        if (text)
        {
            animation.Join(text.DOFade(1, duration));
            animation.Join(text.transform.DOLocalMove(textOriginalPos + Vector3.up * textMoveDistance, duration));
        }
        
        // Center point: move up + scale
        if (centerPoint)
        {
            animation.Join(centerPoint.DOLocalMove(centerOriginalPos + Vector3.up * moveDistance, duration));
            animation.Join(centerPoint.DOScale(centerOriginalScale * selectedScale, duration));
        }
    }
    
    private void AnimateOut()
    {
        // Reset everything to original values
        animation.Append(rect.DOScale(originalScale, duration));
        
        animation.Join(DOTween.To(() => layoutElement.preferredWidth, x => layoutElement.preferredWidth = x, originalLayoutSize.x, duration));
        animation.Join(DOTween.To(() => layoutElement.preferredHeight, x => layoutElement.preferredHeight = x, originalLayoutSize.y, duration));
        
        OnClosed?.Invoke();
        
        if (fadeImage)
        {
            animation.Join(fadeImage.DOFade(0, duration));
            animation.Join(fadeImage.transform.DOScale(imageOriginalScale, duration));
        }
        
        if (text)
        {
            animation.Join(text.DOFade(0, duration));
            animation.Join(text.transform.DOLocalMove(textOriginalPos, duration));
        }
        
        if (centerPoint)
        {
            animation.Join(centerPoint.DOLocalMove(centerOriginalPos, duration));
            animation.Join(centerPoint.DOScale(centerOriginalScale, duration));
        }
    }
    
    private void UpdateDisabledState()
    {
        button.interactable = !disabled;
        
        if (centerImage && enabledSprite && disabledSprite)
            centerImage.sprite = disabled ? disabledSprite : enabledSprite;
        
        if (disabled && selected)
            manager?.DeselectAll();
            
    }
    
    private void SetAlpha(Graphic graphic, float alpha)
    {
        var color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }
    
    private void OnDestroy() => animation?.Kill();
    
    public bool IsSelected => selected;
    public bool IsDisabled => disabled;
    public void SetDisabled(bool value) { disabled = value; lastDisabledState = value; UpdateDisabledState(); }
}