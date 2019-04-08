using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Squirrel;

namespace SquirrelTests
{
    [TestClass]
    public class TestSetup
    {
        [TestMethod]
        public void TestFirstRun()
        {
            SquirrelAwareApp.HandleEvents(onFirstRun: () =>
            {
                
            });
            
        }
    }
}
