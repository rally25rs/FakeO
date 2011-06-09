using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FakeO.Tests
{
  [TestClass]
  public class DistinctTest
  {
    [TestMethod]
    public void DistinctNumbersReturnASequence()
    {
      Assert.AreEqual(1, Distinct.Number());
      Assert.AreEqual(2, Distinct.Number());
      Assert.AreEqual(3, Distinct.Number());
      Assert.AreEqual(4, Distinct.Number());
      Assert.AreEqual(5, Distinct.Number());
    }
  }
}
