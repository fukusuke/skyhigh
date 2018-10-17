using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverTopController : MonoBehaviour {

    private PlayerController playerController;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.name == "Player") {
            playerController.setMoveType("overTopBefore");

            StartCoroutine("endOverTop");
        }
    }

    private IEnumerator endOverTop()
    {
        yield return new WaitForSeconds(3.0f);
        playerController.setMoveType("overTopAfter");
    }
}
    