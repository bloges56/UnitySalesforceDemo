using Salesforce;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Profiling;

public class AccountUI : ObjectUI
{
    Account accountToInsert = new Account();
    Account accountToUpdate = new Account();
    Account accountToDelete = new Account();
    List<Account> accounts;
    Dictionary<Account, AccountRecordGraphic> accountGraphics =  new Dictionary<Account, AccountRecordGraphic>();
    protected override void SetupRecordList()
    {
        base.SetupRecordList();
        foreach (Account account in accounts)
        {
            var newRecord = Instantiate(recordUIPrefab, recordsParent).transform.GetComponent<AccountRecordGraphic>();
            newRecord.Setup(account, this);
            accountGraphics.Add(account, newRecord);
        }
    }

    public override IEnumerator GetRecords()
    {
        yield return base.GetRecords();
        // Get some records
        Coroutine<List<Account>> getRecordsRoutine = this.StartCoroutine<List<Account>>(
            base.salesforceClient.query<Account>(Account.BASE_QUERY + " ORDER BY Name LIMIT 5")
        );
        yield return getRecordsRoutine.coroutine;
        accounts = getRecordsRoutine.getValue();
        SetupRecordList();
    }

    public void SetName(string name)
    {
        accountToInsert.name = name;
    }

    public void OnSelectAccount(Account account)
    {
        OnSelectRecord();
        accountToUpdate = account;
        updateObjectPlaceholderText.text = account.name;
    }

    public void UpdateName(string updateNameString)
    {
        accountToUpdate.name = updateNameString;
    }

    protected override IEnumerator CreateRecord()
    {
        yield return base.CreateRecord();
        // Create account
        Coroutine<Account> insertRecordRoutine = this.StartCoroutine<Account>(
            salesforceClient.insert(accountToInsert)
        );
        yield return insertRecordRoutine.coroutine;
        var newRecord = Instantiate(recordUIPrefab, recordsParent).transform.GetComponent<AccountRecordGraphic>();
        newRecord.Setup(accountToInsert, this);
        accountGraphics.Add(accountToInsert, newRecord);
    }

    protected override IEnumerator UpdateRecord()
    {
        yield return base.UpdateRecord();
        Coroutine<Account> updateRecordRoutine = this.StartCoroutine<Account>(
            salesforceClient.update((accountToUpdate))
        );
        yield return updateRecordRoutine.coroutine;
        accountGraphics[accountToUpdate].Setup(accountToUpdate, this);
        
        OnExitUpdateRecord();
    }

    public void InitiateDeleteAccount(Account acc)
    {
        InitiateDeleteRecord();
        accountToDelete = acc;
        deleteObjectNameText.text = accountToDelete.name;
    }
    protected override IEnumerator DeleteRecord()
    {
        yield return base.DeleteRecord();
        Coroutine<Account> deleteRecordRoutine = this.StartCoroutine<Account>(
            salesforceClient.delete(accountToDelete)
        );
        yield return deleteRecordRoutine.coroutine;
        Destroy(accountGraphics[accountToDelete].gameObject);
        
    }
}
