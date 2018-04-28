using System;
using System.Linq;
using Dartboard.Integration;

namespace Dartboard.Utils.Validation
{
    public abstract class BaseValidator : LoggingObject, IValidatable
    {
        /// <summary>
        /// Validate the current class, and all applicable properties.
        /// 
        /// When overriding, call base method to perform automatic validation.
        /// </summary>
        /// <returns></returns>
        public virtual bool Validate(AbstractRobot robot)
        {
            Log.Trace("Beginning Automatic Validation");

            // Get all properties of the current type which implement IValidatable
            var props = GetType()
                .GetProperties()
                .Where(x => typeof(IValidatable).IsAssignableFrom(x.PropertyType));

            foreach (var propertyInfo in props)
            {
                Log.Trace("Validating Property: " + propertyInfo.Name);

                var property = propertyInfo.GetValue(this) as IValidatable;

                if (property == null)
                {
                    Log.Trace("Property is null. Checking Attributes...");
                    if (!Attribute.IsDefined(propertyInfo, typeof(ValidIfNullAttribute)))
                    {
                        Log.Warn("Null property found which was not marked with [ValidIfNull]; failing.");
                        return false;
                    }

                    Log.Trace("Property is allowed to be null. Continuing.");
                    continue;
                }

                if (!property.Validate(robot))
                {
                    Log.Warn("Validation Failed on " + propertyInfo.Name);
                }
            }

            return true;
        }
    }
}