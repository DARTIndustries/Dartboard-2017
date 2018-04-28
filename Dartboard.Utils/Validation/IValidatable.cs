using Dartboard.Integration;

namespace Dartboard.Utils.Validation
{
    public interface IValidatable
    {
        bool Validate(AbstractRobot robot);
    }
}
