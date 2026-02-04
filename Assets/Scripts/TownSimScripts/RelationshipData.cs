[System.Serializable]
public class RelationshipData
{
    public ID ID1;
    public ID ID2;
    public float Value;

    public bool Involves(ID idA, ID idB)
    {
        return (ID1 == idA && ID2 == idB) || (ID1 == idB && ID2 == idA);
    }

    public RelationshipData(ID id1, ID id2, float startingAmount = 0)
    {
        ID1 = id1;
        ID2 = id2;
        Value = startingAmount;
    }
}
