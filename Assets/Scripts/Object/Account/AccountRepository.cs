using Salesforce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountRepository : MonoBehaviour, IObjectRepository
{
    [HideInInspector]
    public Account accountToInsert = new Account();

    [HideInInspector]
    public Account accountToUpdate = new Account();

    [HideInInspector]

    public Account accountToDelete = new Account();

    [HideInInspector]
    public List<Account> accounts;

    [SerializeField] SalesforceClient salesforceClient;

    public IEnumerator GetRecords()
    {
        Coroutine<List<Account>> getRecordsRoutine = this.StartCoroutine<List<Account>>(
           salesforceClient.query<Account>(Account.BASE_QUERY + " ORDER BY Name LIMIT 5")
       );
        yield return getRecordsRoutine.coroutine;
        accounts = getRecordsRoutine.getValue();
    }

    public IEnumerator UpdateRecord()
    {
        Coroutine<Account> updateRecordRoutine = this.StartCoroutine<Account>(
            salesforceClient.update((accountToUpdate))
        );
        yield return updateRecordRoutine.coroutine;
    }

    public IEnumerator CreateRecord()
    {
        Coroutine<Account> insertRecordRoutine = this.StartCoroutine<Account>(
            salesforceClient.insert(accountToInsert)
        );
        yield return insertRecordRoutine.coroutine;
    }

    public IEnumerator DeleteRecord()
    {
        Coroutine<Account> deleteRecordRoutine = this.StartCoroutine<Account>(salesforceClient.delete(accountToDelete));
        yield return deleteRecordRoutine.coroutine;
    }
}
