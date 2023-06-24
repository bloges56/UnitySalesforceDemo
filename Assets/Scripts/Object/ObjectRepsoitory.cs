using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectRepsoitory
{
    IEnumerator GetRecords();

    IEnumerator CreateRecord();

    IEnumerator UpdateRecord();

    IEnumerator DeleteRecord();

}
