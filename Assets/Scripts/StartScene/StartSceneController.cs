using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour {

    // 音源
    private AudioSource BGMSource;
    private AudioSource decisionSource;

    // プレイヤー
    private GameObject player;

    // スタート
    private float startTime;
    private float waitTime = 3.0f;

	// Use this for initialization
	void Start () {
        // プレイヤー
        player = GameObject.FindGameObjectWithTag("Player");

        // 音楽関係
        AudioSource[] audioSources = GetComponents<AudioSource> ();
        BGMSource = audioSources[0];
        decisionSource = audioSources[1];

        // スタート
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if ((Time.time - startTime) > waitTime) {
            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) {
                changeSceneToGame();
            }

            // debug
            if (Input.GetKey(KeyCode.Z)) {
                changeSceneToGame();
            } 
        }
	}

    void changeSceneToGame()
    {
        decisionSource.PlayOneShot(decisionSource.clip);

        SceneManager.LoadScene("SprintToGoal");
    }
}
