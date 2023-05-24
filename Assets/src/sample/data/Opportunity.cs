using Boomlagoon.JSON;
using Salesforce;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opportunity : SalesforceRecord
{
    public const string BASE_QUERY = "SELECT Id, Name, CloseDate, StageName FROM Opportunity";

    public string name { get; set; }
    public string stage { get; set; }
    public string closeDate { get; set; }

    public Opportunity() { }

    public Opportunity(string id, string name, string stage, string closeDate) : base(id)
    {
        this.name = name;
        this.stage = stage;
        this.closeDate = closeDate;
    }

    public override string getSObjectName()
    {
        return "Opportunity";
    }

    public override JSONObject toJson()
    {
        JSONObject record = base.toJson();
        record.Add("Name", name);
        record.Add("CloseDate", closeDate);
        record.Add("StageName", stage);
        return record;
    }

    public override void parseFromJson(JSONObject jsonObject)
    {
        base.parseFromJson(jsonObject);
        name = jsonObject.GetString("Name");
        closeDate = jsonObject.GetString("CloseDate");
        stage = jsonObject.GetString("StageName");
    }
}
