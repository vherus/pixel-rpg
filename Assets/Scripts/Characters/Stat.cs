using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[Serializable]
public class Stat
{
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;
    public float BaseValue;
    public float Value {
        get {
            if (hasBeenModified || BaseValue != lastBaseValue) {
                lastBaseValue = BaseValue;
                _value = CalculateValue();
                hasBeenModified = false;
            }

            return _value;
        }
    }

    private readonly List<StatModifier> statModifiers;
    private bool hasBeenModified = true;
    private float _value;
    private float lastBaseValue = float.MinValue;

    public Stat() {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }

    public Stat(float baseValue) : this() {
        BaseValue = baseValue;
    }

    public void AddModifier(StatModifier mod) {
        hasBeenModified = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModifierOrder);
    }

    public bool RemoveModifier(StatModifier mod) {
        if (statModifiers.Remove(mod)) {
            hasBeenModified = true;
            return true;
        }

        return false;
    }

    public bool RemoveModifiersFromSource(object source) {
        bool didRemove = false;

        for (int i = statModifiers.Count - 1; i >= 0; i--) {
            if (statModifiers[i].Source == source) {
                hasBeenModified = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }

        return didRemove;
    }

    private int CompareModifierOrder(StatModifier a, StatModifier b) {
        if (a.Order < b.Order) {
            return -1;
        } else if (a.Order > b.Order) {
            return 1;
        }

        return 0;
    }

    private float CalculateValue() {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;

        for (int i = 0; i < statModifiers.Count; i++) {
            StatModifier mod = statModifiers[i];

            if (mod.Type == StatModType.Flat) {
                finalValue += mod.Value;
                continue;
            }
            
            if (mod.Type == StatModType.PercentMultiplicative) {
                finalValue *= 1 + mod.Value;
                continue;
            }

            if (mod.Type == StatModType.PercentAdditive) {
                sumPercentAdd += mod.Value;

                if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdditive) {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
        }

        return (float) Math.Round(finalValue, 4);
    }
}
