using System.Collections;
using System.Collections.Generic;
using GameCraftGuild.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour {

    Timer changeTimer;

    // Start is called before the first frame update
    void Start () {
        changeTimer = TimerFactory.CreateManagedTimer(35, false);
        changeTimer.AddOnFinishAction(ChangeScene);

        changeTimer.StartTimer();
    }

    // Update is called once per frame
    void Update () {

    }

    private void ChangeScene () {
        SceneManager.LoadScene("TestScene2");
    }
}
