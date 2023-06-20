using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpportunityUI : ObjectUI
{
    [SerializeField] OpportunityRepsoitory oppRepo;
    Dictionary<Opportunity, OpportunityRecordGraphic> opportunityGraphics = new Dictionary<Opportunity, OpportunityRecordGraphic>();

    public override IEnumerator SetupRecordList()
    {
        base.SetupRecordList();
        yield return oppRepo.GetRecords();
        foreach (Opportunity opportunity in oppRepo.opportunities)
        {
            var newRecord = Instantiate(recordUIPrefab, recordsParent).transform.GetComponent<OpportunityRecordGraphic>();
            newRecord.Setup(opportunity, this);
            opportunityGraphics.Add(opportunity, newRecord);
        }
    }

    public void SetName(string name)
    {
        oppRepo.oppToInsert.name = name;
    }

    public void SetStage(TMP_Text stage)
    {
        Debug.Log(stage.text);
        oppRepo.oppToInsert.stage = stage.text;
    }

    public void SetCloseDate(string closeDate)
    {
        oppRepo.oppToInsert.closeDate = closeDate;
    }

    public void OnSelectOpportunity(Opportunity opportunity)
    {
        OnSelectRecord();
        oppRepo.oppToUpdate = opportunity;
        updateObjectPlaceholderText.text = opportunity.name;
    }

    public void UpdateName(string updateNameString)
    {
        oppRepo.oppToUpdate.name = updateNameString;
    }

    protected override IEnumerator CreateRecordUI()
    {
        yield return oppRepo.CreateRecord();
        var newRecord = Instantiate(recordUIPrefab, recordsParent).transform.GetComponent<OpportunityRecordGraphic>();
        newRecord.Setup(oppRepo.oppToInsert, this);
        opportunityGraphics.Add(oppRepo.oppToInsert, newRecord);
    }

    protected override IEnumerator UpdateRecordUI()
    {
        yield return oppRepo.UpdateRecord();

        OnExitUpdateRecord();
    }

    public void InitiateDeleteOpportunity(Opportunity opp)
    {
        InitiateDeleteRecord();
        oppRepo.oppToDelete = opp;
        deleteObjectNameText.text = oppRepo.oppToDelete.name;
    }
    protected override IEnumerator DeleteRecordUI()
    {
        yield return oppRepo.DeleteRecord();
        Destroy(opportunityGraphics[oppRepo.oppToDelete].gameObject);
        opportunityGraphics.Remove(oppRepo.oppToDelete);

    }
}
