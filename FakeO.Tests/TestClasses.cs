using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FakeO.Tests
{
  public enum TestEnum
  {
    Value1,
    Value2,
    Value3
  }

  internal class InternalTestClass
  {
    internal string Prop1 { get; set; }
    internal string Field1;
  }

  public class PublicTestClass
  {
    internal string Prop1 { get; set; }
    internal string Field1;
  }

  public class PublicTestClassProxy : PublicTestClass
  {
    internal new string Prop1
    {
      get { return base.Prop1; }
      set { base.Prop1 = value; }
    }
    internal new string Field1;
  }

  public class EnumTestClass
  {
    public TestEnum Enum1;
    public TestEnum Enum2 { get; set; }
  }

  public class NoSettersTestClass
  {
    public string Property1 { get { return null; } }
    public string Property2 { get; private set; }
  }

  public class DateTimeSpanTestClass
  {
    public DateTime Date;
    public TimeSpan Time;
    public TimeSpan ManuallySetTime;
  }

  public class NestedTestClass
  {
    public PublicTestClass Nested { get; set; }
    public long Property1 { get; set; }
  }
}
