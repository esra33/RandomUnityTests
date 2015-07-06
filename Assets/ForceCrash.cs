using UnityEngine;
using System.Collections;

public class ForceCrash : MonoBehaviour {

    public void ForceCrashInstance()
    {
        Debug.Log("This is a log test");
        Debug.LogError("This is an Error test");
        Debug.LogWarning("This is a Warning test");

        ForceCrash instance = this.RequestOne();

        Debug.Log("This is the value of i = " + instance.RequestOne());
    }

    public ForceCrash RequestOne()
    {
        return null;
    }
}
