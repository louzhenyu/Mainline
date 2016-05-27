//Copyright (c) Service Stack LLC. All Rights Reserved.
//License: https://raw.github.com/ServiceStack/ServiceStack/master/license.txt

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ServiceStack
{
	[DataContract]
	public class Property
	{
        [DataMember]
		public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }
	}

    //modified by Yang Li: modify Properties into PropertyList
	[CollectionDataContract(ItemName = "Property")]
	public class PropertyList : List<Property>
	{
		public PropertyList() { }
		public PropertyList(IEnumerable<Property> collection) : base(collection) { }
	}
}