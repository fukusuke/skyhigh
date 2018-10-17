using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultSceneController : MonoBehaviour {

    // スコア
    private ParameterController parameterController;

    private float startTime;
    private float waitTime = 10f;

    // Use this for initialization
    void Start () {
        // スタートタイム
        startTime = Time.time;

        // パラメーター
        parameterController = ParameterController.Instance;

        // プレイヤー
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // カメラ角度を修正
        Vector3 rotate = (-1) * parameterController.getCameraRotate();
        player.transform.Rotate(rotate);

        // タイムを取得
        float time = parameterController.getTime();
        GameObject resultText = GameObject.FindGameObjectWithTag("ResultText");
        Debug.Log(resultText);
        var builder = new StringBuilder();
        builder.Append(time);
        builder.Append(" s");
        resultText.GetComponent<Text>().text = builder.ToString();

    }
    
    // Update is called once per frame
    void Update () {

        if ((Time.time - startTime) > waitTime) {
            if (GvrControllerInput.ClickButtonUp) {
                SceneManager.LoadScene("Start");
            }

            if (Input.GetKey(KeyCode.Z)) {
                SceneManager.LoadScene("Start");
            }
        }
    }
}
