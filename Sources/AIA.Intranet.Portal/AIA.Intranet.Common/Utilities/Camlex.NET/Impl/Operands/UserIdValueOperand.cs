﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;

namespace AIA.Intranet.Common.Utilities.Camlex.Impl.Operands
{
    internal class UserIdValueOperand : GenericStringBasedValueOperand
    {
        public UserIdValueOperand(string value)
            : base(typeof(DataTypes.UserId), value)
        {
            int id;
            if (!int.TryParse(value, out id))
            {
                throw new InvalidValueForOperandTypeException(value, this.Type);
            }
        }

        public override XElement ToCaml()
        {
            // use User type both for User operand and UserId operand
            return
                new XElement(Tags.Value, new XAttribute(Attributes.Type, typeof(DataTypes.User).Name),
                    new XText(this.Value));
        }

//        public override Expression ToExpression()
//        {
//            return Expression.Constant(this.Value);
//        }
    }
}
