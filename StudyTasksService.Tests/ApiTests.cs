using Xunit;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using StudyTasksService.Models;

namespace StudyTasksService.Tests
{
    public class ApiTests
    {
        private readonly HttpClient _client;

        public ApiTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7006")
            };
        }

        [Fact]
        public async Task GetAllItems_ReturnsEmptyListInitially()
        {
            var response = await _client.GetAsync("/api/items");
            response.EnsureSuccessStatusCode();

            var items = await response.Content.ReadFromJsonAsync<List<StudyTask>>();
            Assert.NotNull(items);
            Assert.Empty(items);
        }

        [Fact]
        public async Task CreateItem_WithValidData_ReturnsCreatedItem()
        {
            var newTask = new CreateTaskRequest
            {
                Title = "Learn ASP.NET",
                Difficulty = 3
            };

            var response = await _client.PostAsJsonAsync("/api/items", newTask);
            response.EnsureSuccessStatusCode();

            var createdItem = await response.Content.ReadFromJsonAsync<StudyTask>();
            Assert.NotNull(createdItem);
            Assert.Equal("Learn ASP.NET", createdItem.Title);
            Assert.Equal(3, createdItem.Difficulty);
        }

        [Fact]
        public async Task CreateItem_WithInvalidData_ReturnsError()
        {
            var newTask = new CreateTaskRequest
            {
                Title = "",
                Difficulty = -1
            };

            var response = await _client.PostAsJsonAsync("/api/items", newTask);
            Assert.False(response.IsSuccessStatusCode);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            Assert.NotNull(error);
            Assert.NotEmpty(error.RequestId);
            Assert.NotEmpty(error.Message);
            Assert.NotEmpty(error.ErrorCode);
        }
    }
}