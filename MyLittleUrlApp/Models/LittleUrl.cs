using System;
using System.ComponentModel.DataAnnotations;

namespace MyLittleUrlApp.Models
{
    public class LittleUrl
    {
        // Object id from data store
        //public string _id
        //{
        //    get;
        //    set;
        //}

        [Display(Name="Id")]
        public int UrlId
        {
            get;
            set;
        }

        [DataType(DataType.Url)]
        [Display(Name="Long Url")]
        public string LongUrl
        {
            get;
            set;
        }

        [DataType(DataType.Text)]
        [Display(Name="Short Key")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Key must be 3 characters in length.")]
        public string ShortUrl
        {
            get;
            set;
        }
    }
}
