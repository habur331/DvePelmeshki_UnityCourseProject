using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopupAnimation : MonoBehaviour
{
    [SerializeField] private AnimationCurve opacityCurve;
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private AnimationCurve heightCurve;

    private TextMeshProUGUI _tmp;
    private float _time = 0;
    private Vector3 _origin;

    private void Awake()
    {
        _tmp = transform.GetComponentInChildren<TextMeshProUGUI>();
        _origin = transform.position;
    }
    
    private void Update()
    {
        _tmp.color = new Color(_tmp.color.r, _tmp.color.g, _tmp.color.b, opacityCurve.Evaluate(_time));
        transform.localScale = Vector3.one * scaleCurve.Evaluate(_time);
        transform.position = _origin + new Vector3(0, 1 + heightCurve.Evaluate(_time), 0);
        _time += Time.deltaTime;
    }
}
