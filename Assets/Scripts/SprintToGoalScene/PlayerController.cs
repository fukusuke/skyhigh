using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using UnityEngine.SceneManagement;

namespace SprintToGoal {
    public class PlayerController : MonoBehaviour {

        private CharacterController controller;
        private float verticalVelocity;

        [SerializeField]
        private bool debugMode = false;

        // 動きのタイプ
        public string moveType = "normal";
        public string soundType = "upWindow";
        public string landSoundType = "normalLanding";

        // 進む速度
        private float forwardRate = 10.0f;
        private float maxForwardRate = 40.0f;
        private float minimumForwardRate = 5.0f;
        private float upDownRate = 1.0f;

        // プレイヤーの身長
        private float playerHeight = 1.65f;

        // MyGameManager
        private MyGameManager myGameManager;
        private ParameterController parameterController;

        // ゲーム時間
        private float timeLimit = 110.0f;

        // jump config
        private bool isAbleJump = false;
        public float jumpGravity = 10.0f;
        public float jumpForce = 20.0f;
        public float jumpMoveX = 0.0f;
        public float jumpMoveZ = 0.0f;

        // time
        private float startTime;
        private float endTime;

        // particle system
        [SerializeField]
        public ParticleSystem particleSystemGreen;
        [SerializeField]
        public ParticleSystem particleSystemRed;

        private ParticleSystem.EmissionModule emGreenObj;
        private ParticleSystem.EmissionModule emRedObj;

        // リセット用
        private int resetNumber = 0;

        void Start() {

            controller = transform.GetComponent<CharacterController>();
            GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
            myGameManager = gameManager.GetComponent<MyGameManager>();

            // particle system
            emGreenObj = particleSystemGreen.emission;
            emRedObj = particleSystemRed.emission;

            // // パラメーター
            parameterController = ParameterController.Instance;
        }

        void Update() {

            // プレイヤーの移動
            switch (moveType) {

                case "start":
                    
                    break;
                case "mainBefore":
                    // フェードイン
                    if (myGameManager.FadeOutSky()) {
                        this.setMoveType("normal");

                        // 速度初期化
                        forwardRate = 10.0f;
                    }
                    break;

                case "normalBefore":
                     // 残り1分告知
                    StartCoroutine("setLimitMessage");

                    // ゲーム終了告知
                    StartCoroutine("setCountDown");

                    this.setMoveType("normal");
                    break;

                case "normal":
                    // 前方へ進む
                    moveToForward();

                    // プレイヤースピード調整
                    manageSpeedRate();
                    break;

                case "overTopBefore":
                    // 飛ぶ前に動作
                    myGameManager.startSound("overTop");

                    this.setMoveType("overTop");
                    break;

                case "overTop":
                    moveToTop();
                    break;

                case "overTopAfter":
                    this.setMoveType("normal");
                    break;

                // ジャンプ前の動作
                case "jumpBefore":
                    myGameManager.startSound(this.soundType);

                    this.setMoveType("jump");
                    isAbleJump = true;

                    // ジャンプ力初期化
                    verticalVelocity = jumpForce;
                    break;

                // ジャンプ
                case "jump":
                    jump();

                    manageSpeedRate();
                    break;

                // ジャンプ後の動作
                case "jumpAfter":
                    // ジャンプの音終わり
                    myGameManager.endSound(this.soundType);

                    // 着地の音
                    myGameManager.startSound(this.landSoundType);

                    this.setMoveType("normal");
                    break;

                case "end":
                    StartCoroutine("changeScene");
                    this.setMoveType("endAfter");
                    break;

                case "endAfter":

                    // 前方へ進む
                    moveToForward();

                    // プレイヤースピード調整
                    playeSpeedDown();
                    break;

                case "tutorialEndBefore":
                    // ジャンプの音
                    myGameManager.startSound(this.soundType);

                    // チュートリアル終了
                    this.setMoveType("jump");
                    isAbleJump = true;

                    // ジャンプ力初期化
                    verticalVelocity = jumpForce;

                    // パーティクルを外す
                    forwardRate = 0.0f;

                    StartCoroutine(changeMoveType("tutorialEnd", 3.0f));
                    break;

                case "tutorialEnd":
                    jump();

                    // フェードイン
                    if (myGameManager.FadeInSky()) {
                        this.setMoveType("tutorialEndAfter");

                        maxForwardRate = 125f;

                        // チュートリアルBGM終わり
                        myGameManager.endSound("explain");
                    }

                    break;
                case "tutorialEndAfter":
                    transform.position = new Vector3(50.0f, 1.0f, 32.0f);

                    startTime = Time.time;

                    this.setMoveType("mainBefore");

                    // メインBGM始まり
                    if (myGameManager.isPlayingSound("mainBGM") == false) {
                        myGameManager.startSound("mainBGM");
                    }
                    break;

                case "goalBefore":
                    // 音
                    myGameManager.startSound(this.soundType);

                    this.setMoveType("goal");
                    
                    break;

                case "goal":
                    if (myGameManager.FadeInWhiteSky()) {
                        this.setMoveType("goalAfter");
                    }
                    break;

                case "goalAfter":
                    StartCoroutine(changeScene(2.0f));
                    break;

                case "nothingToDo":
                    break;


            }

            // particle
            manageParticle();

            if (debugMode) {
                TestPCMode();
            }

            if (GvrControllerInput.ClickButton
                && GvrControllerInput.AppButton) {
                resetNumber += 1;

                if (resetNumber > 120) {
                    SceneManager.LoadScene("Start");
                }
            } else {
                resetNumber = 0;
            }
        }

        void manageParticle()
        {
            emGreenObj.rateOverTime = forwardRate;
            emRedObj.rateOverTime = forwardRate * 2;
        }

        void moveToForward()
        {
            Vector3 forward = Camera.main.transform.forward * forwardRate;
            forward.y = 0.0f;
            controller.SimpleMove(forward);
        }

        void moveToTop()
        {
            // 上空へ移動
            transform.position += new Vector3(0.0f, 0.667f, 0.0f);
        }

        // プレイヤーのスピードを調整
        void manageSpeedRate()
        {
            if (GvrControllerInput.ClickButton
                && forwardRate <= maxForwardRate) {
                forwardRate += upDownRate;

                if (forwardRate > maxForwardRate) {
                    forwardRate = maxForwardRate;
                }
            }

            if (GvrControllerInput.AppButton
                && forwardRate >= minimumForwardRate) {
                playeSpeedDown();
            }

            // Debug
            if (Input.GetKey(KeyCode.Z)
                && forwardRate <= maxForwardRate) {
                forwardRate += upDownRate;

                if (forwardRate > maxForwardRate) {
                    forwardRate = maxForwardRate;
                }
            }

            if (Input.GetKey(KeyCode.X)
                && forwardRate >= minimumForwardRate) {
                forwardRate -= upDownRate;

                if (forwardRate < minimumForwardRate) {
                    forwardRate = minimumForwardRate;
                }
            }
        }

        private void jump()
        {
            if (controller.isGrounded)
            {
                if (isAbleJump) {
                    isAbleJump = false;
                } else {
                    this.setMoveType("jumpAfter");
                    return;
                }
                
            } else {
                verticalVelocity -= jumpGravity * Time.deltaTime;
            }

            Vector3 moveVector = Vector3.zero;
            moveVector.x = this.jumpMoveX;
            moveVector.y = verticalVelocity;
            moveVector.z = this.jumpMoveZ;
            controller.Move(moveVector * Time.deltaTime);
        }

        private void playeSpeedDown()
        {
            forwardRate -= upDownRate;

            if (forwardRate < minimumForwardRate) {
                forwardRate = minimumForwardRate;
            }
        }

        public void setMoveType(string moveType)
        {
            this.moveType = moveType;
        }

        public string getMoveType()
        {
            return this.moveType;
        }

        public void setSoundType(string soundType)
        {
            this.soundType = soundType;
        }

        public string getSoundType()
        {
            return this.soundType;
        }

        public void setLandSoundType(string landSoundType)
        {
            this.landSoundType = landSoundType;
        }

        void TestPCMode()
        {
            if (Input.GetKey(KeyCode.LeftArrow)) {
                gameObject.transform.Rotate(new Vector3(0, -1.0f, 0));
            }

            if (Input.GetKey(KeyCode.RightArrow)) {
                gameObject.transform.Rotate(new Vector3(0, 1.0f, 0));
            }
        }

        /****************************************************************/

        private IEnumerator changeScene(float waitSecond = 0.0f)
        {
            yield return new WaitForSeconds (waitSecond);

            // カメラ位置
            parameterController.setCameraRotate(Camera.main.transform.localEulerAngles);

            // タイム
            float time = Time.time - startTime;
            time = Mathf.Round(time * 10) / 10;
            parameterController.setTime(time);

            SceneManager.LoadScene("ResultScene");
        }

        private IEnumerator changeMoveType(string moveType, float waitSecond) { 

            yield return new WaitForSeconds (waitSecond);

            this.setMoveType(moveType);
        }  
    }
}