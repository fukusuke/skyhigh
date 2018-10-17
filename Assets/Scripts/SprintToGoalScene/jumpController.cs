using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SprintToGoal {

    public class jumpController : MonoBehaviour {

        private PlayerController playerController;

        public string jumpType = "jump";
        public float jumpForce = 10.0f;
        public float jumpGravity = 10.0f;
        public float jumpMoveX = 0.0f;
        public float jumpMoveZ = 0.0f;

        void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerController = player.GetComponent<PlayerController>();
        }

        void OnTriggerEnter (Collider other)
        {
            switch (jumpType) {
                case "jump":
                    if (other.gameObject.name == "Player") {
                        playerController.setSoundType("upWindow");
                        playerController.setMoveType("jumpBefore");

                        playerController.jumpGravity = this.jumpGravity;
                        playerController.jumpForce = this.jumpForce;
                        playerController.jumpMoveX = this.jumpMoveX;
                        playerController.jumpMoveZ = this.jumpMoveZ;
                    }
                    break;

                case "toMain":
                    if (other.gameObject.name == "Player") {
                        playerController.setSoundType("upWindow");
                        playerController.setMoveType("tutorialEndBefore");

                        playerController.jumpGravity = this.jumpGravity;
                        playerController.jumpForce = this.jumpForce;
                        playerController.jumpMoveX = this.jumpMoveX;
                        playerController.jumpMoveZ = this.jumpMoveZ;
                    }
                    break;

                case "superJump":
                    if (other.gameObject.name == "Player") {
                        playerController.setSoundType("downWindow");
                        playerController.setLandSoundType("superJumpLanding");
                        playerController.setMoveType("jumpBefore");

                        playerController.jumpGravity = this.jumpGravity;
                        playerController.jumpForce = this.jumpForce;
                        playerController.jumpMoveX = this.jumpMoveX;
                        playerController.jumpMoveZ = this.jumpMoveZ;
                    }
                    break;

                case "jumpAndCamera":
                    if (other.gameObject.name == "Player") {
                        playerController.setSoundType("upWindow");
                        playerController.setMoveType("jumpBefore");

                        playerController.jumpGravity = this.jumpGravity;
                        playerController.jumpForce = this.jumpForce;
                        playerController.jumpMoveX = this.jumpMoveX;
                        playerController.jumpMoveZ = this.jumpMoveZ;

                        Camera.main.farClipPlane = 2000;
                    }
                    break;

                case "goal":
                    if (other.gameObject.name == "Player") {
                        playerController.setSoundType("goalSound");
                        playerController.setMoveType("goalBefore");
                    }
                    break;
            }
            
        }
    }
}