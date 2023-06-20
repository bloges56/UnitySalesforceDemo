using Salesforce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectRepository
{
    IEnumerator GetRecords();
    IEnumerator CreateRecord();
    IEnumerator UpdateRecord();
    IEnumerator DeleteRecord();
}
