﻿using SleekFlowTodoListCore.Domain.Database.EntityTypes;
using SleekFlowTodoListCore.Domain.Database.Users;
using System.ComponentModel.DataAnnotations;

namespace SleekFlowTodoListCore.Domain.Database.Todos
{
    public class Todo : AuditableEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Status { get; set; }

        public User? User { get; set; }
        [Required]
        public Guid UserId { get; set; }
    }
}