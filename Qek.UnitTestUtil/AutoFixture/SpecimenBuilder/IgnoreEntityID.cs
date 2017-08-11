using Ploeh.AutoFixture.Kernel;
using System;
using System.Reflection;

namespace Qek.UnitTestUtil
{
    public class IgnoreEntityID : Ploeh.AutoFixture.Kernel.ISpecimenBuilder
    {
        public object Create(object request, Ploeh.AutoFixture.Kernel.ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            PropertyInfo pi = request as PropertyInfo;
            if (pi == null)
            {
                return new NoSpecimen();
            }

            if (pi.Name.Equals("ID"))
            {
                return null;
            }

            return new NoSpecimen();
        }
    }
}
