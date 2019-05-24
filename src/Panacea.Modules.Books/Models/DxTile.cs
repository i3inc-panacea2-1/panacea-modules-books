using Panacea.Multilinguality;
using System;
using System.Runtime.Serialization;

namespace Panacea.Modules.Books.Models
{
    [DataContract]
    public class DxTile : Translatable
    {
        public DxTile()
        {
        }

        public DxTile(String header, String imgUri)
        {
            Name = header;
            Img = imgUri;
        }

        [IsTranslatable]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "img")]
        public String Img { get; set; }
    }
}
