# FakeO
## Fake Object data population for .NET

This simple utility library can be used to rig up data objects in .NET.
The intention is that it be used in conjunction with other mocking tools, like Moq.

Originally, I ran into needing to mock a
  List<MyObject>
where MyObject was an object with over 60 properties.
I really only cared about the actual value of a handful of the properties,
and the rest were free to be any fake data.

What I wanted was an easy way to say "Give me 10 of MyObject, with Property1 set to XYZ,
and Property2 set to 123, and put any fake data in the other properties" in a nice concice way.

This is often done in Ruby with the Faker gem, so I wanted something simmilar in .NET.

----------------------------

Most of the stuff under FakeO is a copy of this .NET port of Ruby's Faker gem:
  https://github.com/slashdotdash/faker-cs/
  
FakeO.String.Random() is a port of Perl String::Random from here:
  http://cpansearch.perl.org/src/STEVE/String-Random-0.22/lib/String/Random.pm


## Using FakeO

The main 'entry point' of using FakeO is the FakeO.Create class.
This class contains 2 groups of methods, the .New and .Fake methods.

The difference between the two is that the .New set of methods will
create new object instances, but will not insert fake data into the
instances properties and fields.

The .Fake methods will also create object instances, but will insert
fake data into all fields and properties.

Both the .New and .Fake methods take 0 or more Action<T> parameters.
Each of these is defined as a lambda expression in your code.
Each of these actions is then run against each created object instance
before FakeO returns it to you.

FakeO will set data on any fields and properties that are both:

+ either public or internal
+ have a setter (in the case of properties)

In addition, FakeO contains a set of static classes and methods that
can be used to retreive fake data. This makes up all the other classes
in FakeO other than FakeO.Create.
(Most of these are a fork of https://github.com/slashdotdash/faker-cs/)
These include:

+ Addresses (number, street, city, state)
+ Person Names (first and/or last)
+ Company Names
+ Phone Numbers
+ Lorem Ipsum style text (words, scentences, paragraphs)
+ Numbers (int, long, float, double, etc.)
+ Enum values (selects one value of an enum at random)
+ GUIDs
+ Strings (filled with random characters)
+ DateTimes (picks a random date between the years 1900 and 2100)
+ TimeSpans (picks a random timespan between 0 and 10 days)

A value for most built-in types can be retreived by simply doing:

```c#
var rndInt   = FakeO.Data.Random<int>();
var rndFloat = FakeO.Data.Random<float>();
var rndDate  = FakeO.Data.Random<DateTime>();
```
  ...etc...


## Examples
  
### Example 1:
**Get a single instance of an object.**

```c#
// example object class
class Company()
{
  public string Name { get; set; }
  public string Phone { get; set; }
  public int EmployeeCount { get; set; }
}

// example FakeO call
var comp = FakeO.Create.Fake<Company>(
                c => c.Name = FakeO.Company.Name(),
                c => c.Phone = FakeO.Phone.Number(),
                c => c.EmployeeCount = FakeO.Number.Next(100,200)); // random number from 100 to 200

// tests
Assert.IsNotNull(comp.Name);                                           // the Name property was set
Assert.IsTrue(comp.Phone.Length >= 12);                                // phone number is at least 12 chars (may or may not have area code)
Assert.IsTrue(comp.EmployeeCount >= 100 && comp.EmployeeCount <= 200); // EmployeeCount is between 100 and 200
```

### Example 2:
**Set some properties. Fake rest.**

```c#
// example FakeO call
var comp = FakeO.Create.Fake<Company>(
                c => c.Phone = "123-567-9012");

// tests
Assert.IsNotNull(comp.Name);                 // the Name property was set
Assert.AreEqual("123-567-9012", comp.Phone); // phone number is 12 characters ("123-567-9012")
Assert.IsTrue(comp.EmployeeCount > 0);       // EmployeeCount was set to a random number
```

### Example 3:
**Random strings based on RegEx.**

```c#
// example FakeO call
var comp = FakeO.Create.New<Company>(
                 c => c.Name = FakeO.Fake.Random(@"[A-Z][a-z]{6}"));

// tests
Assert.IsTrue(Regex.IsMatch(comp.Name, @"[A-Z][a-z]{6}")); // the Name property was set to 1 uppercase and 6 lowercase.
Assert.AreEqual(default(string), comp.Phone);              // phone number was not set. left at default
Assert.AreEqual(default(int), comp.EmployeeCount);         // EmployeeCount was not set. left at default
```

### Example 4:
**List of items.**

```c#
// example FakeO call
var companies = FakeO.Create.New<Company>(5, // generate a list of length 5
                      c => c.Phone = FakeO.Fake.PhoneNumber());

// tests
Assert.IsTrue(companies is IEnumerable); // .New(int) returns an IEnumerable
Assert.AreEqual(5, companies.Count());
```

### Example 5:
**Used in conjunction with Moq.**

```c#
// return 5 fake addresses when IAddressLookup.LoadAddresses() is called.

var addressLookupMock = new Mock<IAddressLookup>();
addressLookupMock.Setup< List<Address> >(x => x.LoadAddresses()).Returns(
                  FakeO.Create.Fake<Address>(5,
                    x => x.Address1 = FakeO.Address.StreetAddress(),
                    x => x.AttentionTo = FakeO.Name.FullName(),
                    x => x.City = FakeO.Address.City(),
                    x => x.Country = "US",
                    x => x.State = FakeO.Address.UsStateAbbr(),
                    x => x.Zip = FakeO.String.Random(@"\d{5}")
                  ).ToList());
```
