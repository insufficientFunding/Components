using JetBrains.Annotations;
using Serialization;
using Serialization.Converters;
using Serialization.Tests.Converters;
using Serialization.Tests.Models;
using System;
using System.IO;
using Xunit;
namespace Serialization.Tests;

[TestSubject (typeof (Serializer))]
public class SerializerTest
{
    [Fact]
    public async void TestSerialize ()
    {
        var converterService = new Serializer ();
        var testClass = new TestClass { Name = "Test" };

        converterService.RegisterConverter<TestClassConverter> ();

        var result = await converterService.SerializeAsync (testClass);
        
        string filePath = Path.Combine (GetProjectDirectory (), "test.json");
        
        await using FileStream fileStream = File.Open (filePath, FileMode.Create);
        await using StreamWriter streamWriter = new StreamWriter (fileStream);
        
        await streamWriter.WriteAsync (result);

        Assert.NotEqual (string.Empty, result);
    }

    private static string GetProjectDirectory ()
    {
        string workingDirectory = Environment.CurrentDirectory;
        
        return Directory.GetParent(workingDirectory).Parent.Parent.FullName;
    }

}
