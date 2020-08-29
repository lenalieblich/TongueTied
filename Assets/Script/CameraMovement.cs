using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    static CameraMovement instanceCameraMove;

    float deltaTime = 0.0f;


    private Vector3[] angles;
    private int index;
    private Vector3 centerAngle;

    private int thinkTime;

    private Reaction _reaction;
    public Reaction reaction
    {
        get { return _reaction; }
        set
        {
            _reaction = value;
        }
    }

    public enum Reaction{ NULL, POSITIVE, NEGATIVE, NEUTRAL}

    public GameObject responsiveObject;

    Camera mainCamera;

    private void Awake()
    {
        instanceCameraMove = this;
        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        thinkTime = 60;
        ResetGesture();
    }

    public static Reaction ActivateNodding()
    {
        instanceCameraMove.reaction = Reaction.NULL;
        instanceCameraMove.checkReaction();
        return instanceCameraMove.reaction;
    }
 
    void checkReaction()
    {
        angles[index] = mainCamera.transform.eulerAngles;
        index++;
        if (index == thinkTime)
        {
            CheckMovement();
            CheckMovement();
            ResetGesture();
        }
    }

    //checks wether the player has moved his head up and down or to the left and right 
    void CheckMovement()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;


        bool right = false, left = false, up = false, down = false; 
        for(int i =0; i < thinkTime; i++)
        {
            if(angles[i].x < centerAngle.x - 20f && !up)
            {
                up = true;
            } else if (angles[i].x > centerAngle.x - 20f && !down)
            {
                down = true;
            }

            if (angles[i].y < centerAngle.y - 20f && !left)
            {
                left = true;
            }
            else if (angles[i].y > centerAngle.y - 20f && !right)
            {
                right = true;
            }

            if(left && right &&!(up && down))
            {
                //Player shook their head left  & right to indicate 'no'
                //Debug.Log("NO");
                reaction = Reaction.NEGATIVE;


                responsiveObject.GetComponent<Renderer>().material.color = Color.red;
            }
            if (up && down && !(left && right))
            {
                //Player shook their head left & right to indicate 'no'
                //Debug.Log("YES");
                reaction = Reaction.POSITIVE;

                responsiveObject.GetComponent<Renderer>().material.color = Color.green;

            }
        }        
    }

    /*void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = new Color(1f, 1f, 1f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }*/

    void ResetGesture()
    {
        //Debug.Log("reset");
        angles = new Vector3[thinkTime];
        index = 0;
        centerAngle = mainCamera.transform.eulerAngles;
    }
}
