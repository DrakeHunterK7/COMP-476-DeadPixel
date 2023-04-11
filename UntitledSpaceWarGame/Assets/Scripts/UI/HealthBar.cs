using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //HP References
    [Header("Health Settings")]
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Gradient _hpGradient;
    [SerializeField] private Image _hpFill;

    //Shield HP References
    [Header("Shield Settings")]
    [SerializeField] private Slider _shieldSlider;
    [SerializeField] private Gradient _shieldHpGradient;
    [SerializeField] private Image _shieldHpFill;

    //Energy References
    [Header("Energy Settings")]
    [SerializeField] private Slider _energySlider;
    [SerializeField] private Gradient _energyGradient;
    [SerializeField] private Image _energyFill;

    public void SetMaxHP(float maxHP)
    {
        _hpSlider.maxValue = maxHP;
        _hpSlider.value = maxHP;
        _hpFill.color = _hpGradient.Evaluate(1f);
    }

    public void SetHealth(float value)
    {
        _hpSlider.value = value;
        _hpFill.color = _hpGradient.Evaluate(_hpSlider.normalizedValue);
    }

    public void SetMaxShieldHP(float maxShieldHP)
    {
        _shieldSlider.maxValue = maxShieldHP;
        _shieldSlider.value = maxShieldHP;
        _shieldHpFill.color = _shieldHpGradient.Evaluate(1f);
    }

    public void SetShieldHP(float value)
    {
        _shieldSlider.value = value;
        _shieldHpFill.color = _shieldHpGradient.Evaluate(_shieldSlider.normalizedValue);
    }

    public void SetMaxEnergy(float maxEnergy)
    {
        _energySlider.maxValue = maxEnergy;
        _energySlider.value = maxEnergy;
        _energyFill.color = _energyGradient.Evaluate(1f);
    }

    public void SetEnergy(float value)
    {
        _energySlider.value = value;
        _energyFill.color = _energyGradient.Evaluate(_energySlider.normalizedValue);
    }
}
