using System;

public interface ISampleBModel
{
    int Count { get; set; }

    Item this[int index] { get; }

    void Request(Action<Item[]> callback);
}