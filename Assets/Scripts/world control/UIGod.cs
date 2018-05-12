using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGod : MonoBehaviour {
    public static UIGod Instance;
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

    //fuck with the currentscore text! visually!

    public RectTransform CurrentScoreText;
    public Vector3 CurrentScoreCruisingPosition;
    public Vector3 CurrentScoreDrivingPosition;

    public float CurrentScoreStartscale;
    public float CurrentScoreEndscale;

    //fuck with the failk text! visually!

    public RectTransform FailScoreText;
    public Vector3 FailScoreCruisingPosition;
    public Vector3 FailScoreDrivingPosition;

    public float FailScoreStartscale;
    public float FailScoreEndscale;



    // store the points 

    float PointsThisRun;

    float TheHighScore;







    // Use this for initialization
    void Start ()
    {
        Instance = this;
        RealTimeBetweenTurns = TimeBetweenTurns;
        RealTurnAngle = TurnAngle;
        PointsThisRun = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (Passive)
        {
            CruiseTextRotate();
        }
        ScoreTextMaterial.SetColor("_Color", Color.Lerp(Color.white, new Color32(255, 255, 255, 70), Mathf.PingPong(Time.time, PingPongTime)));
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

       

    }


    public void PlayerFailed()
    {
        StartCoroutine(MoveText(FailScoreText, FailScoreDrivingPosition, FailScoreCruisingPosition, FailScoreStartscale, FailScoreEndscale,  1.3f));
    }

    public void PointCollected()
    {
        PointsThisRun++;
        CurrentScoreText.Find("CScore Text").GetComponent<Text>().text = "Current Score " + PointsThisRun;
    }

    void UpdateHighScore()
    {
        TheHighScore = PointsThisRun;
        HighScoreText.Find("HighScore Text").GetComponent<Text>().text = "High Score " + PointsThisRun;
       // Debug.Log("the highscore has been updated to" + TheHighScore);
    }



    public void UIPassiveResponce()
    {
        Passive = true;
        StartCoroutine(MoveText(CruisingTextParent, DrivingPosition, CruisingPosition, Endscale, Startscale, TransitionTime));
        StartCoroutine(MoveText(HighScoreText, HighScoreDrivingPosition, HighScoreCruisingPosition, HighScoreEndscale, HighScoreStartscale, TransitionTime));
        StartCoroutine(MoveText(CurrentScoreText, CurrentScoreDrivingPosition, CurrentScoreCruisingPosition, CurrentScoreEndscale, CurrentScoreStartscale, TransitionTime));
        if (FailScoreText .localScale.x > 0)
        {
            FailScoreText.localScale = new Vector3(0, 0, 0);
        }
        if (PointsThisRun > TheHighScore)
        {
            UpdateHighScore();
        }
    }

    public void UIDriveResponce()
    {
        PointsThisRun = 0;
        Passive = false;
        StartCoroutine(MoveText(CruisingTextParent, CruisingPosition, DrivingPosition, Startscale, Endscale, TransitionTime));
        StartCoroutine(MoveText(HighScoreText, HighScoreCruisingPosition, HighScoreDrivingPosition, HighScoreStartscale, HighScoreEndscale, TransitionTime));
        StartCoroutine(MoveText(CurrentScoreText, CurrentScoreCruisingPosition, CurrentScoreDrivingPosition, CurrentScoreStartscale, CurrentScoreEndscale, TransitionTime));
        CurrentScoreText.Find("CScore Text").GetComponent<Text>().text = "Current Score " + PointsThisRun;
        Debug.Log("the highscore is now" + TheHighScore);

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
