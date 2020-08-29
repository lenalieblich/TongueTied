using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OttScript : MonoBehaviour
{
    public AudioClip[] OttAudio;
    public CameraMovement.Reaction[] positivAnswers;

    private AudioSource myAudio;
    private Animator ott_animator;
    private int index=0;
    private int audioInt;
    private string debugText="hier der Text";
    private bool animationStopped;
    private ObjectFocus[] focusObjects;


    public ParticleSystem leuchtstrahlen;
    private ParticleSystem.MainModule main;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInParent<ObjectFocus>().initialTrigger = false;
        ott_animator = GetComponent<Animator>();
        myAudio = GetComponent<AudioSource>();

        focusObjects = FindObjectsOfType<ObjectFocus>();        

        main = leuchtstrahlen.main;
    }

    // Update is called once per frame
    void Update()
    {

        if (animationStopped)
        {
            if (!leuchtstrahlen.isPlaying)
            {
                leuchtstrahlen.Play();
            }
            CameraMovement.Reaction theReaction = CameraMovement.ActivateNodding();
            if(theReaction != CameraMovement.Reaction.NULL)
            {
                if(positivAnswers[index] == CameraMovement.Reaction.NEUTRAL )
                {
                    CharacterReaction(CameraMovement.Reaction.NEUTRAL);
                }
                else if (theReaction == positivAnswers[index])
                {
                    CharacterReaction(CameraMovement.Reaction.POSITIVE);
                } else
                {
                    CharacterReaction(CameraMovement.Reaction.NEGATIVE);
                }
                //Debug.Log("here i am" + theReaction);
            } 

        }

    }

    IEnumerator ChangeColor(Color color)
    {
        main.startColor = new ParticleSystem.MinMaxGradient(color);

        yield return new WaitForSeconds(3);
        main.startColor = new ParticleSystem.MinMaxGradient(Color.white);
        leuchtstrahlen.Stop();

    }

    void CharacterReaction(CameraMovement.Reaction reaction)
    {
        index++;


        if (index == 0 && reaction != CameraMovement.Reaction.NULL)
        {
            //ObjectFocusManager.EndStartScreen();
        }

        if (reaction == CameraMovement.Reaction.POSITIVE)
        {
            StartCoroutine(ChangeColor(Color.green));
            ObjectFocusManager.score++;
            //debugText = "YAAA";
        }
        else if (reaction == CameraMovement.Reaction.NEGATIVE)
        {
            StartCoroutine(ChangeColor(Color.red));
            //debugText = "NOOO";
        }
        else if (reaction == CameraMovement.Reaction.NEUTRAL)
        {
            StartCoroutine(ChangeColor(Color.white));
        }
        else
        {
            return;
        }
        ott_animator.SetBool("YES", true);
        ott_animator.SetBool("animationStopped", false);
        ott_animator.enabled = true;
        animationStopped = false;
        StartCoroutine(PlayAudio());
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(Screen.width / 2 , Screen.height / 2, 200, 200), debugText + "  " + ott_animator.GetBool("animationStopped") + " "  +  index + " " );
        //GUI.Label(new Rect(Screen.width / 2 , Screen.height / 2, 200, 200), ObjectFocusManager.score.ToString());
    }

    IEnumerator PlayAudio()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(Audio());        
    }

    IEnumerator Audio()
    {
         if (OttAudio.Length > index)
        {
            //Debug.Log("Der INDEX" + OttAudio.Length); 
            myAudio.clip = OttAudio[index];
            myAudio.Play();
            var currentClip = ott_animator.GetCurrentAnimatorClipInfo(0);
            //Debug.Log(currentClip[0].clip.name);
            yield return new WaitForSeconds(ott_animator.GetCurrentAnimatorStateInfo(0).length);

            if (index == OttAudio.Length -1)
            {
                foreach (ObjectFocus obj in focusObjects)
                {
                    obj.isTrigger = true;
                }
                ObjectFocusManager.Instance.IncrementCounter();
                GetComponentInParent<ObjectFocus>().isTrigger = false;
            }
            else
            {
                StopAnimation();

                
            }

        }
       
    }

    void StopAnimation()
    {
        debugText = "ist gestoppt";
        ott_animator.enabled = false;
        ott_animator.SetBool("animationStopped", true);
        Debug.Log("ich stoppe die animation");
        animationStopped = true;

    }

    public void EnableOtherInteractions()
    {
        foreach (ObjectFocus obj in focusObjects)
        {
            obj.isTrigger = false;
        }
    }

    public void StartAnimation()
    {
        if (ott_animator.GetBool("startAnimation") == false)
        {
            ott_animator.SetBool("startAnimation", true);
            ott_animator.SetBool("animationStopped", false);
            StartCoroutine(PlayAudio());

        }
    }
}
