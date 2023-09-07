using System;
using UnityEngine;

[Serializable]
public class StateAnimationSetDictionary : SerializableDictionary<CharacterState, DirectionalAnimationSet>
{
    public AnimationClip GetFacingClipFromState(CharacterState state, Vector2 facingDirection) {
        if (TryGetValue(state, out DirectionalAnimationSet animationSet)) {
            return animationSet.GetFacingClip(facingDirection);
        }

        Debug.LogError($"Character state {state.name} not found in StateAnimations dictionary");

        return null;
    }
}
