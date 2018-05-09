using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGod : MonoBehaviour {

    private bool Passive;

    //fuck with the top text about starting the drive! visually!

    public RectTransform CruisingTextParent;
    public float TurnAngle;
    private float RealTurnAngle;
    public float TimeBetweenTurns;
    private float RealTimeBetweenTurns;

    // Transition cruising stuff between positions
    public Vector3 CruisingPosition;
    public Vector3 DrivingPosition;

    public float Startscale;
    public float Endscale;

    public float TransitionTime;


    //fuck with the highscore text! visually!

    public Material ScoreTextMaterial;


    public RectTransform HighScoreText;
    public Vector3 HighScoreCruisingPosition;
    public Vector3 HighScoreDrivingPosition;

    public float HighScoreStartscale;
    public float HighScoreEndscale;

    public float PingPongTime;
    

	// Use this for initialization
	void Start () {
        RealTimeBetweenTurns = TimeBetweenTurns;
        RealTurnAngle = TurnAngle;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (Passive)
        {
            CruiseTextRotate();
        }
	}

    void CruiseTextRotate()
    {
        RealTimeBetweenTurns -= Time.deltaTime;

        if (RealTimeBetweenTurns <= 0)
        {
            CruisingTextParent.transform.localRotation = Quaternion.Euler(0, -RealTurnAngle, 0);
            RealTurnAngle += TurnAngle;
            RealTimeBetweenTurns = TimeBetweenTurns;
            Debug.Log("Should have turned the thing");
        }

        ScoreTextMaterial.SetColor("_Color", Color.Lerp(Color.white, new Color32(255,255,255,70), Mathf.PingPong(Time.time, PingPongTime)));

    }

    public void UIPassiveResponce()
    {
        Passive = true;
        StartCoroutine(MoveText(CruisingTextParent, DrivingPosition, CruisingPosition, Endscale, Startscale, TransitionTime));
        StartCoroutine(MoveText(HighScoreText, HighScoreDrivingPosition, HighScoreCruisingPosition, HighScoreEndscale, HighScoreStartscale, TransitionTime));
    }

    public void UIDriveResponce()
    {
        Passive = false;
        StartCoroutine(MoveText(CruisingTextParent, CruisingPosition, DrivingPosition, Startscale,Endscale,TransitionTime));
        StartCoroutine(MoveText(HighScoreText, HighScoreCruisingPosition, HighScoreDrivingPosition, HighScoreStartscale, HighScoreEndscale, TransitionTime));
    }

    IEnumerator MoveText(RectTransform CruiseObj, Vector3 start, Vector3 end, float StartScale,float EndScale, float TimeTaken)
    {
        float Timer = 0.0f;

        while (Timer <= TimeTaken)
        {
            
         CruiseObj.transform.localPosition = Vector3.Lerp(start, end, (Timer / TimeTaken));
         CruiseObj.transform.localScale = Vector3.Lerp(new Vector3(StartScale,StartScale,StartScale), new Vector3(EndScale, EndScale, EndScale), (Timer / TimeTaken));

            Timer += Time.deltaTime;

            yield return null;
        }
    }

    
}
