using Salesforce;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SalesforceClient))]
public class ConnectionUI : MonoBehaviour
{
    //serialized input fields
    [SerializeField]
    Button submit;

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


    private void Start()
    {
        salesforceClient = GetComponent<SalesforceClient>();
    }
}


