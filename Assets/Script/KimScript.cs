using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KimScript : MonoBehaviour
{
    public AudioClip[] KimAudio;
    private AudioSource myAudio;
    private Animator kim_animator;
    private ObjectFocus[] focusObjects;
    private int index=0;
    private int audioInt;
    private string debugText="hier der Text";
    private bool animationStopped;

    
    public ParticleSystem leuchtstrahlen;
    private ParticleSystem.MainModule main;

    // Start is called before the first frame update
    void Start()
    {
        kim_animator = GetComponent<Animator>();
        myAudio = GetComponent<AudioSource>();

        focusObjects = FindObjectsOfType<ObjectFocus>();
        foreach(ObjectFocus obj in focusObjects)
        {
            obj.isTrigger = false; 
        }


        main = leuchtstrahlen.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (animationStopped)
        {

            if (!leuchtstrahlen.isPlaying) {
                leuchtstrahlen.Play();
            }

            CameraMovement.Reaction theReaction = CameraMovement.ActivateNodding();
            if(theReaction != CameraMovement.Reaction.NULL)
            {
                //Debug.Log("here i am" + theReaction);
                CharacterReaction(theReaction);
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
        bool theReaction;

        if (index == 0 && reaction != CameraMovement.Reaction.NULL)
        {
            ObjectFocusManager.EndStartScreen();
        }

        if (reaction == CameraMovement.Reaction.POSITIVE)
        {

            StartCoroutine(ChangeColor(Color.green));
            ObjectFocusManager.score++;
            index++;
            //debugText = "YAAA";
            theReaction = true;
        }
        else if (reaction == CameraMovement.Reaction.NEGATIVE)
        {
            StartCoroutine(ChangeColor(Color.red));
            index = index + 2;
            //debugText = "NOOO";
            theReaction = false;
        }
        else
        {
            return;
        }
        kim_animator.SetBool("YES", theReaction);
        kim_animator.SetBool("animationStopped", false);
        kim_animator.enabled = true;
        animationStopped = false;
        StartCoroutine(PlayAudio());
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(Screen.width / 2 , Screen.height / 2, 200, 200), debugText + "  " + kim_animator.GetBool("animationStopped") + " "  +  index + " " );
        //GUI.Label(new Rect(Screen.width / 2 , Screen.height / 2, 200, 200), ObjectFocusManager.score.ToString());
    }

    IEnumerator PlayAudio()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(Audio());        
    }

    IEnumerator Audio()
    {
         if (KimAudio.Length > index)
        {
            //Debug.Log("Der INDEX" + index);

            myAudio.clip = KimAudio[index];

            myAudio.Play();
            var currentClip = kim_animator.GetCurrentAnimatorClipInfo(0);
            //Debug.Log(currentClip[0].clip.name);
            yield return new WaitForSeconds(kim_animator.GetCurrentAnimatorStateInfo(0).length);

            if(index == KimAudio.Length - 2 || index == KimAudio.Length -1)
            {
                foreach (ObjectFocus obj in focusObjects)
                {
                    obj.isTrigger = true;
                }
                ObjectFocusManager.Instance.IncrementCounter();
            } else
            {
                StopAnimation();

                if (index % 2 > 0)
                {
                    index++;
                }
            }     
            
        }           
        
    }

    void StopAnimation()
    {
        debugText = "ist gestoppt";
        kim_animator.enabled = false;
        kim_animator.SetBool("animationStopped", true);
        animationStopped = true;
    }
}
