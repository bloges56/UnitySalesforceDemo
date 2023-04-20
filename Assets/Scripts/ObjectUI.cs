using Salesforce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUI : MonoBehaviour
{

    [SerializeField]
    SalesforceClient salesforceClient;

    [SerializeField]
    PlayerMovement playerMovement;

    [SerializeField]
    CameraMovement cameraMovement;

    Account accountToInsert = new Account(null, "Test Account");

    List<Account> accounts = new List<Account>();

    [SerializeField] RectTransform accountRecordsParent;
    [SerializeField] AccountRecordGraphic accountUIPrefab;

    private void SetupAccountList()
    {
        foreach (Account account in accounts)
        {
            Debug.Log(account.name);
            var newAccount = Instantiate(accountUIPrefab, accountRecordsParent);
            newAccount.Setup(account);
        }
    }

    public void SetName(string name)
    {
        accountToInsert.name = name;
    }

    IEnumerator HandleLogin()
    {
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

    public IEnumerator GetAccounts()
    {
        // Get some Accounts
        string query = Account.BASE_QUERY + " ORDER BY Name LIMIT 5";
        Coroutine<List<Account>> getAccountsRoutine = this.StartCoroutine<List<Account>>(
            salesforceClient.query<Account>(query)
        );
        yield return getAccountsRoutine.coroutine;
        accounts = getAccountsRoutine.getValue();
        SetupAccountList();
    }

    IEnumerator CreateAccount()
    {
        yield return HandleLogin();

        // Create account
        Coroutine<Account> insertAccountRoutine = this.StartCoroutine<Account>(
            salesforceClient.insert(accountToInsert)
        );
        yield return insertAccountRoutine.coroutine;
        insertAccountRoutine.getValue();
        Debug.Log("Account created named: " + accountToInsert.name);
    }

    IEnumerator EditAccunt()
    {
        yield return HandleLogin();

        // Create account
        Coroutine<Account> insertAccountRoutine = this.StartCoroutine<Account>(
            salesforceClient.insert(accountToInsert)
        );
        yield return insertAccountRoutine.coroutine;
        insertAccountRoutine.getValue();
        Debug.Log("Account created named: " + accountToInsert.name);
    }

    public void OnCreateAccount()
    {
        StartCoroutine(CreateAccount());
    }

    public void OnExit()
    {
        playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        cameraMovement.enabled = true;
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void ViewAccounts()
    {
        StartCoroutine(GetAccounts());
    }
}
