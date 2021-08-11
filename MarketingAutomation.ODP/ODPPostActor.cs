using EPiServer;
using EPiServer.Core;
using EPiServer.Forms.Core.PostSubmissionActor;
using EPiServer.Forms.Implementation.Elements;
using EPiServer.ServiceLocation;
using MarketingAutomation.ODP.Models;
using MarketingAutomation.ODP.Services;
using System.Collections.Generic;
using System.Linq;

namespace MarketingAutomation.ODP
{
    public class ODPPostActor : PostSubmissionActorBase
    {
        private string _objectType;

        private readonly Injected<IODPService> _odpService;

        private readonly Injected<IContentLoader> ContentLoader;

        public override object Run(object input)
        {
            string str = string.Empty;
            bool validProfileSave = false;
            string listId = string.Empty;
            bool consentGiven = false;
            string email = string.Empty;

            if (this.SubmissionData == null)
            {
                return str;
            }

            Dictionary<string, string> postedFormDictionary = new Dictionary<string, string>();
            foreach (KeyValuePair<string, object> pair in this.SubmissionData.Data)
            {
                if (!pair.Key.ToLower().StartsWith("systemcolumn") && pair.Value != null)
                {
                    postedFormDictionary.Add(pair.Key, pair.Value.ToString());
                }
            }

            var mappings = base.ActiveExternalFieldMappingTable;

            if (mappings != null)
            {
                Dictionary<string, string> formAttributes = new Dictionary<string, string>();

                foreach (var item in mappings)
                {
                    if (item.Value != null)
                    {
                        var fieldName = item.Key;
                        var remoteFieldName = item.Value.ColumnId;

                        if (postedFormDictionary.ContainsKey(fieldName))
                        {
                            formAttributes.Add(remoteFieldName, postedFormDictionary[fieldName]);

                            if (remoteFieldName.Equals("email", System.StringComparison.OrdinalIgnoreCase))
                            {
                                email = postedFormDictionary[fieldName];
                            }
                        }
                    }
                }

                if (formAttributes.Any() && !string.IsNullOrWhiteSpace(email))
                {
                    validProfileSave = this._odpService.Service.SaveProfileInformation(email, formAttributes);
                }

                var currentForm = ContentLoader.Service.Get<IContent>(FormIdentity.Guid);
                if (currentForm != null && currentForm is FormContainerBlock formContainerBlock)
                {
                    if (formContainerBlock.ElementsArea.Items.Any())
                    {
                        var items = formContainerBlock.ElementsArea.Items.Select(x => ContentLoader.Service.Get<IContent>(x.ContentLink));

                        var list = items.FirstOrDefault(x => x is ODPListFormBlock);
                        if (list != null && list is ODPListFormBlock listFormBlock)
                        {
                            listId = listFormBlock.ListId;
                        }

                        var listConsent = items.FirstOrDefault(x => x is ODPListConsentFormBlock);
                        if (listConsent != null)
                        {
                            var id = $"__field_{listConsent.ContentLink.ToReferenceWithoutVersion().ID}";
                            if (SubmissionData.Data.ContainsKey(id))
                            {
                                var consentStringValue = SubmissionData.Data[id].ToString();
                                if (bool.TryParse(SubmissionData.Data[id].ToString(), out bool consentGivenValue))
                                {
                                    consentGiven = consentGivenValue;
                                }
                                else if (consentStringValue.Equals("Yes", System.StringComparison.OrdinalIgnoreCase) || consentStringValue.Equals("y", System.StringComparison.OrdinalIgnoreCase) || consentStringValue.Equals("on", System.StringComparison.OrdinalIgnoreCase))
                                {
                                    consentGiven = true;
                                }
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(listId) && validProfileSave)
            {
                var listSubscription = new ODPListSubscribeRequest()
                {
                    ListId = listId,
                    Email = email,
                    Subscribed = consentGiven
                };
                this._odpService.Service.AddToList(listSubscription);
            }

            return str;
        }

        public string ObjectType
        {
            get
            {
                return this._objectType;
            }

            set
            {
                this._objectType = value;
            }
        }
    }
}