using System.Text;
using NUnit.Framework;
using Packety.Packagers;
using Packety.Packets;
using Shouldly;

namespace Packety.Tests
{
    public class NetworkMessagePackagerShould
    {
        [Test]
        public void AddPacketDefinitionToDictionaryCorrectly()
        {
            var networkMessagePackager = new NetworkMessagePackager();

            networkMessagePackager.AddDefinition<TestPacket>();

            networkMessagePackager.GetPacketDefinition(0)
                .PacketType.ShouldBe(typeof(TestPacket));
        }

        [Test]
        public void CorrectlyIncrementPacketTypeIds()
        {
            var networkMessagePackager = new NetworkMessagePackager();

            var first = networkMessagePackager.AddDefinition<TestPacket>();
            var second = networkMessagePackager.AddDefinition<TestPacket2>();

            first.PacketTypeId.ShouldNotBe(second.PacketTypeId);
            second.PacketTypeId.ShouldBeGreaterThan(first.PacketTypeId);
        }

        [Test]
        public void PackageCorrectlyBasedOnType()
        {
            var networkMessagePackager = new NetworkMessagePackager();

            var testPacket = new TestPacket()
            {
                Number = 3,
                Name = "Test"
            };

            var definition = networkMessagePackager.AddDefinition<TestPacket>();

            var value = networkMessagePackager.Package(testPacket);

            var expectedString = $"{definition.PacketTypeId}:{testPacket.Number}:{testPacket.Name}";
            var outputString = Encoding.UTF8.GetString(value);

            outputString.ShouldBe(expectedString);
        }

        [Test]
        public void UnPackCorrectly()
        {
            var networkMessagePackager = new NetworkMessagePackager();

            var testPacket = new TestPacket()
            {
                Number = 3,
                Name = "Test"
            };

            networkMessagePackager.AddDefinition<TestPacket>();

            var value = networkMessagePackager.Package(testPacket);
            var result = (TestPacket) networkMessagePackager.Unpack(value);

            result.Name.ShouldBe(testPacket.Name);
            result.Number.ShouldBe(testPacket.Number);
        }

        [Test]
        public void UnPackEnumPropertyCorrectly()
        {
            var networkMessagePackager = new NetworkMessagePackager();

            var testPacket = new EnumPacket()
            {
                TestEnum = TestEnum.Test
            };

            networkMessagePackager.AddDefinition<EnumPacket>();

            var value = networkMessagePackager.Package(testPacket);
            var result = (EnumPacket) networkMessagePackager.Unpack(value);

            result.TestEnum.ShouldBe(testPacket.TestEnum);
        }
    }

    public class TestPacket : INetworkPacket
    {
        public int Number { get; set; }
        public string Name { get; set; }
    }

    public class TestPacket2 : INetworkPacket
    {
        public int Number { get; set; }
        public string Name { get; set; }
    }

    public enum TestEnum
    {
        None,
        Test
    }

    public class EnumPacket : INetworkPacket
    {
        public TestEnum TestEnum { get; set; }
    }
}