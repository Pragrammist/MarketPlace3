using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HukSleva.ViewModels.ValidationModel.Attributes
{
    public class RangeDateTimeAttribute : ValidationAttribute
    {
        public const string DateFormat = "d";
        DateTime _minDate;
        DateTime _maxDate;
        public RangeDateTimeAttribute(string minDate = "", string maxDate = "")
        {
            if (maxDate == "")
            {
                maxDate = DateTime.Now.ToString(DateFormat);
            }
            if (minDate == "")
            {
                maxDate = DateTime.MinValue.ToString(DateFormat);
            }
            bool maxIsParsed = DateTime.TryParse(maxDate, out _maxDate);
            bool minIsParsed = DateTime.TryParse(minDate, out _minDate);
            if (!maxIsParsed)
            {
                _minDate = DateTime.MinValue;
            }
            if (!minIsParsed)
            {
                _maxDate = DateTime.Now;
            }
        }
        public override bool IsValid(object value)
        {
            DateTime date;
            var isParsed = DateTime.TryParse(value.ToString(), out date);
            if (!isParsed)
            {
                return false;
            }
            if (date >= _minDate && date <= _maxDate)
            {
                return true;
            }
            return false;
        }
    }
}
