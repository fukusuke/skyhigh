using UnityEngine;
using System.Collections;

public class ParameterController : MonoBehaviour 
{
    public static ParameterController Instance{
        get; private set;
    }
    public int coinCount = 0;
    public Vector3 cameraRotate;
    public float time;

    void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad (gameObject);

        coinCount = 0;
        cameraRotate = new Vector3(0.0f, 0.0f, 0.0f);
        time = 0.0f;
    }

    // click callback
    public void OnClick()
    {
        Application.LoadLevel (1);
    }

    public int getCoinCount()
    {
        return this.coinCount;
    }

    public void setCoinCount(int coinCount)
    {
        this.coinCount = coinCount;
    }

    public void increaseCoinCount()
    {
        this.coinCount++;
    }

    public Vector3 getCameraRotate()
    {
        return this.cameraRotate;
    }

    public void setCameraRotate(Vector3 cameraRotate)
    {
        // y以外は0にする
        cameraRotate.x = 0.0f;
        cameraRotate.z = 0.0f;
        this.cameraRotate = cameraRotate;
    }

    public void setTime(float time)
    {
        this.time = time;
    }

    public float getTime()
    {
        return this.time;
    }
}