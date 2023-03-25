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

    public TMP_Text username;

    public TMP_Text password;

    [SerializeField]
    TMP_Text consumerKey;

    [SerializeField]
    TMP_Text consumerSecret;

    IEnumerator Connect()
    {
        // Get Salesforce client component
        SalesforceClient salesforceClient = GetComponent<SalesforceClient>();
        //salesforceClient.consumerKey = consumerKey.text;
        //salesforceClient.consumerSecret = consumerSecret.text;
        Coroutine<bool> loginRoutine = this.StartCoroutine<bool>(
        salesforceClient.login(username.text.ToString(), password.text.ToString())
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

}


