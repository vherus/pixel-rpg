using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static bool IsPaused { get; private set; }

    public static GameManager Instance {
        get {
            if (instance == null) {
                Debug.LogError("GameManager is null");
            }

            return instance;
        }
    }

    public static void PauseUnpause() {
        if (IsPaused) {
            Time.timeScale = 1;
            IsPaused = false;
            return;
        }

        Time.timeScale = 0;
        IsPaused = true;
    }

    void Awake() {
        instance = this;
    }
}
