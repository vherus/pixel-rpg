using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRadius : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider) {
        Enemy enemy = collider.gameObject.GetComponent<Enemy>();

        if (enemy) {
            GameManager.AvailableTargets.Add(enemy);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        Enemy enemy = collider.gameObject.GetComponent<Enemy>();

        if (enemy) {
            enemy.GetComponentInChildren<TargetIndicator>().GetComponent<SpriteRenderer>().enabled = false;
            GameManager.AvailableTargets.Remove(enemy);
        }
    }
}
