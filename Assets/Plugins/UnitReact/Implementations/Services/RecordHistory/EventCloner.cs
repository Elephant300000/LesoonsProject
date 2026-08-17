using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Lucky38.UnitReact.Core
{
    public static class EventCloner<TContainer> where TContainer : class, IPoolableEvent, new()
    {
        private static readonly Action<TContainer, TContainer> CopyFieldsDelegate;

        static EventCloner()
        {
            var type = typeof(TContainer);
            var sourceParam = Expression.Parameter(type, "source");
            var targetParam = Expression.Parameter(type, "target");

            var expressions = new List<Expression>();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var sourceField = Expression.Field(sourceParam, field);
                var targetField = Expression.Field(targetParam, field);
                expressions.Add(Expression.Assign(targetField, sourceField));
            }

            if (expressions.Count == 0)
                expressions.Add(Expression.Empty());

            var block = Expression.Block(expressions);
            CopyFieldsDelegate = Expression
                .Lambda<Action<TContainer, TContainer>>(block, sourceParam, targetParam)
                .Compile();
        }
         
        public static TContainer Clone(TContainer source)
        {
            if (source == null) return null;

            var target = EventPool<TContainer>.Get();
            CopyFieldsDelegate(source, target);
            return target;
        }
    }
}
