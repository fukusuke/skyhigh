using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SprintToGoal {
    public class Config : MonoBehaviour {

        public Dictionary<string, float> defaultStep;
        public Dictionary<string, float> jumpStep;

        public static Config Instance{
            get; private set;
        }

        void Awake()
        {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad (gameObject);

            // jump
            defaultStep.Add("gravity", 10.0f);
            defaultStep.Add("jumpForce", 10.0f);

            jumpStep.Add("gravity", 10.0f);
            jumpStep.Add("jumpForce", 10.0f);
        }

    }

}