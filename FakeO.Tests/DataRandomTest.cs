using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FakeO.Tests
{
  /// <summary>
  /// Tests the FakeO.Data.Random() method against various data types.
  /// </summary>
  [TestClass]
  public class DataRandomTest
  {
    [TestMethod]
    public void CanGetString()
    {
      Assert.IsFalse(string.IsNullOrEmpty(FakeO.Data.Random<string>()));
    }

    [TestMethod]
    public void CanGetNumber()
    {
      Assert.AreNotEqual(0, FakeO.Data.Random<int>());
    }

    [TestMethod]
    public void CanGetFloat()
    {
      Assert.AreNotEqual(0f, FakeO.Data.Random<float>());
    }

    [TestMethod]
    public void CanGetBool()
    {
      FakeO.Data.Random<bool>();
    }

    [TestMethod]
    public void CanGetNullableBool()
    {
      FakeO.Data.Random<bool?>();
    }

    [TestMethod]
    public void CanGetEnum()
    {
      Assert.IsTrue(Enum.IsDefined(typeof(TestEnum), FakeO.Data.Random<TestEnum>()));
    }

    [TestMethod]
    public void CanGetNullableEnum()
    {
      TestEnum? e = FakeO.Data.Random<TestEnum?>();
      Assert.IsTrue( e == null || Enum.IsDefined(typeof(TestEnum), e.Value));
    }

  }
}
