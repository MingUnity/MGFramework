using System;

public interface ISampleAModel
{
    void Request(Action<string> callback);
}