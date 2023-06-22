using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class UI_Screen : MonoBehaviour
{
    public enum Name
    {
        Game,
        Lose,
        Win
    }

    [SerializeField] private Name screenName;
    [SerializeField] private float showDuration = 1f;

    private CanvasGroup _canvasGroup;

    public UnityEvent OnShow;
    public UnityEvent OnHide;

    public Name ScreenName => screenName;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(float duration)
    {
        _canvasGroup.DOFade(1f, duration).SetUpdate(true);
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        OnShow?.Invoke();
    }

    public void Show()
    {
        Show(showDuration);
    }

    public void Hide()
    {
        Hide(showDuration);
    }

    public void Hide(float duration)
    {
        _canvasGroup.DOFade(0f, duration).SetUpdate(true);
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        OnHide?.Invoke();
    }
}
