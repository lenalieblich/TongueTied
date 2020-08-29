using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectFocusManager : MonoBehaviour
{
    List<ObjectFocus> objectsInRange = new List<ObjectFocus>();
    public GameObject startScreen;
    public GameObject endScreen;
    private bool openEyes = false;
    private bool crRunning;

    public static int interactionCounter;
    public void IncrementCounter()
    {
        interactionCounter++;
    }

    # region Singleton
    private static ObjectFocusManager _instance;

    public static ObjectFocusManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<ObjectFocusManager>();

                if(_instance == null)
                {
       
                    _instance = (new GameObject("ObjectFocusManager : Singleton")).AddComponent<ObjectFocusManager>(); 
                }
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }

    }
    #endregion

    #region Static Properties & Methods
    static public int Count { get { return Instance.objectsInRange.Count; } }

    private int _score;
    static public int score { get { return Instance._score; } set { Instance._score = value; } }


    static public void Remove (ObjectFocus objectFocus)
    {
        Instance.objectsInRange.Remove(objectFocus);
    }

    static public void Add (ObjectFocus objectFocus)
    {
        if (Instance.objectsInRange.Contains(objectFocus))
        {
            return;
        }

        Instance.objectsInRange.Add(objectFocus);
    }

    static void Sort()
    {

        if (Instance.objectsInRange.Count > 1)
            Instance.objectsInRange.Sort((a, b) => a.delta.CompareTo(b.delta));

        Instance.firstInList = Instance.objectsInRange.Count > 0 ? Instance.objectsInRange[0] : null;
        
    }

    public static void EndStartScreen()
    {
        Instance.startScreen.SetActive(false);
        Instance.openEyes = true;
    }
    #endregion

    #region Instance Properties & Methods
    private ObjectFocus _firstInList;
    public ObjectFocus firstInList
    {
        get { return _firstInList;  }
        private set
        {
            if (value != _firstInList)
            {
                if (_firstInList)
                    _firstInList.LostFocus();

                _firstInList = value;

                if (_firstInList)
                    _firstInList.GotFocus();
            }
        }
    }

    public void TriggerAction()
    {
        if (firstInList)
        {
            firstInList.ActionTrigger();
        }
    }

    IEnumerator OpenEyes()
    {
        bool theEyes = false;

        while(openEyes == false) {
            theEyes = !theEyes;
            float secs;

            if (theEyes)
            {
                secs = Random.Range(0.2f, 0.8f);
            } else
            {
                secs = Random.Range(2f, 6f);
            }

            yield return new WaitForSeconds(secs);
            if(openEyes == true)
            {
                Instance.startScreen.SetActive(false);
            }
            else
            {
                Instance.startScreen.SetActive(theEyes);
            }
        }
    }

    #endregion
    #region Mono
    private void Awake()
    {
        if(Instance && Instance != this)
        {
            Destroy(gameObject);

            Instance = this;
        }
        startScreen.SetActive(true);
    }

    private void OnDisable()
    {
        Instance = null; 
    }


    // Start is called before the first frame update
    void Start()
    {
        var myUIAssistant = startScreen.GetComponentInChildren<UIAssistant>();
        myUIAssistant.setText1("Can you hear me?");
        myUIAssistant.setText2("nod to continue");

        myUIAssistant.SetText();

        StartCoroutine(OpenEyes());
        
    }

    // Update is called once per frame
    void Update()
    {
        Sort();
        if(interactionCounter == 3)
        {
            if (crRunning == false)
            {
                StartCoroutine(EndGame());
            }  
        }
    }

    IEnumerator EndGame()
    {

        crRunning = true;
        
        yield return new WaitForSeconds(5);
        Instance.endScreen.SetActive(true);

        var myUIAssistant = Instance.endScreen.GetComponentInChildren<UIAssistant>();
        if (score < 4)
        {
            myUIAssistant.setText1("Work on your social skills!");
        }
        else
        {
            myUIAssistant.setText1("Welcome on board!");
        }
        myUIAssistant.setText2("thank you for playing");

        myUIAssistant.SetText();

    }

#if DEBUG
    private void OnGUI()
    {
        /*GUILayout.Label("Objects in Focus : " + Count.ToString() + interactionCounter);
        if (firstInList)
            GUILayout.Label("1st : " + firstInList.name); */
    }
#endif
#endregion
}
