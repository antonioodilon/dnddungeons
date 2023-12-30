using DndDungeons.API.OtherFunctions;

namespace DndDungeons.API.Models.Domain
{
    public class DndRegion
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }

        public DndRegion SearchReturnRegion(List<DndRegion> listObjects, Guid objectId)
        {
            dynamic objectToReturn = null;
            OtherFunctionsToUse otherFunctions = new OtherFunctionsToUse();

            for (int i = 0; i < listObjects.Count; ++i)
            {
                if (otherFunctions.CompareGuids(listObjects[i].Id, objectId))
                {
                    //idsAreEqual = true;
                    objectToReturn = listObjects[i];
                    return objectToReturn;
                    //break;
                    //return Ok(regionToFind);
                }
            }

            return objectToReturn;
        }
    }
}
