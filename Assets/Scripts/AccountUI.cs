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
    List<Account> accounts;
    Account accountToUpdate = new Account();

    [SerializeField] TMP_Text updateAccountPlaceholderText;

    protected override void SetupRecordList()
    {
        foreach (Account account in accounts)
        {
            var newRecord = Instantiate(recordUIPrefab, recordsParent).transform.GetComponent<AccountRecordGraphic>();
            newRecord.Setup(account, this);
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
        updateAccountPlaceholderText.text = account.name;
    }

    public void UpdateName(string updateNameString)
    {
        accountToUpdate.name = updateNameString;
    }

    protected override IEnumerator CreateRecord()
    {
        yield return base.CreateRecord();
        // Create account
        Coroutine<SalesforceRecord> insertRecordRoutine = this.StartCoroutine<SalesforceRecord>(
            salesforceClient.insert(accountToInsert)
        );
        yield return insertRecordRoutine.coroutine;
        insertRecordRoutine.getValue();
    }

    protected override IEnumerator UpdateRecord()
    {
        yield return base.UpdateRecord();
        Coroutine<SalesforceRecord> updateRecordRoutine = this.StartCoroutine<SalesforceRecord>(
            salesforceClient.update(accountToUpdate)
        );
        yield return updateRecordRoutine.coroutine;
        updateRecordRoutine.getValue();
        OnExitUpdateRecord();
    }
}
