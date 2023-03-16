using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Audit.WebApi.Template.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IValuesService _provider;

        public ValuesController(IValuesService provider)
        {
            _provider = provider;
        }

        // Getting NotSupportedException: The type 'Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1[System.String]' can only be serialized using async serialization methods.
        // See https://github.com/dotnet/runtime/issues/61044

        /*
         * System.Text.Json.ThrowHelper.ThrowNotSupportedException_TypeRequiresAsyncSerialization(Type propertyType)
         * System.Text.Json.Serialization.Converters.IAsyncEnumerableOfTConverter<TAsyncEnumerable, TElement>.OnTryWrite(Utf8JsonWriter writer, TAsyncEnumerable value, JsonSerializerOptions options, ref WriteStack state)
         * System.Text.Json.Serialization.JsonConverter<T>.TryWrite(Utf8JsonWriter writer, ref T value, JsonSerializerOptions options, ref WriteStack state)
         * System.Text.Json.Serialization.JsonConverter<T>.TryWriteAsObject(Utf8JsonWriter writer, object value, JsonSerializerOptions options, ref WriteStack state)
         * System.Text.Json.Serialization.JsonConverter<T>.TryWrite(Utf8JsonWriter writer, ref T value, JsonSerializerOptions options, ref WriteStack state)
         * System.Text.Json.Serialization.Metadata.JsonPropertyInfo<T>.GetMemberAndWriteJson(object obj, ref WriteStack state, Utf8JsonWriter writer)
         * System.Text.Json.Serialization.Converters.ObjectDefaultConverter<T>.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, ref WriteStack state)
         * System.Text.Json.Serialization.JsonConverter<T>.TryWrite(Utf8JsonWriter writer, ref T value, JsonSerializerOptions options, ref WriteStack state)
         * System.Text.Json.Serialization.Metadata.JsonPropertyInfo<T>.GetMemberAndWriteJson(object obj, ref WriteStack state, Utf8JsonWriter writer)
         * System.Text.Json.Serialization.Converters.ObjectDefaultConverter<T>.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, ref WriteStack state)
         * System.Text.Json.Serialization.JsonConverter<T>.TryWrite(Utf8JsonWriter writer, ref T value, JsonSerializerOptions options, ref WriteStack state)
         * System.Text.Json.Serialization.Metadata.JsonPropertyInfo<T>.GetMemberAndWriteJson(object obj, ref WriteStack state, Utf8JsonWriter writer)
         * System.Text.Json.Serialization.Converters.ObjectDefaultConverter<T>.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, ref WriteStack state)
         * System.Text.Json.Serialization.JsonConverter<T>.TryWrite(Utf8JsonWriter writer, ref T value, JsonSerializerOptions options, ref WriteStack state)
         * System.Text.Json.Serialization.JsonConverter<T>.WriteCore(Utf8JsonWriter writer, ref T value, JsonSerializerOptions options, ref WriteStack state)
         */

        // GET api/values
        // [HttpGet]
        // public ActionResult<IEnumerable<string>> Get()
        // {
        //     return Ok(_provider.GetValues());
        // }

        [HttpGet]
        public async IAsyncEnumerable<string?> GetAsync()
        {
            await foreach (var value in _provider.GetValuesAsync())
            {
                yield return value;
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            return Ok(await _provider.GetAsync(id));
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] string value)
        {
            return Ok(await _provider.InsertAsync(value));
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] string value)
        {
            await _provider.ReplaceAsync(id, value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            return Ok(await _provider.DeleteAsync(id));
        }

        // DELETE api/values/delete
        [HttpDelete]
        [Route("delete")]
        public async Task<ActionResult<bool>> Delete([FromBody] string ids)
        {
            return Ok(await _provider.DeleteMultipleAsync(ids.Split(',').Select(s => int.Parse(s)).ToArray()));
        }
    }
}
