using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {

    private GameObject text;
    private GameObject endText;
    private TextMesh textMesh;
    private TextMesh endTextMesh;

	// Use this for initialization
	void Start () {
		text = gameObject.transform.Find("Text").gameObject;
        textMesh = text.GetComponent<TextMesh>();

        endText = gameObject.transform.Find("EndText").gameObject;
        endTextMesh = endText.GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetMessageAndDelete(string message, float waitTime)
    {
        this.textMesh.text = message;

        StartCoroutine ("DeleteMessage", waitTime);  
    }

    private IEnumerator DeleteMessage(float waitTime)
    {
        yield return new WaitForSeconds (waitTime); 

        this.textMesh.text = "";
    }

    public void SetMessage(string message)
    {
        this.endTextMesh.text = message;
    }
}
