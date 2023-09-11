using UnityEngine;

public class Spell : MonoBehaviour
{
    private PlayerController player;

    void Start() {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject) {
            player = playerObject.GetComponent<PlayerController>();
        }
    }

    void FixedUpdate() {
        if (!player) {
            Destroy(gameObject);
            return;
        }

        Enemy target = player.Target;

        if (target) {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 1.5f * Time.deltaTime);
            return;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }
}
