using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DigitalRuby.Tween;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Synthesis.UI;

[RequireComponent(typeof(Button))]
public class MenuButtonTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    // public float IndicateScaleFactor = 1.1f;
    private float NormalX;
    private float NormalY;

    public static float IndicateExpansion = 10;
    public static float ClickedContraction = 5;
    // public float ClickedScaleFactor = 0.98f;
    private float _completionTime = 0.2f;
    private Button _button;

    public const string INDICATE_TWEEN = "indicate";
    public const string RETURN_TWEEN = "return";
    public const string CLICKED_TWEEN = "pressed";

    private Vector3Tween _activeTween = null;
    private string _state = "exit";

    private Action<ITween<Vector3>> updateButtonScale;

    private Vector3 _indicateScale {
        get => new Vector3((NormalX + IndicateExpansion) / NormalX, (NormalY + IndicateExpansion) / NormalY, 1f);
    }
    private Vector3 _clickedScale {
        get => new Vector3((NormalX - ClickedContraction) / NormalX, (NormalY - ClickedContraction) / NormalY, 1f);
    }

    public void Start() {
        _button = GetComponent<Button>();
        updateButtonScale = t => {
            gameObject.transform.localScale = t.CurrentValue;
        };

        var rect = GetComponent<RectTransform>().rect;
        NormalX = rect.width / 2;
        NormalY = rect.height / 2;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _state = "enter";
        // MenuManager.Instance.SpotlightButton(transform);
        TweenFactory.RemoveTweenKey(gameObject.name + CLICKED_TWEEN, TweenStopBehavior.DoNotModify);
        TweenFactory.RemoveTweenKey(gameObject.name + RETURN_TWEEN, TweenStopBehavior.DoNotModify);
        // transform.GetComponent<Image>().
        _activeTween = gameObject.Tween(gameObject.name + INDICATE_TWEEN, gameObject.transform.localScale, _indicateScale, _completionTime, TweenScaleFunctions.CubicEaseOut, updateButtonScale);
    }
    public void OnPointerExit(PointerEventData eventData) {
        _state = "exit";
        TweenFactory.RemoveTweenKey(gameObject.name + INDICATE_TWEEN, TweenStopBehavior.DoNotModify);
        if (_activeTween.State == TweenState.Running && _activeTween.Key.Equals(gameObject.name + CLICKED_TWEEN)) {
            _activeTween.ContinueWith(new Vector3Tween().Setup(_clickedScale, Vector3.one, _completionTime, TweenScaleFunctions.CubicEaseOut, updateButtonScale));
        } else {
            _activeTween = gameObject.Tween(gameObject.name + RETURN_TWEEN, gameObject.transform.localScale, Vector3.one, _completionTime, TweenScaleFunctions.CubicEaseOut, updateButtonScale);
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        _state = "click";
        if (TweenFactory.RemoveTweenKey(gameObject.name + CLICKED_TWEEN, TweenStopBehavior.DoNotModify)) {
            gameObject.transform.localScale = Vector3.one;
        }
        TweenFactory.RemoveTweenKey(gameObject.name + INDICATE_TWEEN, TweenStopBehavior.DoNotModify);
        _activeTween = gameObject.Tween(gameObject.name + CLICKED_TWEEN, gameObject.transform.localScale, _clickedScale, _completionTime, TweenScaleFunctions.CubicEaseOut, updateButtonScale, x => {
            if (!_state.Equals("exit"))
                OnPointerEnter(null);
        });
    }
}
