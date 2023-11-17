using Domain.Common.Seedwork.Abstract;

namespace Domain.Publicity;

public sealed class Space : Aggregate<Space>
{
    public int Width { get; }

    public int Price { get; }

    public string? GroupId { get; set; }

    public Space(string id, int width, int price, string? groupId = null) : base(id)
    {
        Width = width;
        Price = price;
        GroupId = groupId;
    }

    public double GetNormalizedValue()
    {
        return Math.Sqrt(Math.Pow(Width,2) + Math.Pow(Price, 2));
    }

    public override string ToString()
    {
        return $"Id: {Id}, Width: {Width}, Price: {Price}$" + ((GroupId != null) ? $", GroupdId: {GroupId}" : "");
    }
}