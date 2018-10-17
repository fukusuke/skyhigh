using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SprintToGoal {
    public class MyGameManager : MonoBehaviour {

        // プレイヤー
        private PlayerController playerController;

        // パラメーター
        private ParameterController parameterController;

        // 音源
        private AudioSource[] audioSources;
        private Dictionary<string, int> soundKeys;

        // alpha
        public float alpha;
        public float alphaMax = 1.0f;
        public float alphaMin = 0.0f;

        // sky
        private GameObject sky;
        private Material skyMaterial;
        private Color skyColor;

        // sky
        private GameObject whiteSky;
        private Material whiteSkyMaterial;
        private  Color whiteSkyColor;

        // Use this for initialization
        void Start () {

            // パラメーター
            parameterController = ParameterController.Instance;

            // プレイヤー
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerController = player.GetComponent<PlayerController>();

            // 音源
            GameObject audioManager = GameObject.FindGameObjectWithTag("AudioManager");
            audioSources = audioManager.GetComponents<AudioSource>();
            soundKeys = new Dictionary<string, int>()
            {
                {"mainBGM", 0},
                {"upWindow", 1},
                {"explain", 2},
                {"downWindow", 3},
                {"goalSound", 4},
                {"normalLanding", 5},
                {"superJumpLanding", 6}
            };

            audioSources[2].PlayOneShot(audioSources[2].clip);

            // sky
            sky = player.transform.Find("sky").gameObject;
            skyMaterial = sky.GetComponent<Renderer>().material;
            skyColor = skyMaterial.GetColor("_Color");

            // white sky
            whiteSky = player.transform.Find("WhiteSky").gameObject;
            whiteSkyMaterial = whiteSky.GetComponent<Renderer>().material;
            whiteSkyColor = skyMaterial.GetColor("_Color");
            whiteSkyColor.r = 255.0f;
            whiteSkyColor.g = 255.0f;
            whiteSkyColor.b = 255.0f;
        }

        public void startSound(string soundType)
        {
            int key = soundKeys[soundType];
            audioSources[key].PlayOneShot(audioSources[key].clip);
        }

        public void endSound(string soundType)
        {
            int key = soundKeys[soundType];
            audioSources[key].Stop();
        }

        public bool isPlayingSound(string soundType)
        {
            int key = soundKeys[soundType];
            return audioSources[key].isPlaying;
        }

        public bool FadeInSky()
        {
            if (skyColor.a < 1.0) {
                skyColor.a += 0.01f;
                skyMaterial.SetColor("_Color", skyColor);
                return false;
            }
         
            return true;   
        }

        public bool FadeOutSky()
        {
            if (skyColor.a > 0) {
                skyColor.a -= 0.01f;
                skyMaterial.SetColor("_Color", skyColor);

                return false;
            }

            return true;
        }

        public bool FadeInWhiteSky()
        {
            if (whiteSkyColor.a < 1.0) {
                whiteSkyColor.a += 0.01f;
                whiteSkyMaterial.SetColor("_Color", whiteSkyColor);
                return false;
            }
         
            return true;   
        }

        public bool FadeOutWhiteSky()
        {
            if (whiteSkyColor.a > 0) {
                whiteSkyColor.a -= 0.01f;
                whiteSkyMaterial.SetColor("_Color", whiteSkyColor);

                return false;
            }

            return true;
        }
    }
}