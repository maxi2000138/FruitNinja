using System.Collections.Generic;

public class SliceTrigger
{
    private List<ISlicable> _slicables = new();

    public void AddSliceObject(ISlicable slicable)
    {
        _slicables.Add(slicable);
    }
}
