using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using TaskFlow;

namespace TaskFlow.Tests
{
    public class ProgramTests : IClassFixture<WebApplicationFactory<ProgramEntryPoint>>
    {
        private readonly WebApplicationFactory<ProgramEntryPoint> _factory;

        public ProgramTests(WebApplicationFactory<ProgramEntryPoint> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_HomePage_ReturnsSuccess()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
