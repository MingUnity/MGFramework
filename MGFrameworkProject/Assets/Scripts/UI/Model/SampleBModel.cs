using MGFramework;
using System;

public class SampleBModel : ISampleBModel
{
    private Item[] _cache;

    public Item this[int index] => _cache?.GetValueAnyway(index);

    public int Count { get; set; }

    public void Request(Action<Item[]> callback)
    {
        Item[] items = new Item[Count];

        for (int i = 0; i < Count; i++)
        {
            items[i] = new Item()
            {
                index = i + 1,
                nickName = $"第{i + 1}号"
            };
        }

        _cache = items;

        callback?.Invoke(items);
    }
}
