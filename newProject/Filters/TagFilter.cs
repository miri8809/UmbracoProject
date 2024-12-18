using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Models;

namespace newProject.Filters
{
    public class TagFilter : IFilterHandler, IContentIndexHandler
    {
        private const string FilterSpecifier = "tag:";
        private const string FieldName = "tags";

        // Querying
        public bool CanHandle(string query)
              => query.StartsWith(FilterSpecifier, StringComparison.OrdinalIgnoreCase);
        public FilterOption BuildFilterOption(string filter)
        {
            var fieldValue = filter.Substring(FilterSpecifier.Length);

            // There might be several values for the filter
            var values = fieldValue.Split(',');

            return new FilterOption
            {
                FieldName = FieldName,
                Values = values,
                Operator = FilterOperation.Contains
            };
        }

        // Indexing
        public IEnumerable<IndexFieldValue> GetFieldValues(IContent content, string? culture)
        {
            var props = content.Properties;
            if (props == null || props.Count == 0)
                return Array.Empty<IndexFieldValue>();
            string fieldValue = string.Empty;
            if (props.Contains(FieldName) && props[FieldName].Values.Count > 0)
            {
                fieldValue = props[FieldName].Values.First().PublishedValue?.ToString();
            }

            if (string.IsNullOrEmpty(fieldValue))
                return Array.Empty<IndexFieldValue>();
            return new[]
            {
                    new IndexFieldValue
                    {
                        FieldName = FieldName,
                        Values = new object[] { fieldValue }
                    },
                };
        }

        public IEnumerable<IndexField> GetFields() => new[]
    {
            new IndexField
            {
                FieldName = FieldName,
                FieldType = FieldType.StringRaw,
                VariesByCulture = false
            },
        };
    }
}

