﻿using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Forms.Implementation.Elements.BaseClasses;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using MarketingAutomation.ODP.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MarketingAutomation.ODP.Models
{
    [ContentType(DisplayName = "ODP List Form Block", GroupName = "ODP", GUID = "fed7d217-d2df-4c6e-9cb3-9290bdb2ece5", Description = "")]
    public class ODPListFormBlock : HiddenElementBlockBase
    {
        [Display(Name = "List Id", Order = 1)]
        [SelectOne(SelectionFactoryType = typeof(ODPListSelectionFactory))]
        public virtual string ListId { get; set; }
    }

    public class ODPListSelectionFactory : ISelectionFactory
    {
        private Injected<ODPService> ODPService;

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata) =>
            ODPService.Service.GetLists()
                   .OrderBy(x => x.Name)
                   .Select(x => new SelectItem
                   {
                       Text = x.Name,
                       Value = x.ListId
                   });
    }
}