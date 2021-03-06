// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.Sql.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The managed server capability
    /// </summary>
    public partial class ManagedInstanceEditionCapability
    {
        /// <summary>
        /// Initializes a new instance of the ManagedInstanceEditionCapability
        /// class.
        /// </summary>
        public ManagedInstanceEditionCapability()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ManagedInstanceEditionCapability
        /// class.
        /// </summary>
        /// <param name="name">The managed server version name.</param>
        /// <param name="supportedFamilies">The supported families.</param>
        /// <param name="status">The status of the capability. Possible values
        /// include: 'Visible', 'Available', 'Default', 'Disabled'</param>
        /// <param name="reason">The reason for the capability not being
        /// available.</param>
        public ManagedInstanceEditionCapability(string name = default(string), IList<ManagedInstanceFamilyCapability> supportedFamilies = default(IList<ManagedInstanceFamilyCapability>), CapabilityStatus? status = default(CapabilityStatus?), string reason = default(string))
        {
            Name = name;
            SupportedFamilies = supportedFamilies;
            Status = status;
            Reason = reason;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets the managed server version name.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        /// <summary>
        /// Gets the supported families.
        /// </summary>
        [JsonProperty(PropertyName = "supportedFamilies")]
        public IList<ManagedInstanceFamilyCapability> SupportedFamilies { get; private set; }

        /// <summary>
        /// Gets the status of the capability. Possible values include:
        /// 'Visible', 'Available', 'Default', 'Disabled'
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public CapabilityStatus? Status { get; private set; }

        /// <summary>
        /// Gets or sets the reason for the capability not being available.
        /// </summary>
        [JsonProperty(PropertyName = "reason")]
        public string Reason { get; set; }

    }
}
