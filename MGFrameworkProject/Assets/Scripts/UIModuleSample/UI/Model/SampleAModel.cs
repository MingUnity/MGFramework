using System;

public class SampleAModel : ISampleAModel
{
    public void Request(Action<string> callback)
    {
        callback?.Invoke("Title");
    }
}
