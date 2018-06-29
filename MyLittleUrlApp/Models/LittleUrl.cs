using System;
using System.ComponentModel.DataAnnotations;

namespace MyLittleUrlApp.Models
{
    public class LittleUrl
    {
        public int Id
        {
            get;
            set;
        }

        [DataType(DataType.Url)]
        public string LongUrl
        {
            get;
            set;
        }

        [StringLength(3, MinimumLength = 3)]
        public string ShortUrl
        {
            get;
            set;
        }
    }
}
