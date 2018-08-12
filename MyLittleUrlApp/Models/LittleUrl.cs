using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MyLittleUrlApp.Models
{
    public class LittleUrl
    {
        //public class ObjectId
        //{
        //    [JsonProperty("creationTime")]
        //    [Display(Name="Created on (UTC)")]
        //    public DateTime CreationTime
        //    { 
        //        get;
        //        set;
        //    }
        //}

        //// Object id from data store
        //public ObjectId _id
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

        [Display(Name="Created on (UTC)")]
        public DateTime CreationTime
        { 
            get;
            set;
        }

    }
}
