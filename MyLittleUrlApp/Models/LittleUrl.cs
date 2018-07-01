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

        [DataType(DataType.Text)]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Key must be 3 characters in length.")]
        public string ShortUrl
        {
            get;
            set;
        }
    }
}
