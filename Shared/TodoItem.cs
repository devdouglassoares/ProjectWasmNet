// TodoItem.cs
using System;

namespace ProjectBlazor.Shared
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public decimal Value { get; set; }
        public decimal Unidad {get; set;}
        public bool Green {get; set;}
        public bool IsDone { get; set; }
        public DateTime DateInsert {get; set;}
        public string? UserId { get; set; }
    }
}
