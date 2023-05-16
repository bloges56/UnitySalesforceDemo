using Salesforce;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUI : MonoBehaviour
{

    [SerializeField]
    protected SalesforceClient salesforceClient;

    [SerializeField]
    PlayerMovement playerMovement;

    [SerializeField]
    CameraMovement cameraMovement;

    protected SalesforceRecord recordToInsert;

    [SerializeField] protected GameObject recordUIPrefab;
    [SerializeField] protected RectTransform recordsParent;

    [SerializeField] GameObject recordListUI;
    [SerializeField] GameObject createRecordUI;

    [SerializeField] ObjectInteraction objectInteraction;

    [SerializeField] GameObject updateRecordUI;


    protected virtual void SetupRecordList()
    {
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

    public virtual IEnumerator GetRecords()
    {
        yield return HandleLogin();
    }

    protected virtual IEnumerator CreateRecord()
    {
        yield return HandleLogin();
    }

    protected virtual IEnumerator UpdateRecord()
    {
        yield return HandleLogin();
    }

    protected void OnSelectRecord()
    {
        recordListUI.SetActive(false);
        updateRecordUI.SetActive(true);
    }

    public void OnUpdateRecord()
    {
        StartCoroutine(UpdateRecord());
    }

    public void OnCreateRecord()
    {
        OnExitCreateNewRecord();
        StartCoroutine(CreateRecord());
    }

    public void OnExit()
    {
        playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        cameraMovement.enabled = true;
        recordListUI.SetActive(false);
        createRecordUI.SetActive(false);
        updateRecordUI.SetActive(false);
        objectInteraction.enabled= true;
        foreach(Transform child in recordsParent.transform)
        {
           Destroy(child.gameObject);
        }
    }

    public void OnClickNewRecord()
    {
        recordListUI.SetActive(false);
        createRecordUI.SetActive(true);
    }

    public void OnExitCreateNewRecord()
    {
        recordListUI.SetActive(true);
        createRecordUI.SetActive(false);
    }

    public void OnExitUpdateRecord()
    {
        recordListUI.SetActive(true);
        updateRecordUI.SetActive(false);
    }


}
