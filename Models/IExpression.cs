namespace AdventOfCode2022.Models
{
    public interface IExpression
    {
        public int GoesBefore(IExpression expression);
    }

    public class IntegerExpression : IExpression
    {
        public int Value { get; }

        public IntegerExpression(int value)
        {
            Value = value;
        }

        public int GoesBefore(IExpression expression)
        {
            if(expression is IntegerExpression other)
            {
                if (Value == other.Value)
                    return 0;
                if (Value < other.Value)
                    return 1;
                return -1;
            }
            return -expression.GoesBefore(this);
        }
    }

    public class ListExpression : IExpression
    {
        public IExpression[] Expressions { get; }

        public ListExpression(params IExpression[] expressions)
        {
            Expressions = expressions;
        }

        public int GoesBefore(IExpression expression)
        {
            if(expression is IntegerExpression other)
            {
                return GoesBefore(new ListExpression(other));
            }
            var otherList = (ListExpression) expression;
            for(int i = 0; i < Expressions.Length; i++)
            {
                if (i >= otherList.Expressions.Length)
                    return -1;
                var res = Expressions[i].GoesBefore(otherList.Expressions[i]);
                if(res != 0)
                    return res;
            }
            if(otherList.Expressions.Length > Expressions.Length)
                return 1;
            return 0;
        }
    }
}