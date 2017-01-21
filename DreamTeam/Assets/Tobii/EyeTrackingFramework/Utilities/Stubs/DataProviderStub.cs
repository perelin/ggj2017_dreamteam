using System.Collections.Generic;

namespace Tobii.EyeTracking.Stubs
{
    public class DataProviderStub<T> : IDataProvider<T> where T : ITimestamped
    {
        // --------------------------------------------------------------------
        //  Implementation of IDataProvider<T>
        // --------------------------------------------------------------------

        public T Last { get; protected set; }

        public IEnumerable<T> GetDataPointsSince(ITimestamped dataPoint)
        {
            return new List<T>();
        }

        public T GetFrameConsistentDataPoint()
        {
            return Last;
        }

        public void Start(int subscriberId)
        {
            // no implementation
        }

        public void Stop(int subscriberId)
        {
            // no implementation
        }
    }

    public class GazePointDataProviderStub : DataProviderStub<GazePoint>
    {
        public GazePointDataProviderStub()
        {
            Last = GazePoint.Invalid;
        }
    }

    public class EyePositionDataProviderStub: DataProviderStub<EyePositions>
    {
        public EyePositionDataProviderStub()
        {
            Last = EyePositions.Invalid;
        }
    }
}
