using System.Collections.Generic;
using UnityEngine;

public class StatusEffectRunner : MonoBehaviour
{
    private List<StatusEffect> activeEffects = new List<StatusEffect>();

    public void ApplyEffect(StatusEffect effect)
    {
        activeEffects.Add(effect);
    }

    private void Update()
    {
        for(int i = 0; i < activeEffects.Count; i++)
        {
            var effect = activeEffects[i];
            effect.Tick(Time.deltaTime);
            if (effect.IsFinished)
            {
                activeEffects.RemoveAt(i);
                i--;
            }
        }
    }
}
