using Salesforce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUI : MonoBehaviour
{

    [SerializeField]
    SalesforceClient salesforceClient;

    IEnumerator CreateAccount()
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

        // Create sample account
        Account accountRecord = new Account(null, "Test account");
        Coroutine<Account> insertAccountRoutine = this.StartCoroutine<Account>(
            salesforceClient.insert(accountRecord)
        );
        yield return insertAccountRoutine.coroutine;
        insertAccountRoutine.getValue();
        Debug.Log("Account created");
    }

    public void OnCreateAccount()
    {
        StartCoroutine(CreateAccount());
    }
}
