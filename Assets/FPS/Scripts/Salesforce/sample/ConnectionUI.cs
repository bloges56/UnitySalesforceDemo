using Salesforce;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionUI : MonoBehaviour
{
    //serialized input fields
    [SerializeField]
    Button submit;

    [SerializeField]
    GameObject playerCam;

    [SerializeField]
    GameObject player;
    [SerializeField]
    SalesforceClient salesforceClient;

    IEnumerator Connect()
    {
        // Get Salesforce client component 
        //salesforceClient.consumerKey = consumerKey.text;
        //salesforceClient.consumerSecret = consumerSecret.text;
        Coroutine<bool> loginRoutine = this.StartCoroutine<bool>(
        salesforceClient.login()
    );
        yield return loginRoutine.coroutine;
        try
        {
            loginRoutine.getValue();
            Debug.Log("Salesforce login successful.");
            gameObject.SetActive(false);
            player.SetActive(true);
            playerCam.SetActive(true);
        }
        catch (SalesforceConfigurationException e)
        {
            Debug.Log("Salesforce login failed due to invalid auth configuration");
            throw e;
        }
        catch (SalesforceAuthenticationException e)
        {
            Debug.Log("Salesforce login failed due to invalid credentials");
            throw e;
        }
        catch (SalesforceApiException e)
        {
            Debug.Log("Salesforce login failed");
            throw e;
        }
    }

    public void OnSubmit()
    {
        StartCoroutine(Connect());
    }

    private void Awake()
    {
        if(PlayerPrefs.HasKey("username"))
        {
            StartCoroutine(Connect());
        }
    }
}


