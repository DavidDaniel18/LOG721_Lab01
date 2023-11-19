using Domain.Grouping;
using Domain.Publicity;

namespace Domain.Services;

public static class GroupServices
{
    public static Group GetClosestGroupByBarycentre(Space space, List<Group> groups)
    {
        return groups.OrderBy(g => Math.Abs(g.Barycentre - space.GetNormalizedValue())).First();
    }
}