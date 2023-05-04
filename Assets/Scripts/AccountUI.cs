using Salesforce;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Profiling;

public class AccountUI : ObjectUI
{
    Account accountToInsert;
    List<Account> accounts;

    protected override void SetupRecordList()
    {
        foreach (Account account in accounts)
        {
            var newRecord = Instantiate(recordUIPrefab, recordsParent).transform.GetComponent<AccountRecordGraphic>();
            newRecord.Setup(account);
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

    protected override IEnumerator CreateRecord()
    {
        yield return base.CreateRecord();

        // Create account
        Coroutine<SalesforceRecord> insertRecordRoutine = this.StartCoroutine<SalesforceRecord>(
            salesforceClient.insert(accountToInsert)
        );
        yield return insertRecordRoutine.coroutine;
        insertRecordRoutine.getValue();
        Debug.Log("Account Created");
    }
}
