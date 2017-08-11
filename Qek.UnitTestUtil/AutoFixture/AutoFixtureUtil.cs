using Ploeh.AutoFixture;
using System;

namespace Qek.UnitTestUtil
{
    public static class AutoFixtureUtil
    {
        public static Fixture GetFixture(bool ignoreEntityID = true, int stringLength = 30)
        {
            var fixture = new Fixture();
            if (ignoreEntityID) fixture.Customizations.Add(new IgnoreEntityID());
            fixture.Customizations.Add(
                new StringGenerator(() =>
                    Guid.NewGuid().ToString().Substring(0, stringLength)));

            return fixture;
        }
    }
}
