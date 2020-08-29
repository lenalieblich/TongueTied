using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateText : MonoBehaviour
{
    private bool isTriggered;
    private GameObject myPrefab;
    private GameObject instance; 
    private int index = 0;

    public void MakeText(GameObject prefab)
    {

        if (myPrefab == prefab && isTriggered)
        {
            return;
        }

            if (myPrefab != prefab && myPrefab != null)
        {
            Destroy(instance);
            isTriggered = false;
        }


        if (isTriggered == false)
        {
            myPrefab = prefab; 
            instance = Instantiate(prefab);
            isTriggered = true;
            DestroyText();
        }

    }

    private void DestroyText()
    {
        StartCoroutine(DestroyObject()); 
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(15);
        isTriggered = false;
        Destroy(instance);
    }

    public void DestroyText(GameObject text)
    {
        if (text != null)
        {
            if (text.GetComponentInParent<ObjectFocus>().initialTrigger == true)
            {
                ObjectFocusManager.Instance.IncrementCounter();
                text.GetComponentInParent<ObjectFocus>().initialTrigger = false;
            }

            Destroy(text);
        } else
        {
            return;
        }
    }
}
