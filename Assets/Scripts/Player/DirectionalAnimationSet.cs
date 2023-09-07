using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DirectionalAnimationSet", menuName = "ScriptableObjects/DirectionalAnimationSet")]
public class DirectionalAnimationSet : ScriptableObject
{
    [field: SerializeField] public AnimationClip Up { get; private set; }
    [field: SerializeField] public AnimationClip Down { get; private set; }
    [field: SerializeField] public AnimationClip Left { get; private set; }
    [field: SerializeField] public AnimationClip Right { get; private set; }

    public AnimationClip GetFacingClip(Vector2 facing) {
        Vector2 closestDirection = GetClosestDirection(facing);

        if (closestDirection == Vector2.left) {
            return Left;
        }
        
        if (closestDirection == Vector2.right) {
            return Right;
        }
        
        if (closestDirection == Vector2.up) {
            return Up;
        }
        
        if (closestDirection == Vector2.down)  {
            return Down;
        }

        throw new ArgumentException($"Direction not expected: {closestDirection}");
    }

    public Vector2 GetClosestDirection(Vector2 inputDirection) {
        Vector2 normalised = inputDirection.normalized;

        Vector2 closestDirection = Vector2.zero;
        float closestDistance = 0f;

        Vector2[] directionsToCheck = new Vector2[4] { Vector2.down, Vector2.up, Vector2.left, Vector2.right };

        for (int i = 0; i < directionsToCheck.Length; i++) {
            if (i == 0) {
                closestDirection = directionsToCheck[i];
                closestDistance = Vector2.Distance(normalised, directionsToCheck[i]);
                continue;
            }

            float nextDistance = Vector2.Distance(normalised, directionsToCheck[i]);

            if (nextDistance < closestDistance) {
                closestDistance = nextDistance;
                closestDirection = directionsToCheck[i];
            }
        }

        return closestDirection;
    }
}
