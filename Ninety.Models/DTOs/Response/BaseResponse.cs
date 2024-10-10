using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Models.DTOs.Response
{
    public class BaseResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
        public object? Data { get; set; } = null!;
        public IDictionary<string, string[]>? Errors { get; set; } = null!;
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }
}
