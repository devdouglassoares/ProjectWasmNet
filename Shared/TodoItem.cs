// TodoItem.cs
using System;

namespace ProjectBlazor.Shared
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public int Value { get; set; }
        public bool IsDone { get; set; }
    }
}
