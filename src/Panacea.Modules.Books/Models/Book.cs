using Panacea.Models;
using Panacea.Multilinguality;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Panacea.Modules.Books.Models
{
    [DataContract]
    public class Book : ServerItem
    {
        public Book()
        {
            DataUrl = new List<DataUrl>();
        }

        private string _language;

        [IsTranslatable]
        [DataMember(Name = "language")]
        public string Language
        {
            get { return _language; }
            set
            {

                _language = value;
            }
        }

        [IsTranslatable]
        [DataMember(Name = "writer")]
        public string Writer { get; set; }

        [DataMember(Name = "year")]
        public string Year { get; set; }

        [DataMember(Name = "ISBN")]
        public string Isbn { get; set; }


        [DataMember(Name = "img")]
        public String Img { get; set; }

        [IsTranslatable]
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "dataUrl")]
        public List<DataUrl> DataUrl { get; set; }
    }

    [DataContract]
    public class DataUrl
    {
        public DataUrl()
        {
        }

        public DataUrl(String url, String dataType)
        {
            Url = url;
            DataType = dataType;
        }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "dataType")]
        public string DataType { get; set; }
    }
    public class AudioBookChapter
    {
        public String Title
        {
            get
            {
                if (Url == null)
                {
                    return "N/A";
                }
                String[] splits = Url.Split('/');
                String fname = splits[splits.Length - 1].Replace("_", " ");
                return fname.Substring(0, fname.LastIndexOf("."));
            }
        }

        public String Url { get; set; }
    }
}
