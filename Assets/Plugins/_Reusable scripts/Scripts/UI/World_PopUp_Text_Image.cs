using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class World_PopUp_Text_Image : MonoBehaviour
{
    [SerializeField] private World_PopUp_Data _data;
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;

    private float _origScale;

    public Image Image { get => _image; }
    public TMP_Text Text { get => _text; }

    private void Awake()
    {
        _origScale = transform.localScale.x;
    }

    public void Show(Sprite sprite = null, string text = null, Action<World_PopUp_Text_Image> onComplete = null)
    {
        if (sprite != null)
            _image.sprite = sprite;
        if (text != null)
            _text.text = text;

        transform.DOScale(_origScale, _data.CanvasTweenDuration).SetEase(_data.CanvasScaleEase).From(0f);
        transform.DOMoveY(_data.CanvasYRange, _data.CanvasTweenDuration).SetRelative(true).
            onComplete += () =>
            {
                if (onComplete == null)
                    onComplete = (x) => Destroy(x.gameObject);

                if (isActiveAndEnabled)
                    StartCoroutine(ShowRoutine(onComplete));
            };
    }

    private IEnumerator ShowRoutine(Action<World_PopUp_Text_Image> onComplete)
    {
        yield return new WaitForSeconds(_data.CanvasShowDuration);
        onComplete?.Invoke(this);
    }
}
