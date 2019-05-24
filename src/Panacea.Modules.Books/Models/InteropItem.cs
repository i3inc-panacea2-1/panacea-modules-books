using Panacea.Models;
using Panacea.Multilinguality;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Panacea.Modules.Books.Models
{
    [DataContract]
    public class InteropItem : DxTile
    {
        [DataMember(Name = "dataUrl")]
        public List<DataUrl> DataUrl { get; set; }

        [IsTranslatable]
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "active")]
        public Boolean active { get; set; }

        [DataMember(Name = "educationType")]
        public String educationType { get; set; }

        [DataMember(Name = "img_thumbnail")]
        public Thumbnail ImgThumbnail { get; set; }

        [DataMember(Name = "priority")]
        public int priority { get; set; }
    }
}
