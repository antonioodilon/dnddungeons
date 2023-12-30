namespace DndDungeons.API.OtherFunctions
{
    public class OtherFunctionsToUse
    {
        public bool CompareGuids(Guid firstGuid, Guid secondGuid)
        {
            bool guidsAreEqual = true;

            for (int i = 0; i < firstGuid.ToString().Length; ++i)
            {
                for (int j = 0; j < secondGuid.ToString().Length; ++j)
                {
                    if ((firstGuid.ToString()[i] != secondGuid.ToString()[j]) && (i == j))
                    {
                        guidsAreEqual = false;
                        return guidsAreEqual;
                        //break;
                    }
                }
                //Console.WriteLine(another_guid.ToString()[i]);
            }

            /*if (guidsAreEqual)
            {
                Console.WriteLine("Guids are equal");
            }
            else
            {
                Console.WriteLine("Guids are different");
            }*/

            return guidsAreEqual;
        }

        /*public dynamic SearchDomainObjectIdFromList(List<dynamic> listObjects, Guid objectId)
        {
            dynamic objectToReturn = null;

            for (int i = 0; i < listObjects.Count; ++i)
            {
                if (CompareGuids(listObjects[i].Id, objectId))
                {
                    //idsAreEqual = true;
                    objectToReturn = listObjects[i];
                    return objectToReturn;
                    //break;
                    //return Ok(regionToFind);
                }
            }

            return objectToReturn;
        }*/
    }
}
