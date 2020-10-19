using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyHigh {
    public class CoinController : MonoBehaviour {

        private MyGameManager myGameManager;

        void Start () {
            GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
            myGameManager = gameManager.GetComponent<MyGameManager>();
        }

    	// Update is called once per frame
    	void Update () {
            // コインの回転
    		transform.Rotate(0,  Time.deltaTime * 100, 0);
    	}

        void OnTriggerEnter (Collider other)
        {
            // プレイヤーとの衝突時
            if (other.gameObject.name == "Player") {

                // 自分自身を削除
                Destroy(this.gameObject);

                // コイン獲得枚数を増加
                myGameManager.increaseCoinCoint();
            }
        }
    }
}