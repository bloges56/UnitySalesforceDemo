using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpportunityUI : ObjectUI
{
    Opportunity opportunityToInsert = new Opportunity();
    Opportunity opportunityToUpdate = new Opportunity();
    Opportunity opportunityToDelete = new Opportunity();
    List<Opportunity> opportunities;
    Dictionary<Opportunity, OpportunityRecordGraphic> opportunityGraphics = new Dictionary<Opportunity, OpportunityRecordGraphic>();
    protected override void SetupRecordList()
    {
        base.SetupRecordList();
        foreach (Opportunity opportunity in opportunities)
        {
            var newRecord = Instantiate(recordUIPrefab, recordsParent).transform.GetComponent<OpportunityRecordGraphic>();
            newRecord.Setup(opportunity, this);
            opportunityGraphics.Add(opportunity, newRecord);
        }
    }

    public override IEnumerator GetRecords()
    {
        yield return base.GetRecords();
        // Get some records
        Coroutine<List<Opportunity>> getRecordsRoutine = this.StartCoroutine<List<Opportunity>>(
            base.salesforceClient.query<Opportunity>(Opportunity.BASE_QUERY + " ORDER BY Name LIMIT 5")
        );
        yield return getRecordsRoutine.coroutine;
        opportunities = getRecordsRoutine.getValue();
        SetupRecordList();
    }

    public void SetName(string name)
    {
        opportunityToInsert.name = name;
    }

    public void SetStage(TMP_Text stage)
    {
        Debug.Log(stage.text);
        opportunityToInsert.stage = stage.text;
    }

    public void SetCloseDate(string closeDate)
    {
        opportunityToInsert.closeDate = closeDate;
    }

    public void OnSelectOpportunity(Opportunity opportunity)
    {
        OnSelectRecord();
        opportunityToUpdate = opportunity;
        updateObjectPlaceholderText.text = opportunity.name;
    }

    public void UpdateName(string updateNameString)
    {
        opportunityToUpdate.name = updateNameString;
    }

    protected override IEnumerator CreateRecord()
    {
        yield return base.CreateRecord();
        // Create opportunity
        Coroutine<Opportunity> insertRecordRoutine = this.StartCoroutine<Opportunity>(
            salesforceClient.insert(opportunityToInsert)
        );
        yield return insertRecordRoutine.coroutine;
        var newRecord = Instantiate(recordUIPrefab, recordsParent).transform.GetComponent<OpportunityRecordGraphic>();
        newRecord.Setup(opportunityToInsert, this);
        opportunityGraphics.Add(opportunityToInsert, newRecord);
    }

    protected override IEnumerator UpdateRecord()
    {
        yield return base.UpdateRecord();
        Coroutine<Opportunity> updateRecordRoutine = this.StartCoroutine<Opportunity>(
            salesforceClient.update((opportunityToUpdate))
        );
        yield return updateRecordRoutine.coroutine;
        opportunityGraphics[opportunityToUpdate].Setup(opportunityToUpdate, this);

        OnExitUpdateRecord();
    }

    public void InitiateDeleteOpportunity(Opportunity acc)
    {
        InitiateDeleteRecord();
        opportunityToDelete = acc;
        deleteObjectNameText.text = opportunityToDelete.name;
    }
    protected override IEnumerator DeleteRecord()
    {
        yield return base.DeleteRecord();
        Coroutine<Opportunity> deleteRecordRoutine = this.StartCoroutine<Opportunity>(
            salesforceClient.delete(opportunityToDelete)
        );
        yield return deleteRecordRoutine.coroutine;
        Destroy(opportunityGraphics[opportunityToDelete].gameObject);

    }
}
