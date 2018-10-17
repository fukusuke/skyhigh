using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private bool debugMode = false;

    // 動きのタイプ
    public string moveType = "start";

    // 進む速度
    private float forwardRate = 0.3f;
    private float maxForwardRate = 1.5f;
    private float minimumForwardRate = 0.1f;

    // プレイヤーの身長
    private float playerHeight = 1.65f;

    // MyGameManager
    private MyGameManager myGameManager;
    private TimeController myTimeController;
    private ParameterController parameterController;

    // ゲーム時間
    private float timeLimit = 110.0f;

    // particle system
    [SerializeField]
    public ParticleSystem particleSystemGreen;
    [SerializeField]
    public ParticleSystem particleSystemRed;

    private ParticleSystem.EmissionModule emGreenObj;
    private ParticleSystem.EmissionModule emRedObj;

    void Start() {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        myGameManager = gameManager.GetComponent<MyGameManager>();

        GameObject timeObject = GameObject.FindGameObjectWithTag("TimeObject");
        myTimeController = timeObject.GetComponent<TimeController>();

        // パラメーター
        parameterController = ParameterController.Instance;

        // particle system
        emGreenObj = particleSystemGreen.emission;
        emRedObj = particleSystemRed.emission;
    }

    void Update() {
        // プレイヤーの移動
        switch (moveType) {

            case "start":
                messageManagement();
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
                myGameManager.startUpWindSound();

                this.setMoveType("overTop");
                break;

            case "overTop":
                moveToTop();
                break;

            case "overTopAfter":
                // 飛んだ後に動作
                myGameManager.startDownWindSound();

                this.setMoveType("normal");
                break;

            case "end":
                myGameManager.startWhistleSound();
                StartCoroutine("changeScene");
                this.setMoveType("endAfter");
                break;

            case "endAfter":

                // 前方へ進む
                moveToForward();

                // プレイヤースピード調整
                playeSpeedDown();
                break;
        }

        if (debugMode) {
            TestPCMode();
        }

        // 画面外に出ないように調節
        warpPlayer();
    }

    void messageManagement()
    {
        if (GvrControllerInput.ClickButtonUp) {
            myGameManager.nextMessage();
        }

        if (Input.GetKey(KeyCode.Z)) {
            myGameManager.nextMessage();
        }
    }

    void warpPlayer()
    {
        float playeAreeLimit = 1500f;
        Vector3 currentPosition = transform.position;
        float x = currentPosition.x;
        float z = currentPosition.z;

        if (x > playeAreeLimit) {
            currentPosition.x = -1 * playeAreeLimit;
            transform.position = currentPosition;
        }
        if (x < (-1 * playeAreeLimit)) {
            currentPosition.x = playeAreeLimit;
            transform.position = currentPosition;
        }

        if (z > playeAreeLimit) {
            currentPosition.z = -1 * playeAreeLimit;
            transform.position = currentPosition;
        }
        if (z < (-1 * playeAreeLimit)) {
            currentPosition.z = playeAreeLimit;
            transform.position = currentPosition;
        }
    }

    void moveToForward()
    {
        Vector3 forward = Camera.main.transform.forward * forwardRate;
        forward.y = 0.0f;
        transform.position += forward;

        // 地面へ落とす
        if (transform.position.y > playerHeight) {
            transform.position += new Vector3(0.0f, -0.4f, 0.0f);
        }
    }

    void moveToTop()
    {
        // 上空へ移動
        transform.position += new Vector3(0.0f, 0.667f, 0.0f);
    }

    // プレイヤーのスピードを調整
    void manageSpeedRate()
    {
        if (GvrControllerInput.ClickButtonUp
            && forwardRate <= maxForwardRate) {
            forwardRate += 0.1f;

            if (forwardRate > maxForwardRate) {
                forwardRate = maxForwardRate;
            }
        }

        if (GvrControllerInput.AppButtonUp 
            && forwardRate >= minimumForwardRate) {
            playeSpeedDown();
        }

        // Debug
        if (Input.GetKey(KeyCode.Z)
            && forwardRate <= maxForwardRate) {
            forwardRate += 0.1f;

            if (forwardRate > maxForwardRate) {
                forwardRate = maxForwardRate;
            }
        }

        if (Input.GetKey(KeyCode.X)
            && forwardRate >= minimumForwardRate) {
            forwardRate -= 0.1f;

            if (forwardRate < minimumForwardRate) {
                forwardRate = minimumForwardRate;
            }
        }

        // Particle 調整
        manageParticleSystem();
    }

    private void manageParticleSystem()
    {
        emRedObj.rate = new ParticleSystem.MinMaxCurve(forwardRate * 200);
    }

    private void playeSpeedDown()
    {
        forwardRate -= 0.1f;

        if (forwardRate < minimumForwardRate) {
            forwardRate = minimumForwardRate;
        }
    }

    public void setMoveType(string moveType)
    {
        // Debug.Log("seted type:" + moveType);
        this.moveType = moveType;
    }

    public string getMoveType(string moveType)
    {
        return this.moveType;
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

    private IEnumerator setLimitMessage()
    {
        yield return new WaitForSeconds (60.0f);

        myTimeController.SetMessageAndDelete("残り1分", 3.0f);
    }

    private IEnumerator setCountDown()
    {
        yield return new WaitForSeconds (timeLimit);

        myTimeController.SetMessage("10");
        yield return new WaitForSeconds (1.0f);

        myTimeController.SetMessage("9");
        yield return new WaitForSeconds (1.0f);

        myTimeController.SetMessage("8");
        yield return new WaitForSeconds (1.0f);

        myTimeController.SetMessage("7");
        yield return new WaitForSeconds (1.0f);

        myTimeController.SetMessage("6");
        yield return new WaitForSeconds (1.0f);

        myTimeController.SetMessage("5");
        yield return new WaitForSeconds (1.0f);

        myTimeController.SetMessage("4");
        yield return new WaitForSeconds (1.0f);

        myTimeController.SetMessage("3");
        yield return new WaitForSeconds (1.0f);

        myTimeController.SetMessage("2");
        yield return new WaitForSeconds (1.0f);

        myTimeController.SetMessage("1");
        yield return new WaitForSeconds (1.0f);

        myTimeController.SetMessage("終了");

        // 終了
        this.setMoveType("end");
    }

    private IEnumerator changeScene()
    {
        yield return new WaitForSeconds (3.0f);

        parameterController.setCameraRotate(Camera.main.transform.localEulerAngles);

        SceneManager.LoadScene("ResultScene");
    }
}