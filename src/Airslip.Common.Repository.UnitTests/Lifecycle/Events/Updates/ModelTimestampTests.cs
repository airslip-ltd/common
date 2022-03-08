using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations.Events.Model.PreProcess;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.UnitTests.Common;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Repository.UnitTests.Lifecycle.Events.Updates;

public class ModelTimestampTests
{
    [Theory]
    [InlineData(LifecycleStage.Update)]
    [InlineData(LifecycleStage.Create)]
    [InlineData(LifecycleStage.Delete)]
    public async Task Update_acts_as_expected(
        LifecycleStage lifecycleStage)
    {
        IModelPreProcessEvent<MyModelWithTimeStamp> preProcessEvent =
            new ModelTimeStampEvent<MyModelWithTimeStamp>();
            
        MyModelWithTimeStamp updatedEntity = await preProcessEvent
            .Execute(new MyModelWithTimeStamp()
            {
                TimeStamp = 5
            }, lifecycleStage);

        updatedEntity.TimeStamp.Should().NotBe(5);
    }
    
    [Theory]
    [InlineData(LifecycleStage.Update)]
    [InlineData(LifecycleStage.Create)]
    [InlineData(LifecycleStage.Delete)]
    public async Task Update_acts_as_expected_when_interface_not_implemented(
        LifecycleStage lifecycleStage)
    {
        IModelPreProcessEvent<MyModel> preProcessEvent =
            new ModelTimeStampEvent<MyModel>();
            
        MyModel updatedEntity = await preProcessEvent
            .Execute(new MyModel()
            {
                TimeStamp = 5
            }, lifecycleStage);

        updatedEntity.TimeStamp.Should().Be(5);
    }
}