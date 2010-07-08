using TestifyTDD;
using Tests.TestingDomain;

namespace Tests.TestDataBuilders
{
    public class BuilderEventFiredMock :
        TestDataBuilder<BuilderEventFiredDummy, BuilderEventFiredMock>
    {
        public BuilderEventFiredMock()
        {
            OnPostBuildEventFired = false;
        }

        public PostBuildEventArgs<BuilderEventFiredDummy> PostBuildEventArgs { get; private set; }
        public bool OnPostBuildEventFired { get; private set; }

        public override void OnPostBuild(object sender, PostBuildEventArgs<BuilderEventFiredDummy> eventArgs)
        {
            OnPostBuildEventFired = true;
            PostBuildEventArgs = eventArgs;
        }
    }
}
