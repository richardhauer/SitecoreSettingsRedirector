namespace PING.Feature.SitecoreConfigurationOverrideSystem.Models
{
    public class XpathReplacement
    {
        public string Xpath { get; set; }
        public ActionType Action { get; set; }
        public string AttributenName { get; set; }
        public string NewValue { get; set; }

        public enum ActionType
        {
            Add,
            UpdateText,
            UpdateAttribute,
            Remove
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Xpath)
                    &&
                    (
                        (
                            Action == ActionType.UpdateAttribute && !string.IsNullOrEmpty(AttributenName) && !string.IsNullOrEmpty(NewValue)
                        )
                        ||
                        (
                            Action == ActionType.UpdateText && !string.IsNullOrEmpty(NewValue)
                        )
                        ||
                        (
                            Action == ActionType.Add || Action == ActionType.Remove
                        )
                    );
        }
    }
}