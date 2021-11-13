using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace BragaPets.Domain.Entities
{
    public abstract class Entity
    {
        public Guid Uid { get; protected set; }
        public Int64 Id { get; protected set; }
        public List<ValidationFailure> Errors { get; protected set; }
        public bool HasErrors() => Errors.Any();

        protected Entity()
        {
            Uid = Guid.NewGuid();
            Errors = new List<ValidationFailure>();
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo))
                return true;

            if (ReferenceEquals(null, compareTo))
                return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id = {Id}]";
        }
    }
}