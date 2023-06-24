using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Salesforce;
using Unity.FPS.Game;

public class OpportunityRepsoitory : MonoBehaviour, IObjectRepository
{
    [HideInInspector]
    public Opportunity oppToInsert;

    [HideInInspector]
    public Opportunity oppToUpdate;

    [HideInInspector]

    public Opportunity oppToDelete;

    [HideInInspector]
    public List<Opportunity> opportunities;

    SalesforceClient salesforceClient;

    void Awake()
    {
        salesforceClient = GameObject.Find("SalesforceClient").GetComponent<SalesforceClient>();
        EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
    }

    public IEnumerator GetRecords()
    {
        Coroutine<List<Opportunity>> getRecordsRoutine = this.StartCoroutine<List<Opportunity>>(
           salesforceClient.query<Opportunity>(Opportunity.BASE_QUERY + " ORDER BY Name LIMIT 5")
       );
        yield return getRecordsRoutine.coroutine;
        opportunities = getRecordsRoutine.getValue();
    }

    public IEnumerator UpdateRecord()
    {
        Coroutine<Opportunity> updateRecordRoutine = this.StartCoroutine<Opportunity>(
            salesforceClient.update((oppToUpdate))
        );
        yield return updateRecordRoutine.coroutine;
    }

    public IEnumerator CreateRecord()
    {
        Coroutine<Opportunity> insertRecordRoutine = this.StartCoroutine<Opportunity>(
            salesforceClient.insert(oppToInsert)
        );
        yield return insertRecordRoutine.coroutine;
    }

    public IEnumerator DeleteRecord()
    {
        Coroutine<Opportunity> deleteRecordRoutine = this.StartCoroutine<Opportunity>(salesforceClient.delete(oppToDelete));
        yield return deleteRecordRoutine.coroutine;
    }

    void UpdateOppClosedLost()
    {
        oppToUpdate.stage = "Closed Lost";
        StartCoroutine(UpdateRecord());
    }

    void OnPlayerDeath(PlayerDeathEvent evt) => UpdateOppClosedLost();
}
