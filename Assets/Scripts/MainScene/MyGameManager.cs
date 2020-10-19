using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyHigh {
    public class MyGameManager : MonoBehaviour {

        [SerializeField]
        private int coinCount = 0;

        private string message1 = "この平原にはコインがたくさん\n散らばっているぞ！\n                   Click > Next Page";
        private string message2 = "制限時間は2分間！\nできるだけたくさんコインを集めよう！\n                   Click > Next Page";
        private string message3 = "Clickボタンを押すとスピードアップするぞ！\nその下のボタンはスピードダウンだ！\n                   Click > Next Page";
        private string message4 = "準備はいいかい？\nさぁ ゲームスタート！";
        private int pageCount = 1;

        // 音源
        private AudioSource BGMSource;
        private AudioSource explainSource;
        private AudioSource coinSource;
        private AudioSource upWindSource;
        private AudioSource downWindSource;
        private AudioSource whistleSource;

        // 説明オブジェクト
        private GameObject explainText;
        private bool lastMessageFlag = false;

        // プレイヤー
        private PlayerController playerController;

        // パラメーター
        private ParameterController parameterController;

    	// Use this for initialization
    	void Start () {

            // パラメーター
            parameterController = ParameterController.Instance;

            // プレイヤー
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerController = player.GetComponent<PlayerController>();

            // 音楽関係
            AudioSource[] audioSources = GetComponents<AudioSource> ();
            BGMSource = audioSources[0];
            coinSource = audioSources[1];
            upWindSource = audioSources[2];
            downWindSource = audioSources[3];
            explainSource = audioSources[4];
            whistleSource = audioSources[5];
    		explainSource.PlayOneShot(explainSource.clip);

            // 説明オブジェクト
            explainText = GameObject.FindGameObjectWithTag("ExplainText");
            this.setMessage(message1);

            // スコア
            parameterController.setCoinCount(0);
    	}
    	
    	// Update is called once per frame
    	void Update () {

    	}

        public void increaseCoinCoint()
        {
            coinSource.PlayOneShot(coinSource.clip);

            parameterController.increaseCoinCount();
        }

        public void startUpWindSound()
        {
            upWindSource.PlayOneShot(upWindSource.clip);
        }

        public void startDownWindSound()
        {
            downWindSource.PlayOneShot(downWindSource.clip);
        }

        public void startWhistleSound()
        {
            whistleSource.PlayOneShot(whistleSource.clip);
        }

        public bool isPlayingDownpWindSound()
        {
            return downWindSource.isPlaying;
        }

        public void stopDownpWindSound()
        {
            downWindSource.Stop();
        }

        public void nextMessage()
        {
            switch (this.pageCount) {
                case 1:
                    this.setMessage(message2);
                    break;
                case 2:
                    this.setMessage(message3);
                    break;
                case 3:
                    this.setMessage(message4);
                    break;
                case 4:
                    Destroy(explainText);
                    playerController.setMoveType("normalBefore");

                    explainSource.Stop();
                    BGMSource.PlayOneShot(BGMSource.clip);
                    break;
            }
            this.pageCount++;
        }

        private void setMessage(string message)
        {
            explainText.transform.Find("Text").GetComponent<TextMesh>().text = message;
        }
    }
}